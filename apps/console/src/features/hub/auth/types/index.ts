/** Login/Refresh/Exchange contracts */
export type LoginRequest = { usernameOrEmail: string; password: string };
export type LoginResponse = { accessToken: string; refreshToken: string; expiresAtUtc?: string; userId?: string; userName?: string; tenantKey?: string };

export type RefreshResponse = { accessToken: string; refreshToken: string; expiresAtUtc?: string };
export type ExchangeResponse = { accessToken: string; refreshToken: string; expiresAtUtc?: string };