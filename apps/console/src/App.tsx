import { RouterProvider } from "react-router";
import { router } from "@/app/routes/router";
import { ConfigProvider, theme } from "antd";
import { useUIStore } from "@/app/stores/ui-store";
import './App.css'
import { useEffect } from "react";
import { useSessionStore } from "@/app/stores/session-store.ts";

function App() {
    const { theme: mode } = useUIStore();
    const algorithm = mode === "dark" ? theme.darkAlgorithm : theme.defaultAlgorithm;
    const initFromCookies = useSessionStore((s) => s.initFromCookies);
    const selectedTenant = useSessionStore((s) => s.selectedTenant);

    useEffect(() => {
        // If tenant already chosen, try restore session from refresh cookie
        if (selectedTenant) void initFromCookies();
    }, [selectedTenant, initFromCookies]);

    return (
        <ConfigProvider theme={{ algorithm }}>
            <RouterProvider router={router} />
        </ConfigProvider>
    );
}

export default App
