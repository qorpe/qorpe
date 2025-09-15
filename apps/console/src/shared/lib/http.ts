import axios, {
    AxiosError,
    AxiosHeaders,
    type AxiosInstance,
    type AxiosRequestConfig,
    type InternalAxiosRequestConfig,
} from "axios";
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

/** Axios instance with cookie refresh and RAM access token. */
export const createHttp = (baseURL: string): AxiosInstance => {
    const instance = axios.create({
        baseURL,
        withCredentials: true, // send/receive cookies
        timeout: 15000,
    });

    // Attach Authorization & Tenant
    instance.interceptors.request.use((config: InternalAxiosRequestConfig) => {
        const { tokens, selectedTenant } = useSessionStore.getState();
        if (tokens?.accessToken) setHeaderSafe(config, "Authorization", `Bearer ${tokens.accessToken}`);
        if (selectedTenant) setHeaderSafe(config, "X-Tenant-Key", selectedTenant);
        return config;
    });

    // Single-flight refresh queue
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
            const { selectedTenant } = useSessionStore.getState();
            if (!selectedTenant) {
                // No tenant context; cannot refresh
                useSessionStore.getState().clearSession();
                return Promise.reject(error);
            }

            // Start a single refresh (cookie-based, no body)
            if (!isRefreshing) {
                isRefreshing = true;
                try {
                    const refreshUrl = `/t/${encodeURIComponent(selectedTenant)}/hub/v1/auth/refresh`;
                    const { data } = await axios.post<{ accessToken: string }>(
                        `${baseURL}${refreshUrl}`,
                        {},
                        { withCredentials: true }
                    );
                    useSessionStore.getState().applyNewAccessToken(data.accessToken);
                    flush(data.accessToken);
                } catch (e) {
                    useSessionStore.getState().clearSession();
                    flush(null);
                    return Promise.reject(e);
                } finally {
                    isRefreshing = false;
                }
            }

            // Enqueue the original request until refresh completes
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

/** Export a singleton client; */
export const http = createHttp(import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5223");
