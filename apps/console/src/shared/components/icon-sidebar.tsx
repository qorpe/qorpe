import { Layout, Menu, Tooltip } from "antd";
import { useNavigate } from "react-router";
import { MODULES } from "@/app/route-registry.tsx";
import { useAppStore } from "@/app/stores/app-store";
import { type JSX } from "react";

/** Left-most icon-only sidebar that switches active module. */
export default function IconSidebar(): JSX.Element {
    const current = useAppStore((s) => s.currentModule);
    const setModule = useAppStore((s) => s.setModule);
    const nav = useNavigate();

    return (
        <Layout.Sider width={64} theme="dark" style={{ position: "sticky", top: 0, height: "100vh" }}>
            <Menu
                mode="inline"
                selectedKeys={[current]}
                inlineCollapsed
                items={MODULES.map((m) => ({
                    key: m.key,
                    icon: <Tooltip title={m.label}>{m.icon}</Tooltip>,
                    label: m.label,
                }))}
                onClick={({ key }) => {
                    setModule(key as typeof current);
                    const first = MODULES.find((m) => m.key === key)?.sections[0];
                    if (first) nav(first.path);
                }}
            />
        </Layout.Sider>
    );
}
