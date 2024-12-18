import { createBrowserRouter } from "react-router-dom";
import DefaultLayout from "./layouts/DefaultLayout";
import RealmLayout from "./layouts/RealmLayout";
import RealmListPage from "./pages/RealmListPage";
import RealmOverviewPage from "./pages/RealmOverviewPage";
import RouteNewPage from "./pages/RouteNewPage";
import RouteEditPage from "./pages/RouteEditPage";
import ClusterNewPage from "./pages/ClusterNewPage";
import ClusterEditPage from "./pages/ClusterEditPage";

export const router = createBrowserRouter([
    {
        path: "/",
        Component: DefaultLayout,
        children: [
            {
                path: "realms",
                Component: RealmListPage
            }
        ],
    },
    {
        path: "/realms/:realmId",
        Component: RealmLayout,
        children: [
            {
                path: "overview",
                Component: RealmOverviewPage
            },
            {
                path: "routes/new",
                Component: RouteNewPage
            },
            {
                path: "routes/:routeId",
                Component: RouteEditPage
            },
            {
                path: "clusters/new",
                Component: ClusterNewPage
            },
            {
                path: "clusters/:clusterId",
                Component: ClusterEditPage
            }
        ],
    },
]);
