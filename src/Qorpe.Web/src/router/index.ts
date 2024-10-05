import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/workspace',
            name: 'workspace',
            component: () => import('@/layouts/WorkspaceLayout.vue'),
            children: [
                {
                    path: 'routes/new',
                    name: 'routeNew',
                    component: () => import('@/views/routes/RouteNew.vue')
                },
                {
                    path: '/routes/:id',
                    name: 'route',
                    component: () => import('@/views/routes/RouteEdit.vue')
                },
            ]
        },
        // {
        //     path: '/routes',
        //     name: 'routes',
        //     component: () => import('@/views/routes/RouteList.vue')
        // },
        // {
        //     path: '/clusters',
        //     name: 'clusters',
        //     component: () => import('@/views/ClustersView.vue')
        // },
        // {
        //     path: '/clusters/:id',
        //     name: 'cluster',
        //     component: () => import('@/views/ClustersView.vue')
        // },
        // {
        //     path: '/plugins',
        //     name: 'plugins',
        //     component: () => import('@/views/PluginsView.vue')
        // },
        // {
        //     path: '/apim',
        //     name: 'apim',
        //     component: () => import('@/views/ApisView.vue')
        // }
    ]
})

export default router
