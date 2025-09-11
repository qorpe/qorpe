import { Tabs } from "antd";
import { type JSX, useEffect, useMemo} from "react";
import { useLocation, useNavigate } from "react-router";
import { useAppStore } from "@/app/stores/app-store";
import type { AppTab, TabKey } from "@/shared/types/app";
import { getRouteMeta } from "@/app/route-meta";

/** Derive a module key from the path prefix. */
function pickModule(pathname: string): AppTab["module"] {
    const seg = pathname.split("/").filter(Boolean)[0] ?? "gate";
    return seg === "scheduler" || seg === "hub" ? seg : "gate";
}

export default function TabsView(): JSX.Element {
    const nav = useNavigate();
    const loc = useLocation();
    const { tabs, activeKey, openTab, closeTab, setActive } = useAppStore();

    const moduleKey = useMemo(() => pickModule(loc.pathname), [loc.pathname]);
    const meta = useMemo(() => getRouteMeta(loc.pathname), [loc.pathname]);

    // Only open tab for tabbed routes
    useEffect(() => {
        const path = loc.pathname as TabKey;
        if (!meta.tabbed) {
            // Ensure no "phantom" activeKey on non-tabbed pages
            if (activeKey && activeKey !== path) setActive(activeKey);
            return;
        }
        const title = meta.title ?? path;
        openTab({
            key: path,
            path,
            title,
            closable: true,
            module: moduleKey,
        });
        setActive(path);
    }, [loc.pathname, meta.tabbed, meta.title, moduleKey, openTab, setActive, activeKey]);

    return (
        <Tabs
            type="editable-card"
            hideAdd
            activeKey={activeKey ?? undefined}
            items={tabs.map((t) => ({ key: t.key, label: t.title, closable: t.closable }))}
            onChange={(k) => {
                setActive(k as TabKey);
                const target = tabs.find((t) => t.key === (k as TabKey));
                if (target) nav(target.path);
            }}
            onEdit={(targetKey, action) => {
                if (action === "remove") {
                    const k = targetKey as TabKey;
                    const idx = tabs.findIndex((t) => t.key === k);
                    const next = tabs.filter((t) => t.key !== k);
                    const fallback = next[Math.max(0, idx - 1)]?.path ?? "/gate/routes";
                    closeTab(k);
                    nav(next.length ? fallback : "/gate/routes");
                }
            }}
        />
    );
}
