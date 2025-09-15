/** Authentication & identity types. */
export interface AuthTokens {
    accessToken: string;
}

/** Minimal JWT payload mapped from backend claims. */
export type JwtPayload = {
    sub: string;
    name?: string;
    email?: string;
    roles?: string[];
    tid?: string;     // tenant key/id claim if you add it
    exp?: number;
    iat?: number;
    nbf?: number;
    [k: string]: unknown;
};