import 'axios';

declare module 'axios' {
    interface InternalAxiosRequestConfig {
        /** Guard flag to avoid infinite 401->refresh loops. */
        __retryAfterRefresh?: boolean;
        /** Internal retry counter for idempotent retry logic. */
        __retryCount?: number;
    }
}
