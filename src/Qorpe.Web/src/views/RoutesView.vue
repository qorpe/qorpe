<template>
    <v-data-table-server v-model="selected" v-model:items-per-page="itemsPerPage" :headers="headers"
        :items="serverItems" :items-length="totalItems" :loading="loading" :search="search" item-value="Order"
        @update:options="loadItems" show-select return-object
        style="background-color: transparent;"></v-data-table-server>
</template>

<script>
const desserts = [
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 1,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 2,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 3,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 4,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 5,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 6,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 7,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 8,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 9,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 10,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
    {
        RouteId: 'Route Id - xxxxxxx - xxxxxxx',
        Order: 11,
        ClusterId: 'Cluster Id - xxxxxxx - xxxxxxx',
        AuthorizationPolicy: 'Policy - xxxxxxx',
        RateLimiterPolicy: 'Policy - xxxxxxx',
        OutputCachePolicy: 'Policy - xxxxxxx',
        TimeoutPolicy: 'Policy - xxxxxxx',
        CorsPolicy: 'Policy - xxxxxxx',
        Timeout: '00:12:12',
        MaxRequestBodySize: '1000',
    },
]

const FakeAPI = {
    async fetch({ page, itemsPerPage, sortBy }) {
        return new Promise(resolve => {
            setTimeout(() => {
                const start = (page - 1) * itemsPerPage
                const end = start + itemsPerPage
                const items = desserts.slice()

                if (sortBy.length) {
                    const sortKey = sortBy[0].key
                    const sortOrder = sortBy[0].order
                    items.sort((a, b) => {
                        const aValue = a[sortKey]
                        const bValue = b[sortKey]
                        return sortOrder === 'desc' ? bValue - aValue : aValue - bValue
                    })
                }

                const paginated = items.slice(start, end)

                resolve({ items: paginated, total: items.length })
            }, 500)
        })
    },
}

export default {
    data: () => ({
        selected: [],
        itemsPerPage: 5,
        headers: [
            { title: 'Order', key: 'Order' },
            { title: 'Route Id', key: 'RouteId' },
            { title: 'Cluster Id', key: 'ClusterId' },
            { title: 'Authorization Policy', key: 'AuthorizationPolicy' },
            { title: 'Rate-Limiter Policy', key: 'RateLimiterPolicy' },
            { title: 'Output-Cache Policy', key: 'OutputCachePolicy' },
            // { title: 'Timeout Policy', key: 'TimeoutPolicy' },
            // { title: 'Cors Policy', key: 'CorsPolicy' },
            // { title: 'Timeout', key: 'Timeout' },
            // { title: 'Max-Request Body Size', key: 'MaxRequestBodySize' },
        ],
        search: '',
        serverItems: [],
        loading: true,
        totalItems: 0,
    }),
    methods: {
        loadItems({ page, itemsPerPage, sortBy }) {
            this.loading = true
            FakeAPI.fetch({ page, itemsPerPage, sortBy }).then(({ items, total }) => {
                this.serverItems = items
                this.totalItems = total
                this.loading = false
            })
        },
    },
}
</script>