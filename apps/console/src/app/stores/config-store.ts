import { create, type StateCreator } from "zustand";
import { devtools, persist, createJSONStorage } from "zustand/middleware";

/** Concise one-liner summary in English. */
type ConfigState = {
    apiBase: string;
    timeoutMs: number;
};

type ConfigActions = {
    setApiBase: (u: string) => void;
    setTimeout: (ms: number) => void;
};

export type ConfigStore = ConfigState & ConfigActions;

type SC = StateCreator<
    ConfigStore,
    [["zustand/devtools", never], ["zustand/persist", unknown]],
    [],
    ConfigStore
>;

const initialBase = import.meta.env.VITE_API_BASE ?? "/api";
const initialTimeout = Number(import.meta.env.VITE_TIMEOUT_MS ?? "15000");

const createConfigStore: SC = (set) => ({
    apiBase: initialBase,
    timeoutMs: initialTimeout,
    setApiBase: (u) => set({ apiBase: u }),
    setTimeout: (ms) => set({ timeoutMs: ms }),
});

export const useConfigStore = create<ConfigStore>()(
    devtools(
        persist(createConfigStore, {
            name: "Qorpe.config",
            storage: createJSONStorage(() => localStorage),
            partialize: (s) => ({ apiBase: s.apiBase, timeoutMs: s.timeoutMs }),
        }),
        { name: "config-store" }
    )
);
