import axios, {
    type AxiosInstance,
    AxiosHeaders,
    AxiosError,
    type InternalAxiosRequestConfig
} from 'axios';
import type { HttpOptions } from '@/shared/types/auth';
import { useSessionStore } from '@/app/stores/session-store';
import { useConfigStore } from '@/app/stores/config-store';
import { useAppStore } from "@/app/stores/app-store.ts";

/** Safely sets a header on Axios config regardless of headers shape. */
function setHeaderSafe(
    config: InternalAxiosRequestConfig,
    key: string,
    value?: string
): void {
    if (!value) return;
    if (!config.headers) config.headers = {};
    const h = config.headers as unknown;
    if (h instanceof AxiosHeaders || typeof (h as AxiosHeaders)?.set === 'function') {
        (h as AxiosHeaders).set(key, value);
    } else {
        (config.headers as Record<string, string>)[key] = value;
    }
}

/** Generates a short request id for tracing. */
function genRequestId(): string {
    return Math.random().toString(36).slice(2) + Date.now().toString(36);
}

/** Returns whether the HTTP method is idempotent and safe to auto-retry. */
function isIdempotent(method?: string): boolean {
    const m = (method ?? 'GET').toUpperCase();
    return m === 'GET' || m === 'HEAD' || m === 'OPTIONS';
}

/** Exponential backoff with jitter. */
function backoff(baseMs: number, attempt: number): number {
    const jitter = Math.random() * baseMs;
    return Math.pow(2, attempt - 1) * baseMs + jitter;
}

/** Shared in-flight refresh promise to avoid stampede. */
let refreshing: Promise<unknown> | null = null;

/** Runs refresh once and shares the result among concurrent 401s. */
async function runRefresh(onRefreshToken?: HttpOptions['onRefreshToken']) {
    if (!onRefreshToken) throw new Error('No refresh handler configured');
    if (!refreshing) {
        refreshing = onRefreshToken().finally(() => {
            refreshing = null;
        });
    }
    return refreshing;
}

/** Request interceptor: baseURL (dynamic), headers (auth/tenant/app/version, request-id). */
function makeRequestInterceptor(opts: HttpOptions) {
    return (config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
        if (typeof opts.baseURL === 'function') {
            config.baseURL = opts.baseURL();
        }

        // App metadata
        setHeaderSafe(config, 'X-Request-ID', genRequestId());
        if (opts.appName) setHeaderSafe(config, 'X-App-Name', opts.appName);
        if (opts.apiVersion) setHeaderSafe(config, 'X-Api-Version', opts.apiVersion);

        // Tenant & auth (store’dan veya ops’tan)
        const tenantKey = opts.getTenantKey?.() ?? useSessionStore.getState().tenantKey;
        if (tenantKey) setHeaderSafe(config, 'X-Tenant-Key', tenantKey);

        const token = opts.getToken?.() ?? useSessionStore.getState().accessToken;
        if (token) setHeaderSafe(config, 'Authorization', `Bearer ${token}`);

        return config;
    };
}

/** Response interceptor: 401 -> refresh once, then retry original request. */
function makeAuthResponseInterceptor(instance: AxiosInstance, opts: HttpOptions) {
    return async (error: AxiosError) => {
        const { response, config } = error;
        if (!response || response.status !== 401 || !config) throw error;

        if (config.__retryAfterRefresh) throw error;

        try {
            await runRefresh(opts.onRefreshToken);
            // Guard & retry
            config.__retryAfterRefresh = true;
            // Token header next request will be set by request interceptor (fresh token in store)
            return instance.request(config);
        } catch {
            throw error;
        }
    };
}

/** Response interceptor: retry idempotent requests on network/5xx with backoff. */
function makeRetryResponseInterceptor(instance: AxiosInstance, opts: HttpOptions) {
    const maxRetries = opts.maxRetries ?? 2;
    const base = opts.backoffBaseMs ?? 300;

    return async (error: AxiosError) => {
        const cfg = error.config as InternalAxiosRequestConfig | undefined;
        if (!cfg || !isIdempotent(cfg.method)) throw error;

        const status = error.response?.status;
        const retryable = !status || status >= 500;
        if (!retryable) throw error;

        const attempt = (cfg.__retryCount ?? 0) + 1;
        if (attempt > maxRetries) throw error;

        cfg.__retryCount = attempt;
        const delay = backoff(base, attempt);

        if (opts.enableLogging && import.meta.env.DEV) {
            // eslint-disable-next-line no-console
            console.debug(`[HTTP] retry #${attempt} in ${Math.round(delay)}ms`, cfg.method, cfg.url);
        }

        await new Promise((r) => setTimeout(r, delay));
        return instance.request(cfg);
    };
}

/**
 * Creates a configured Axios client (headers, 401->refresh, idempotent retry, dynamic baseURL).
 * Keep it singleton and import where needed.
 */
export function createHttpClient(opts?: Partial<HttpOptions>): AxiosInstance {
    // Default options from stores if not provided
    const merged: HttpOptions = {
        baseURL: () => useConfigStore.getState().apiBaseUrl ?? 'http://localhost:5248',
        getToken: () => useSessionStore.getState().accessToken,
        getTenantKey: () => useAppStore.getState().tenantKey,
        onRefreshToken: async () => {
            // Default stub (override in opts for real refresh)
            const { refreshToken } = useAuthStore.getState();
            if (!refreshToken) throw new Error('No refresh token');
            // Call your real refresh endpoint here:
            // const { data } = await axios.post<RefreshResult>(`${...}/auth/refresh`, { refreshToken });
            // Update store with new tokens:
            // useAuthStore.getState().setAuth({ token: data.accessToken, refreshToken: data.refreshToken });
            const data = { accessToken: 'NEW_ACCESS', refreshToken: 'NEW_REFRESH' };
            useAuthStore.getState().setAuth({ token: data.accessToken, refreshToken: data.refreshToken });
            return data;
        },
        withCredentials: false,
        appName: 'Qorpe.App',
        apiVersion: 'v1',
        maxRetries: 2,
        backoffBaseMs: 300,
        enableLogging: import.meta.env.DEV,
        ...opts
    };

    const client = axios.create({
        baseURL: typeof merged.baseURL === 'function' ? merged.baseURL() : merged.baseURL,
        withCredentials: merged.withCredentials
    });

    // Request
    client.interceptors.request.use(makeRequestInterceptor(merged));

    // Response: 401 refresh
    client.interceptors.response.use(
        (r) => r,
        makeAuthResponseInterceptor(client, merged)
    );

    // Response: retry idempotent
    client.interceptors.response.use(
        (r) => r,
        makeRetryResponseInterceptor(client, merged)
    );

    // Dev logging
    if (merged.enableLogging) {
        client.interceptors.request.use((c) => {
            // eslint-disable-next-line no-console
            console.debug('[HTTP] ->', c.method?.toUpperCase(), c.baseURL, c.url);
            return c;
        });
        client.interceptors.response.use(
            (r) => {
                // eslint-disable-next-line no-console
                console.debug('[HTTP] <-', r.status, r.config.url);
                return r;
            },
            (e) => {
                // eslint-disable-next-line no-console
                console.debug('[HTTP] x ', e.response?.status, e.config?.url);
                return Promise.reject(e);
            }
        );
    }

    return client;
}

/** Shared singleton instance for app-wide use. */
export const http = createHttpClient();
