import { createBrowserRouter, Navigate } from "react-router";
import AppLayout from "@/app/layouts/app-layout";
import Login from "@/features/hub/auth/pages/login";
import NotFound from "@/app/pages/not-found";
import GateRoutes from "@/features/gate/routes";

import { useSessionStore } from "./stores/session-store";
import { type JSX } from "react";

/** Simple guard wrapper. */
function Protected({ children }: { children: JSX.Element }) {
    const isAuth = useSessionStore((s) => s.isAuthenticated);
    return isAuth ? children : <Navigate to="/login" replace />;
}

export const router = createBrowserRouter([
    { path: "/login", element: <Login /> },
    {
        path: "/",
        element: (
            <Protected>
                <AppLayout />
            </Protected>
        ),
        children: [
            { index: true, element: <Navigate to="/gate/routes" replace /> },
            { path: "/gate/routes", element: <GateRoutes /> },
            { path: "/gate/clusters", element: <GateClusters /> },
            { path: "/gate/settings", element: <GateSettings /> },
            { path: "/scheduler/jobs", element: <SchedulerJobs /> },
            { path: "/scheduler/triggers", element: <SchedulerTriggers /> },
            { path: "/hub/tenants", element: <HubTenants /> },
            { path: "*", element: <NotFound /> },
        ],
    },
]);
