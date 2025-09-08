import { Layout, Button, Dropdown, Tabs, theme as antdTheme } from "antd";
import { Outlet, NavLink, useLocation, useNavigate } from "react-router";
import { useEffect, useMemo } from "react";
import { useUiStore } from "./stores/ui-store";
import { tabTitleFor } from "./tab-registry";
import * as React from "react";

/** App shell with header, sidebars and Postman-like tabs. */
const AppLayout: React.FC = () => {
    const { token } = antdTheme.useToken();
    const nav = useNavigate();
    const loc = useLocation();

    const { tabs, activeKey, addTab, closeTab, setActive } = useUiStore();

    // Route → tab (idempotent): any navigation adds/activates the tab once
    useEffect(() => {
        const path = loc.pathname;
        if (!path.startsWith("/app")) return;
        const title = tabTitleFor(path);
        addTab({ key: path, title, path });
    }, [loc.pathname, addTab]);

    // Hangi app seçili? (header ve subsidebar için)
    const selectedApp = useMemo<"scheduler" | "gate" | "none">(() => {
        if (loc.pathname.startsWith("/app/scheduler")) return "scheduler";
        if (loc.pathname.startsWith("/app/gate")) return "gate";
        return "none";
    }, [loc.pathname]);

    return (
        <Layout style={{ minHeight: "100vh" }}>
            {/* Header: App switcher, ileride tenant/env/search eklenecek */}
            <Layout.Header
                style={{
                    background: token.colorBgContainer,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                    paddingInline: 16,
                }}
            >
                <strong>Qorpe Console</strong>

                <Dropdown
                    menu={{
                        items: [
                            { key: "scheduler", label: "Scheduler", onClick: () => nav("/app/scheduler") },
                            { key: "gate", label: "Gate", onClick: () => nav("/app/gate") },
                        ],
                    }}
                >
                    <Button>App: {selectedApp === "none" ? "—" : selectedApp}</Button>
                </Dropdown>
            </Layout.Header>

            <Layout>
                {/* Sidebar (icon+label) */}
                <Layout.Sider width={100} style={{ background: token.colorBgContainer, paddingTop: 8 }}>
                    <nav style={{ display: "grid", gap: 8, padding: 8 }}>
                        <NavLink to="/app/scheduler">🗓️ Scheduler</NavLink>
                        <NavLink to="/app/gate">🛣️ Gate</NavLink>
                    </nav>
                </Layout.Sider>

                {/* Subsidebar (module-specific) */}
                <Layout.Sider width={240} style={{ background: token.colorBgContainer, paddingTop: 8 }}>
                    {selectedApp === "scheduler" && (
                        <nav style={{ display: "grid", gap: 8, padding: 8 }}>
                            <NavLink to="/app/scheduler/jobs">Jobs</NavLink>
                            {/* İleride: Triggers, Calendars vs. */}
                        </nav>
                    )}

                    {selectedApp === "gate" && (
                        <nav style={{ display: "grid", gap: 8, padding: 8 }}>
                            <NavLink to="/app/gate/routes">Routes</NavLink>
                            {/* İleride: Clusters, Settings vs. */}
                        </nav>
                    )}
                </Layout.Sider>

                <Layout.Content style={{ display: "flex", flexDirection: "column" }}>
                    {/* Tabs bar */}
                    <div style={{ paddingInline: 12 }}>
                        <Tabs
                            type="editable-card"
                            hideAdd
                            onChange={(k) => {
                                setActive(k);
                                nav(k);
                            }}
                            activeKey={activeKey}
                            onEdit={(k, action) => {
                                if (action === "remove") closeTab(k as string);
                            }}
                            items={tabs.map((t) => ({
                                key: t.key,
                                label: t.title,
                                closable: t.key.toString() !== "/app",
                            }))}
                        />
                    </div>

                    {/* Routed content */}
                    <div style={{ padding: 16 }}>
                        <Outlet />
                    </div>
                </Layout.Content>
            </Layout>
        </Layout>
    );
};

export default AppLayout;
