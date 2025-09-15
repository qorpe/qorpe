import { http } from "@/shared/lib/http";
import type { LoginRequest, LoginResponse, ExchangeResponse } from "@/features/hub/auth/types";

/** Concatenate tenant-scoped path. */
const t = (tenant: string, path: string) => `/t/${encodeURIComponent(tenant)}/hub/v1${path}`;

export const authApi = {
    /** POST /auth/login */
    async login(tenant: string, body: LoginRequest): Promise<LoginResponse> {
        const { data } = await http.post<LoginResponse>(t(tenant, "/auth/login"), body);
        return data;
    },

    /** POST /auth/refresh (cookie-based, body empty) */
    async refresh(tenant: string): Promise<{ accessToken: string }> {
        const { data } = await http.post<{ accessToken: string }>(t(tenant, "/auth/refresh"), {});
        return data;
    },

    /** POST /auth/exchange */
    async exchange(tenant: string): Promise<ExchangeResponse> {
        const { data } = await http.post<ExchangeResponse>(t(tenant, "/auth/exchange"), {});
        return data;
    },

    /** POST /auth/logout (body empty; server reads cookie) */
    async logout(tenant: string): Promise<void> {
        await http.post<void>(t(tenant, "/auth/logout"), {});
    },
};
