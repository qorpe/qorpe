import axios, {
    AxiosError,
    AxiosHeaders,
    type AxiosInstance,
    type AxiosRequestConfig,
    type InternalAxiosRequestConfig,
} from "axios";
import type { AuthTokens } from "@/shared/types/auth";
import { useSessionStore } from "@/app/stores/session-store";

/** Type guard for AxiosHeaders. */
function isAxiosHeaders(h: unknown): h is AxiosHeaders {
    return !!h && typeof (h as AxiosHeaders).set === "function";
}

/** Safely set a header regardless of header shape. */
function setHeaderSafe(config: InternalAxiosRequestConfig, key: string, value: string): void {
    if (!value) return;
    if (!config.headers) config.headers = new AxiosHeaders();
    if (isAxiosHeaders(config.headers)) {
        config.headers.set(key, value);
    } else {
        (config.headers as Record<string, string>)[key] = value;
    }
}

/** Extract/set tokens through zustand (no direct import cycles). */
const tokenStore = {
    get: (): AuthTokens | null => useSessionStore.getState().tokens,
    set: (t: AuthTokens | null) => useSessionStore.getState().setTokens(t),
};

/** Create and configure the Axios instance with JWT & refresh flow. */
export const createHttp = (baseURL: string): AxiosInstance => {
    const instance = axios.create({
        baseURL,
        withCredentials: true,
        timeout: 15000,
    });

    // Request: attach auth and tenant headers
    instance.interceptors.request.use((config: InternalAxiosRequestConfig) => {
        const tokens = tokenStore.get();
        const tenant = useSessionStore.getState().selectedTenant;

        if (tokens?.accessToken) setHeaderSafe(config, "Authorization", `Bearer ${tokens.accessToken}`);
        if (tenant) setHeaderSafe(config, "X-Tenant-Key", tenant);

        return config;
    });

    // Response: single-flight refresh with queue
    let isRefreshing = false;
    const queue: Array<(t: string | null) => void> = [];
    const flush = (token: string | null) => {
        queue.forEach((r) => r(token));
        queue.length = 0;
    };

    instance.interceptors.response.use(
        (r) => r,
        async (error: AxiosError) => {
            const status = error.response?.status ?? 0;
            const original = error.config as AxiosRequestConfig & { _retry?: boolean };

            if (status !== 401 || original?._retry) return Promise.reject(error);
            original._retry = true;

            if (!isRefreshing) {
                isRefreshing = true;
                try {
                    const tokens = tokenStore.get();
                    if (!tokens?.refreshToken) {
                        tokenStore.set(null);
                        flush(null);
                        return Promise.reject(error);
                    }

                    // Adjust to your API path/payload as needed
                    const { data } = await axios.post<{ accessToken: string; refreshToken?: string }>(
                        `${baseURL}/auth/refresh`,
                        { refreshToken: tokens.refreshToken },
                        { withCredentials: true }
                    );

                    const next: AuthTokens = {
                        accessToken: data.accessToken,
                        refreshToken: data.refreshToken ?? tokens.refreshToken,
                    };
                    tokenStore.set(next);
                    flush(next.accessToken);
                } catch (e) {
                    tokenStore.set(null);
                    flush(null);
                    return Promise.reject(e);
                } finally {
                    isRefreshing = false;
                }
            }

            // Enqueue request until refresh finishes, then replay
            return new Promise((resolve, reject) => {
                queue.push((newAccess) => {
                    if (!newAccess) return reject(error);
                    original.headers = { ...(original.headers ?? {}), Authorization: `Bearer ${newAccess}` };
                    resolve(axios(original));
                });
            });
        }
    );

    return instance;
};

/** Export a singleton client; prefer injecting this in feature code. */
export const http = createHttp(import.meta.env.VITE_API_BASE_URL ?? "/api");
