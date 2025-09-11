import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { AuthTokens, UserIdentity } from "@/shared/types/auth";

interface SessionState {
    tokens: AuthTokens | null;
    user: UserIdentity | null;
    tenants: string[];
    selectedTenant: string | null;
    setTokens: (t: AuthTokens | null) => void;
    setUser: (u: UserIdentity | null) => void;
    setTenants: (list: string[]) => void;
    selectTenant: (tenant: string) => void;
    logout: () => void;
}

export const useSessionStore = create<SessionState>()(
    persist(
        (set) => ({
            tokens: null,
            user: null,
            tenants: [],
            selectedTenant: null,
            setTokens: (t) => set({ tokens: t }),
            setUser: (u) => set({ user: u }),
            setTenants: (list) => set({ tenants: list, selectedTenant: list[0] ?? null }),
            selectTenant: (tenant) => set({ selectedTenant: tenant }),
            logout: () => set({ tokens: null, user: null, tenants: [], selectedTenant: null }),
        }),
        { name: "qorpe-session" }
    )
);
