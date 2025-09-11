import { Layout, Menu } from "antd";
import { type JSX, useMemo} from "react";
import { useLocation, useNavigate } from "react-router";
import { MODULES } from "@/app/route-registry.tsx";
import { useAppStore } from "@/app/stores/app-store";

/** Secondary sidebar that lists sections of the current module. */
export default function SubSidebar(): JSX.Element {
    const module = useAppStore((s) => s.currentModule);
    const sections = useMemo(
        () => MODULES.find((m) => m.key === module)?.sections ?? [],
        [module]
    );
    const loc = useLocation();
    const nav = useNavigate();

    return (
        <Layout.Sider width={220} theme="light" style={{ borderRight: "1px solid #f0f0f0" }}>
            <Menu
                mode="inline"
                selectedKeys={[loc.pathname]}
                items={sections.map((s) => ({ key: s.path, label: s.label }))}
                onClick={({ key }) => nav(String(key))}
            />
        </Layout.Sider>
    );
}
