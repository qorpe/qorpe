import { createBrowserRouter, Navigate } from "react-router";
import AuthLayout from "@/app/layouts/auth-layout";
import AppLayout from "@/app/layouts/app-layout";
import Login from "@/features/hub/auth/pages/login";
import NotFound from "@/app/pages/not-found";
import JobList from "@/features/scheduler/jobs/pages/job-list";
import JobDetail from "@/features/scheduler/jobs/pages/job-detail";
import JobCreate from "@/features/scheduler/jobs/pages/job-create";
import ProtectedRoute from "@/app/routes/protected-route"

// src/app/router.tsx
export const router = createBrowserRouter([
    {
        element: <AuthLayout />,
        children: [{ path: "/console/login", element: <Login /> }],
    },
    {
        element: (
            <ProtectedRoute>
                <AppLayout />
            </ProtectedRoute>
        ),
        children: [
            { index: true, element: <Navigate to="/console/scheduler/jobs" replace /> }, // 🔧 düzeltildi
            { path: "/console/scheduler/jobs", element: <JobList /> },
            { path: "/console/scheduler/jobs/new", element: <JobCreate /> },
            { path: "/console/scheduler/jobs/:id", element: <JobDetail /> },
            { path: "*", element: <NotFound /> },
        ],
    },
]);

