import { Outlet } from "react-router";

/** Layout without navigation, used for login/register etc. */
export default function AuthLayout() {
    return (
        <div style={{ display: "grid", placeItems: "center", minHeight: "100vh" }}>
            <Outlet />
        </div>
    );
}
