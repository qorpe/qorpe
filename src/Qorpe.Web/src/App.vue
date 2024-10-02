<script setup lang="ts">
import { ref } from 'vue'
import { useRoute } from "vue-router";
import TheSidebar from '@/components/TheSidebar.vue'

const route = useRoute();
const drawer = ref(true)
const rail = ref(false)
const items = ref([
    {
        title: 'Dashboard',
        disabled: false,
        href: 'breadcrumbs_dashboard',
    },
    {
        title: 'Link 1',
        disabled: false,
        href: 'breadcrumbs_link_1',
    },
    {
        title: 'Link 2',
        disabled: true,
        href: 'breadcrumbs_link_2',
    },
])
</script>

<template>
    <v-app>
        <v-app-bar color="primary" flat>
            <template v-slot:prepend>
                <v-app-bar-nav-icon @click="rail = !rail"
                    :icon="rail ? 'mdi-menu-close' : 'mdi-menu-open'"></v-app-bar-nav-icon>
            </template>
            <v-app-bar-title>
                Qorpe
                <v-chip variant="outlined" class="ml-1">
                    Personel
                </v-chip>
            </v-app-bar-title>
        </v-app-bar>

        <v-navigation-drawer color="grey-lighten-4" style="border: none;" v-model="drawer" :rail="rail" permanent>
            <TheSidebar :key="route.fullPath" :rail="rail" />
        </v-navigation-drawer>

        <v-main>
            <v-container>
                <RouterView />
            </v-container>
        </v-main>
    </v-app>
</template>
