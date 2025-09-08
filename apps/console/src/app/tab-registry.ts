/** Concise one-liner summary in English. */
export const tabTitleFor = (pathname: string): string => {
    // Static matches
    if (pathname === "/app") return "Dashboard";
    if (pathname === "/app/scheduler") return "Scheduler";
    if (pathname === "/app/scheduler/jobs") return "Jobs";
    if (pathname === "/app/gate") return "Gate";
    if (pathname === "/app/gate/routes") return "Routes";

    // Dynamic patterns
    // /app/scheduler/jobs/:id  => "Job #id"
    const job = pathname.match(/^\/app\/scheduler\/jobs\/([^/]+)$/);
    if (job) return `Job #${decodeURIComponent(job[1])}`;

    // /app/gate/routes/:id     => "Route #id"
    const route = pathname.match(/^\/app\/gate\/routes\/([^/]+)$/);
    if (route) return `Route #${decodeURIComponent(route[1])}`;

    // Fallback: last segment capitalized
    const last = pathname.split("/").filter(Boolean).pop() ?? "Tab";
    return last.charAt(0).toUpperCase() + last.slice(1);
};
