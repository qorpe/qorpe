import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { AppTab, ModuleKey, TabKey } from "@/shared/types/app";

interface AppState {
    currentModule: ModuleKey;
    tabs: AppTab[];
    activeKey: TabKey | null;
    setModule: (m: ModuleKey) => void;
    openTab: (tab: AppTab) => void;
    closeTab: (key: TabKey) => void;
    setActive: (key: TabKey) => void;
}

export const useAppStore = create<AppState>()(
    persist(
        (set, get) => ({
            currentModule: "gate",
            tabs: [],
            activeKey: null,
            setModule: (m) => set({ currentModule: m }),
            openTab: (tab) => {
                const exists = get().tabs.some((t) => t.key === tab.key);
                set((s) => ({
                    tabs: exists ? s.tabs : [...s.tabs, tab],
                    activeKey: tab.key,
                }));
            },
            closeTab: (key) => {
                const { tabs, activeKey } = get();
                const idx = tabs.findIndex((t) => t.key === key);
                const next = tabs.filter((t) => t.key !== key);
                const nextActive = activeKey === key ? next[Math.max(0, idx - 1)]?.key ?? null : activeKey;
                set({ tabs: next, activeKey: nextActive });
            },
            setActive: (key) => set({ activeKey: key }),
        }),
        { name: "qorpe-app" }
    )
);
