<template>
    <v-data-table-server v-model="selected" v-model:items-per-page="itemsPerPage" :headers="headers"
        :items="serverItems" :items-length="totalItems" :loading="loading" :search="search" item-value="Order"
        @update:options="loadItems" show-select return-object style="background-color: transparent;">
        <template v-slot:top>
            <v-toolbar flat color="grey-lighten-4" class="mt-5 mb-3">
                <v-toolbar-title>Routes</v-toolbar-title>
                <v-spacer></v-spacer>
                <v-dialog v-model="dialog" max-width="500px">
                    <template v-slot:activator="{ props }">
                        <v-btn variant="tonal" class="mr-3 text-capitalize" dark v-bind="props" append-icon="mdi-plus">
                            New Route
                        </v-btn>
                    </template>
                    <v-card>
                        <v-card-title>
                            <span class="text-h5">{{ formTitle }}</span>
                        </v-card-title>
                        <v-card-text>
                            <v-container>
                                <v-row>
                                    <!-- <v-col cols="12" md="4" sm="6">
                                        <v-text-field v-model="editedItem.name" label="Dessert name"></v-text-field>
                                    </v-col>
                                    <v-col cols="12" md="4" sm="6">
                                        <v-text-field v-model="editedItem.calories" label="Calories"></v-text-field>
                                    </v-col>
                                    <v-col cols="12" md="4" sm="6">
                                        <v-text-field v-model="editedItem.fat" label="Fat (g)"></v-text-field>
                                    </v-col>
                                    <v-col cols="12" md="4" sm="6">
                                        <v-text-field v-model="editedItem.carbs" label="Carbs (g)"></v-text-field>
                                    </v-col>
                                    <v-col cols="12" md="4" sm="6">
                                        <v-text-field v-model="editedItem.protein" label="Protein (g)"></v-text-field>
                                    </v-col> -->
                                </v-row>
                            </v-container>
                        </v-card-text>
                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <!-- <v-btn color="blue-darken-1" variant="text" @click="close">
                                Cancel
                            </v-btn>
                            <v-btn color="blue-darken-1" variant="text" @click="save">
                                Save
                            </v-btn> -->
                        </v-card-actions>
                    </v-card>
                </v-dialog>
                <!-- <v-dialog v-model="dialogDelete" max-width="500px">
                    <v-card>
                        <v-card-title class="text-h5">Are you sure you want to delete this item?</v-card-title>
                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
                            <v-btn color="blue-darken-1" variant="text" @click="deleteItemConfirm">OK</v-btn>
                            <v-spacer></v-spacer>
                        </v-card-actions>
                    </v-card>
                </v-dialog> -->
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
import { ref, reactive, computed, watch, nextTick } from 'vue'
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

const dialog = ref<boolean>(false)
const dialogDelete = ref<boolean>(false)
const editedIndex = ref<number>(-1)
const editedItem = reactive({})
const defaultItem = reactive({})

// Computed example with explicit type
const formTitle = computed<string>(() => editedIndex.value === -1 ? 'New Item' : 'Edit Item')

function editItem(item: any) {
    editedIndex.value = desserts.value.indexOf(item)
    Object.assign(editItem, item)
    dialog.value = true
}

function deleteItem(item: any) {
    editedIndex.value = desserts.value.indexOf(item)
    Object.assign(editedItem, item)
    dialogDelete.value = true
}

function deleteItemConfirm() {
    desserts.value.splice(editedIndex.value, 1)
    closeDelete()
}

function close() {
    dialog.value = false
    nextTick(() => {
        Object.assign(editedItem, defaultItem)
        editedIndex.value = -1
    })
}

function closeDelete() {
    dialogDelete.value = false
    nextTick(() => {
        Object.assign(editedItem, defaultItem)
        editedIndex.value = -1
    })
}

function save() {
    if (editedIndex.value > -1) {
        Object.assign(desserts.value[editedIndex.value], editedItem)
    } else {
        desserts.value.push(editedItem)
    }
    close()
}

watch(dialog, (val) => {
    val || close()
})

watch(dialogDelete, (val) => {
    val || closeDelete()
})

const selected = ref([])
const showCopy = ref<boolean>(false)
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

axios.get('https://localhost:44303/api/routes')
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