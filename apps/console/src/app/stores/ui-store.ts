import { create } from "zustand";
import { persist } from "zustand/middleware";

interface ThemeState {
    theme: "light" | "dark";
    toggle: () => void;
}

export const useUIStore = create<ThemeState>()(
    persist(
        (set, get) => ({
            theme: "light",
            toggle: () => set({ theme: get().theme === "light" ? "dark" : "light" }),
        }),
        { name: "qorpe-theme" }
    )
);
