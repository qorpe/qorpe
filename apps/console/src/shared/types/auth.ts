/** Authentication & identity types. */
export interface AuthTokens {
    accessToken: string;
    refreshToken: string;
}

export interface UserIdentity {
    id: string;
    email: string;
    displayName: string;
    roles: string[];
    tenantKey?: string;
}
