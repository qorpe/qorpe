<template>
    <v-data-table-server v-model="selected" v-model:items-per-page="itemsPerPage" :headers="headers"
        :items="serverItems" :items-length="totalItems" :loading="loading" :search="search" item-value="Order"
        @update:options="loadItems" show-select return-object style="background-color: transparent;" class="mt-5">
        <template v-slot:top>
            <v-toolbar flat color="white" class="mb-3 pa-0">
                <v-toolbar-title></v-toolbar-title>
                <v-spacer></v-spacer>
                <v-btn to="/routes/new" variant="tonal" class="ma-0 " dark append-icon="mdi-plus">
                    New Route
                </v-btn>
            </v-toolbar>
        </template>
        <template v-slot:[`item.match.path`]="{ value }">
            <RouterLink :to="`/routes/${value}`" class="link">
                <v-chip color="primary">
                    {{ value }}
                </v-chip>
            </RouterLink>
        </template>
        <template v-slot:[`item.routeId`]="{ value }">
            <RouterLink :to="`/routes/${value}`" class="link">
                {{ value }}
            </RouterLink>
        </template>
        <template v-slot:[`item.actions`]>
            <v-tooltip text="Edit" location="left">
                <template v-slot:activator="{ props }">
                    <v-btn v-bind="props" variant="tonal" size="small" class="mr-1">
                        <v-icon size="large">mdi-square-edit-outline</v-icon>
                    </v-btn>
                </template>
            </v-tooltip>
            <v-tooltip text="Delete" location="right">
                <template v-slot:activator="{ props }">
                    <v-btn v-bind="props" variant="tonal" size="small" class="mr-1">
                        <v-icon size="large">mdi-delete-outline</v-icon>
                    </v-btn>
                </template>
            </v-tooltip>
        </template>
    </v-data-table-server>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'

const desserts = ref([
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
])



const selected = ref([])
const itemsPerPage = ref(5)
const headers = ref([
    { title: 'Route Id', key: 'routeId' },
    { title: 'Order', key: 'order' },
    { title: 'Path', key: 'match.path' },
    { title: 'Cluster Id', key: 'clusterId' },
    { title: 'Actions', key: 'actions' },
    // { title: 'Output-Cache Policy', key: 'OutputCachePolicy' },
    // { title: 'Cors Policy', key: 'CorsPolicy' },
    // { title: 'Timeout', key: 'Timeout' },
    // { title: 'Max-Request Body Size', key: 'MaxRequestBodySize' },
])
const search = ref('')
const serverItems = ref([])
const loading = ref<boolean>(true)
const totalItems = ref<number>(0)

axios.get('https://localhost:7222/api/routes')
    .then(response => {
        console.log(response.data);
        desserts.value = response.data.data
    })
    .catch(error => {
        console.error('Error making the request:', error);
    });

async function fetch({ page, itemsPerPage, sortBy }: any) {
    return new Promise(resolve => {
        setTimeout(() => {
            const start = (page - 1) * itemsPerPage
            const end = start + itemsPerPage
            const items = desserts.value.slice()

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
}

function loadItems({ page, itemsPerPage, sortBy }: any) {
    loading.value = true
    fetch({ page, itemsPerPage, sortBy }).then(({ items, total }: any) => {
        serverItems.value = items
        totalItems.value = total
        loading.value = false
    })
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