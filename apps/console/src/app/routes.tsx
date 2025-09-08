import { createBrowserRouter, RouteObject } from "react-router-dom";
import { protectedLoader } from "./auth";
import AppLayout from "./app-layout";
import LoginPage from "../modules/auth/login-page";
import NotFoundPage from "./ui/not-found";

// Scheduler pages
import SchedulerHome from "../features/scheduler/pages/home";
import SchedulerJobs from "../features/scheduler/jobs/pages/list-page";
import SchedulerJobDetail from "../features/scheduler/jobs/pages/detail-page";

// Gate pages (placeholder)
import GateHome from "../features/gate/pages/home";
import GateRoutes from "../features/gate/routes/pages/list-page";
import GateRouteDetail from "../features/gate/routes/pages/detail-page";

const appChildren: RouteObject[] = [
    { index: true, element: <div>Dashboard</div> },

    {
        path: "scheduler",
        children: [
            { index: true, element: <SchedulerHome /> },
            { path: "jobs", element: <SchedulerJobs /> },
            { path: "jobs/:id", element: <SchedulerJobDetail /> },
            // triggers vb. sonra eklenecek
        ],
    },

    {
        path: "gate",
        children: [
            { index: true, element: <GateHome /> },
            { path: "routes", element: <GateRoutes /> },
            { path: "routes/:id", element: <GateRouteDetail /> },
            // clusters vb. sonra eklenecek
        ],
    },

    { path: "*", element: <NotFoundPage /> },
];

export const router = createBrowserRouter([
    { path: "/login", element: <LoginPage /> },
    { path: "/app", element: <AppLayout />, loader: protectedLoader, children: appChildren },
    { path: "*", element: <NotFoundPage /> },
]);
