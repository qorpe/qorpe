import { Layout } from "antd";
import IconSidebar from "@/shared/components/icon-sidebar";
import SubSidebar from "@/shared/components/sub-sidebar";
import TabsView from "@/shared/components/tabs-view";
import HeaderBar from "@/shared/components/header-bar";
import { Outlet } from "react-router";

/** Main application shell with header, sidebars and tabs. */
export default function AppLayout() {
    return (
        <Layout style={{ minHeight: "100vh" }}>
            <IconSidebar />
            <SubSidebar />
            <Layout>
                <HeaderBar />
                <Layout.Content style={{ padding: 16 }}>
                    <TabsView />
                    <div style={{ marginTop: 12 }}>
                        <Outlet />
                    </div>
                </Layout.Content>
            </Layout>
        </Layout>
    );
}
