// Strict HTTP/auth types for the client

export type GetToken = () => string | undefined;
export type GetTenantKey = () => string | undefined;

/** Result of token refresh call. */
export interface RefreshResult {
    accessToken: string;
    refreshToken?: string;
}

/** Refresh callback that returns new tokens. */
export type OnRefreshToken = () => Promise<RefreshResult>;

/** Http client factory options. */
export interface HttpOptions {
    /** Base URL or function that resolves it per request. */
    baseURL: string | (() => string);
    /** Bearer access token getter. */
    getToken?: GetToken;
    /** Tenant key getter. */
    getTenantKey?: GetTenantKey;
    /** 401 -> refresh handler (optional). */
    onRefreshToken?: OnRefreshToken;
    /** Axios withCredentials flag. */
    withCredentials?: boolean;
    /** App metadata headers. */
    appName?: string;
    apiVersion?: string;
    /** Retry settings for idempotent methods. */
    maxRetries?: number;     // default 2
    backoffBaseMs?: number;  // default 300
    /** Dev logging. */
    enableLogging?: boolean;
}
