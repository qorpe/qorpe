import { ApiOutlined, ScheduleOutlined, TeamOutlined } from "@ant-design/icons";
import type { ModuleConfig } from "@/shared/types/app";

/** Declarative module navigation config. */
export const MODULES: ModuleConfig[] = [
    {
        key: "gate",
        label: "Gate",
        icon: <ApiOutlined />,
        sections: [
            { key: "routes",   label: "Routes",   path: "/gate/routes" },
            { key: "clusters", label: "Clusters", path: "/gate/clusters" },
            { key: "settings", label: "Settings", path: "/gate/settings" },
        ],
    },
    {
        key: "scheduler",
        label: "Scheduler",
        icon: <ScheduleOutlined />,
        sections: [
            { key: "jobs",     label: "Jobs",     path: "/scheduler/jobs" },
            { key: "triggers", label: "Triggers", path: "/scheduler/triggers" },
        ],
    },
    {
        key: "hub",
        label: "Hub",
        icon: <TeamOutlined />,
        sections: [
            { key: "tenants",  label: "Tenants",  path: "/hub/tenants" },
        ],
    },
];
