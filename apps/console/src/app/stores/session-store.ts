import { create } from "zustand";
import type { AuthTokens, JwtPayload } from "@/shared/types/auth";
import type { LoginRequest } from "@/features/hub/auth/types";
import { authApi } from "@/features/hub/auth/api";
import { tenantsApi } from "@/features/hub/tenants/api";

function decodeJwt<T extends object = JwtPayload>(jwt: string): T {
    const [, payload] = jwt.split(".");
    const json = atob(payload.replace(/-/g, "+").replace(/_/g, "/"));
    return JSON.parse(json) as T;
}

export type AppUser = {
    id: string;
    name?: string;
    email?: string;
    roles: string[];
    tenant?: string;
};

type SessionState = {
    tokens: AuthTokens | null;
    user: AppUser | null;
    isAuthenticated: boolean;
    selectedTenant: string | null;

    initFromCookies: () => Promise<void>;
    login: (body: LoginRequest) => Promise<void>;
    logout: () => Promise<void>;
    applyNewAccessToken: (access: string) => void;
    setSelectedTenant: (tenant: string | null, options?: { silentExchange?: boolean }) => Promise<void>;
    clearSession: () => void;
};

function buildUser(access: string | null, fallbackTenant: string | null): AppUser | null {
    if (!access) return null;
    const p = decodeJwt<JwtPayload>(access);
    return {
        id: String(p.sub),
        name: typeof p.name === "string" ? p.name : undefined,
        email: typeof p.email === "string" ? p.email : undefined,
        roles: Array.isArray(p.roles) ? p.roles.map(String) : [],
        tenant: (typeof p.tid === "string" ? p.tid : undefined) ?? fallbackTenant ?? undefined,
    };
}

export const useSessionStore = create<SessionState>((set, get) => ({
    tokens: null,
    user: null,
    isAuthenticated: false,
    selectedTenant: null,

    initFromCookies: async () => {
        const tenant = get().selectedTenant;
        if (!tenant) {
            set({ tokens: null, user: null, isAuthenticated: false });
            return;
        }
        try {
            const { accessToken } = await authApi.refresh(tenant);
            set({ tokens: { accessToken }, user: buildUser(accessToken, tenant), isAuthenticated: true });
        } catch {
            set({ tokens: null, user: null, isAuthenticated: false });
        }
    },

    login: async (body) => {
        const res = await authApi.login("default", body);
        set({ tokens: { accessToken: res.accessToken }, user: buildUser(res.accessToken, null), isAuthenticated: true });

        if (res.tenantKey) {
            await get().setSelectedTenant(res.tenantKey, { silentExchange: true });
            return;
        }

        const mine = await tenantsApi.mine();
        const first = mine[0]?.key ?? null;
        await get().setSelectedTenant(first, { silentExchange: !!first });
    },

    logout: async () => {
        const tenant = get().selectedTenant;
        if (tenant) { try { await authApi.logout(tenant); } catch(exp: unknown) { console.log(exp) } }
        set({ tokens: null, user: null, isAuthenticated: false });
        try { new BroadcastChannel("auth").postMessage({ type: "logout" }); } catch(exp: unknown) { console.log(exp) }
    },

    applyNewAccessToken: (access) => {
        const tenant = get().selectedTenant;
        set({ tokens: { accessToken: access }, user: buildUser(access, tenant), isAuthenticated: true });
    },

    setSelectedTenant: async (tenant, options) => {
        set({ selectedTenant: tenant ?? null });
        const { isAuthenticated } = get();
        const silent = options?.silentExchange ?? true;
        if (tenant && isAuthenticated && silent) {
            const data = await authApi.exchange(tenant);
            set({ tokens: { accessToken: data.accessToken }, user: buildUser(data.accessToken, tenant), isAuthenticated: true });
        }
    },

    clearSession: () => {
        set({ tokens: null, user: null, isAuthenticated: false });
        try { new BroadcastChannel("auth").postMessage({ type: "logout" }); } catch(exp: unknown) { console.log(exp) }
    },
}));
