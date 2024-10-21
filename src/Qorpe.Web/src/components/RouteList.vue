<script setup lang="ts">
import { ref } from 'vue'
import { useTabsStore } from '@/stores/tabs'
import { useRoutesStore } from '@/stores/routes'
import { useRoute } from "vue-router";
import type { ITab, IRoute } from '@/interfaces'
import axios from 'axios'

const route = useRoute();
const tabsStore = useTabsStore()
const routesStore = useRoutesStore()

const routes = ref<IRoute[]>([]);
const page = ref(1);
const pageSize = ref(10);
const allLoaded = ref(false);

function to(_route: IRoute): string {
    return `/realms/${route.params.realmId}/routes/${_route.routeId}`
}

const loadRoutes = async () => {
    try {
        const response = await axios.get('https://localhost:7222/api/routes', {
            params: {
                page: page.value,
                pageSize: pageSize.value,
            },
        });

        if (response.data.data.length < pageSize.value) {
            allLoaded.value = true;  // Tüm veriler yüklendi
        }

        routes.value.push(...response.data.data); // Gelen verileri listeye ekle
        page.value++;
    } catch (error) {
        console.error('Error loading products', error);
    }
};

const loadMore = () => {
    if (!allLoaded.value) {
        routesStore.loadRoutes();
    }
};

if (routesStore.page === 1) {
    routesStore.loadRoutes();
}

function addTab(_route: IRoute) {
    const tab = {
        to: `/realms/${route.params.realmId}/routes/${_route.routeId}`,
        label: _route.name,
        type: 'route'
    } as ITab
    tabsStore.addTab(tab)
}
</script>

<template>
    <v-list lines="one" class="mt-4">
        <v-list-item v-for="(route, i) in routesStore.routes" :key="i" :title="route.name" :to="to(route)" link
            @click="addTab(route)"></v-list-item>
    </v-list>
    <v-btn @click="loadMore" block variant="text" class="">Load More</v-btn>
</template>