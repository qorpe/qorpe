import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: '',
            component: () => import('@/layouts/DefaultLayout.vue'),
            children: [
                {
                    path: 'realms',
                    name: 'realms',
                    component: () => import('@/views/RealmList.vue'),
                },
            ]
        },
        {
            path: '/realms/:realmId',
            name: 'realm',
            component: () => import('@/layouts/RealmLayout.vue'),
            children: [
                {
                    path: 'overview',
                    name: 'overview',
                    component: () => import('@/views/RealmEdit.vue')
                },
                {
                    path: 'routes/new',
                    name: 'routeNew',
                    component: () => import('@/views/routes/RouteNew.vue')
                },
                {
                    path: 'routes/:routeId',
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
