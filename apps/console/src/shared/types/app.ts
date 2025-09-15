import * as React from "react";

/** App-level types for tabs & modules. */
export type ModuleKey = "gate" | "scheduler" | "hub";

/** Branded path-like key for tabs. */
export type TabKey = `/${string}`;

export interface AppTab {
    key: TabKey;
    title: string;
    path: string;
    closable: boolean;
    module: ModuleKey;
}

export interface ModuleSection {
    key: string;
    label: string;
    path: string;
}

export interface ModuleConfig {
    key: ModuleKey;
    label: string;
    icon: React.ReactNode;
    sections: ModuleSection[];
}

export type TenantInfo = {
    id: string;
    key: string;          // "acme"
    name?: string;
    isActive?: boolean;
};