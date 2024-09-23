<template>
    <v-data-table-server v-model="selected" v-model:items-per-page="itemsPerPage" :headers="headers"
        :items="serverItems" :items-length="totalItems" :loading="loading" :search="search" item-value="Order"
        @update:options="loadItems" show-select return-object style="background-color: transparent;">
        <template v-slot:[`item.Match.Path`]="{ value }">
            <RouterLink :to="`/routes/${value}`" class="link">
                <v-chip color="primary">
                    {{ value }}
                </v-chip>
            </RouterLink>
        </template>

        <template v-slot:[`item.RouteId`]="{ value }">
            <RouterLink :to="`/routes/${value}`" class="link">
                {{ value }}
            </RouterLink>
        </template>

        <template v-slot:[`item.Actions`]>
            <v-icon class="me-2" size="small" @click="editItem(item)">
                mdi-pencil
            </v-icon>
            <v-icon size="small" @click="deleteItem(item)">
                mdi-trash-can-outline
            </v-icon>
        </template>
    </v-data-table-server>
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
        Match: { Path: '{**catch-all}' }
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
        Match: { Path: '/test/{**catch-all}' }
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
        Match: { Path: '/routes/{**catch-all}' }
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
        Match: { Path: '/routes/1/{**catch-all}' }
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
        Match: { Path: '/clusters/{**catch-all}' }
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
        showCopy: false,
        itemsPerPage: 5,
        headers: [
            { title: 'Route Id', key: 'RouteId' },
            { title: 'Order', key: 'Order' },
            { title: 'Path', key: 'Match.Path' },
            { title: 'Cluster Id', key: 'ClusterId' },
            { title: 'Actions', key: 'Actions' },
            // { title: 'Output-Cache Policy', key: 'OutputCachePolicy' },
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

<style scoped>
.link {
    text-decoration: none;
    color: inherit;
}

.link:hover {
    text-decoration: underline;
}
</style>