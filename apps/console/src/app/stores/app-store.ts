import { create, type StateCreator } from "zustand";
import { devtools, persist, createJSONStorage } from "zustand/middleware";

export type AppId = "scheduler" | "gate";

type AppState = {
    selectedApp: AppId;
    tenantKey: string | null; // e.g., "acme" | "globex"
};

type AppActions = {
    setSelectedApp: (app: AppId) => void;
    setTenantKey: (key: string | null) => void;
};

export type AppStore = AppState & AppActions;

type SC = StateCreator<
    AppStore,
    [["zustand/devtools", never], ["zustand/persist", unknown]],
    [],
    AppStore
>;

const createAppStore: SC = (set) => ({
    selectedApp: "scheduler",
    tenantKey: null,
    setSelectedApp: (app) => set({ selectedApp: app }),
    setTenantKey: (key) => set({ tenantKey: key }),
});

export const useAppStore = create<AppStore>()(
    devtools(
        persist(createAppStore, {
            name: "Qorpe.App",
            storage: createJSONStorage(() => localStorage),
            partialize: (s) => ({ selectedApp: s.selectedApp, tenantKey: s.tenantKey }),
        }),
        { name: "app-store" }
    )
);
