import { create, type StateCreator } from "zustand";
import { devtools, persist, createJSONStorage } from "zustand/middleware";

export type ThemeMode = "light" | "dark";

type SessionState = {
    accessToken: string | null;
    tenantKey: string | null;
    theme: ThemeMode;
};

type SessionActions = {
    setToken: (t: string | null) => void;
    setTenant: (k: string | null) => void;
    setTheme: (m: ThemeMode) => void;
    clear: () => void;
};

/** Full store type. */
export type SessionStore = SessionState & SessionActions;

/** Typed StateCreator with middleware tuple annotations (per docs). */
type SC = StateCreator<
    SessionStore,
    [["zustand/devtools", never], ["zustand/persist", unknown]],
    [],
    SessionStore
>;

/** Factory (middleware-friendly) */
const createSessionStore: SC = (set) => ({
    accessToken: null,
    tenantKey: null,
    theme: "light",
    setToken: (t) => set({ accessToken: t }),
    setTenant: (k) => set({ tenantKey: k }),
    setTheme: (m) => set({ theme: m }),
    clear: () => set({ accessToken: null, tenantKey: null }),
});

/** Exported hook with devtools + persist (typed). */
export const useSessionStore = create<SessionStore>()(
    devtools(
        persist(createSessionStore, {
            name: "Qorpe.Session",
            storage: createJSONStorage(() => localStorage),
            partialize: (s) => ({
                accessToken: s.accessToken,
                tenantKey: s.tenantKey,
                theme: s.theme,
            }),
        }),
        { name: "session-store" }
    )
);
