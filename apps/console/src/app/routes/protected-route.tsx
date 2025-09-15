import { useSessionStore } from "@/app/stores/session-store";
import { type JSX } from "react";
import { Navigate } from "react-router";

/** Simple guard wrapper. */
export default function ProtectedRoute({ children }: { children: JSX.Element }) {
    const isAuth = useSessionStore((s) => s.isAuthenticated);
    return isAuth ? children : <Navigate to="/console/login" replace />;
}