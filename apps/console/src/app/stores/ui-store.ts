import { create, type StateCreator } from "zustand";
import { devtools } from "zustand/middleware";

/** Nominal (branded) TabKey to avoid mixing with arbitrary strings. */
export type TabKey = string & { readonly __brand: "TabKey" };

/** Factory to safely create TabKey from a path. */
export const makeTabKey = (path: string): TabKey => {
    if (!path.startsWith("/app")) {
        if (import.meta.env.DEV) {
            throw new Error(`TabKey must start with "/app", got: ${path}`);
        }
    }
    return path as TabKey;
};

export type AppTab = Readonly<{
    key: TabKey;
    title: string;
    path: TabKey;
}>;

type UiState = { tabs: AppTab[]; activeKey: TabKey };
type UiActions = {
    addTab: (t: AppTab) => void;
    closeTab: (k: TabKey) => void;
    setActive: (k: TabKey) => void;
};

export type UiStore = UiState & UiActions;

type SC = StateCreator<UiStore, [["zustand/devtools", never]], [], UiStore>;

const createUiStore: SC = (set, get) => {
    const home = makeTabKey("/app");
    return {
        tabs: [{ key: home, title: "Dashboard", path: home }],
        activeKey: home,
        addTab: (t) => {
            const exists = get().tabs.some((x) => x.key === t.key);
            set({ tabs: exists ? get().tabs : [...get().tabs, t], activeKey: t.key });
        },
        closeTab: (k) => {
            const prev = get();
            const next = prev.tabs.filter((x) => x.key !== k);
            const nextActive =
                k === prev.activeKey && next.length ? next[next.length - 1].key : prev.activeKey;
            set({ tabs: next, activeKey: nextActive });
        },
        setActive: (k) => set({ activeKey: k }),
    };
};

export const useUiStore = create<UiStore>()(
    devtools(createUiStore, { name: "ui-store" })
);
