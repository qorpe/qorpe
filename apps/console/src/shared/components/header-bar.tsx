import { Layout, Select, Switch } from "antd";
import { MODULES } from "@/app/route-registry.tsx";
import { useAppStore } from "@/app/stores/app-store.ts";
import { useSessionStore } from "@/app/stores/session-store.ts";
import { useUIStore } from "@/app/stores/ui-store.ts";

export default function HeaderBar() {
    const { currentModule, setModule } = useAppStore();
    const { tenants, selectedTenant, selectTenant } = useSessionStore();
    const { theme, toggle } = useUIStore();

    return (
        <Layout.Header style={{ display: "flex", gap: 16, alignItems: "center", background: "#fff" }}>
            {/* Module selector */}
            <Select
                style={{ width: 160 }}
                value={currentModule}
                options={MODULES.map((m) => ({ label: m.label, value: m.key }))}
                onChange={(val) => setModule(val)}
            />
            {/* Tenant selector */}
            <Select
                style={{ width: 180 }}
                value={selectedTenant ?? undefined}
                options={tenants.map((t) => ({ label: t, value: t }))}
                onChange={(val) => selectTenant(val)}
            />
            {/* Theme switch */}
            <div style={{ marginLeft: "auto" }}>
                <Switch
                    checkedChildren="🌙"
                    unCheckedChildren="☀️"
                    checked={theme === "dark"}
                    onChange={toggle}
                />
            </div>
        </Layout.Header>
    );
}
