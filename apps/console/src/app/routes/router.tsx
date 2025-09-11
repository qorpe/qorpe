import { createBrowserRouter, Navigate } from "react-router";
import AuthLayout from "@/app/layouts/auth-layout";
import AppLayout from "@/app/layouts/app-layout";
import Login from "@/features/hub/auth/pages/login";
import NotFound from "@/app/pages/not-found";
import JobList from "@/features/scheduler/jobs/pages/job-list";
import JobDetail from "@/features/scheduler/jobs/pages/job-detail";
import JobCreate from "@/features/scheduler/jobs/pages/job-create";
import ProtectedRoute from "@/app/routes/protected-route"

export const router = createBrowserRouter([
    // Public (auth layout)
    {
        element: <AuthLayout />,
        children: [{ path: "/login", element: <Login /> }],
    },

    // Protected
    {
        element: <ProtectedRoute />,
        children: [
            {
                element: <AppLayout />,
                children: [
                    { index: true, element: <Navigate to="/scheduler/jobs" replace /> },

                    // Scheduler - Jobs
                    { path: "/scheduler/jobs", element: <JobList /> },
                    { path: "/scheduler/jobs/new", element: <JobCreate /> },
                    { path: "/scheduler/jobs/:id", element: <JobDetail /> },

                    // ... other modules
                    { path: "*", element: <NotFound /> },
                ],
            },
        ],
    },
]);
