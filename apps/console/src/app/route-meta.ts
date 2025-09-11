import { matchRoutes } from "react-router";

export interface RouteMeta {
    /** Shown in tabs; can be overridden by a resolver. */
    title?: string;
    /** If false, the route change won't open a tab. */
    tabbed: boolean;
    /** Resolve dynamic titles like "Routes / {name}". */
    titleResolver?: (params: Record<string, string>) => string;
}

const metaTable: Array<{ path: string; meta: RouteMeta }> = [
    // GATE - ROUTES
    { path: "/gate/routes",         meta: { tabbed: false, title: "Routes" } },
    { path: "/gate/routes/new",     meta: { tabbed: true,  title: "New Route" } },
    { path: "/gate/routes/:id",     meta: { tabbed: true,  title: "Route Detail" } },

    // GATE - CLUSTERS (sample)
    { path: "/gate/clusters",       meta: { tabbed: false, title: "Clusters" } },
    { path: "/gate/clusters/new",   meta: { tabbed: true,  title: "New Cluster" } },
    { path: "/gate/clusters/:id",   meta: { tabbed: true,  title: "Cluster Detail" } },

    // SCHEDULER - JOBS/TRIGGERS
    { path: "/scheduler/jobs",        meta: { tabbed: false, title: "Jobs" } },
    { path: "/scheduler/jobs/new",    meta: { tabbed: true,  title: "New Job" } },
    { path: "/scheduler/jobs/:id",    meta: { tabbed: true,  title: "Job Detail" } },

    { path: "/scheduler/triggers",    meta: { tabbed: false, title: "Triggers" } },
    { path: "/scheduler/triggers/new",meta: { tabbed: true,  title: "New Trigger" } },
    { path: "/scheduler/triggers/:id",meta: { tabbed: true,  title: "Trigger Detail" } },

    // HUB - TENANTS
    { path: "/hub/tenants",         meta: { tabbed: false, title: "Tenants" } },
    { path: "/hub/tenants/new",     meta: { tabbed: true,  title: "New Tenant" } },
    { path: "/hub/tenants/:id",     meta: { tabbed: true,  title: "Tenant Detail" } },
];

/** Returns meta for the current path. */
export function getRouteMeta(pathname: string): RouteMeta {
    const m = matchRoutes(
        metaTable.map((x) => ({ path: x.path })),
        pathname
    );
    if (!m?.length) return { tabbed: true, title: pathname };
    const idx = metaTable.findIndex((x) => x.path === m[0].route.path);
    const found = metaTable[idx];
    if (!found) return { tabbed: true, title: pathname };

    const params = (m[0].params ?? {}) as Record<string, string>;
    const title =
        found.meta.titleResolver ? found.meta.titleResolver(params) : found.meta.title ?? pathname;

    return { ...found.meta, title };
}
