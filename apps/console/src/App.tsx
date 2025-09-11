import { RouterProvider } from "react-router";
import { router } from "@/app/routes/router";
import { ConfigProvider, theme } from "antd";
import { useUIStore } from "@/app/stores/ui-store";
import './App.css'

function App() {
    const { theme: mode } = useUIStore();
    const algorithm = mode === "dark" ? theme.darkAlgorithm : theme.defaultAlgorithm;

    return (
        <ConfigProvider theme={{ algorithm }}>
            <RouterProvider router={router} />
        </ConfigProvider>
    );
}

export default App
