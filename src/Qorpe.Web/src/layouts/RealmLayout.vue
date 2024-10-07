<script setup lang="ts">
import { ref } from 'vue'
import { useTabsStore } from '@/stores/tabs'
import TheSidebar from '@/components/TheSidebar.vue'
import RouteList from '@/components/RouteList.vue'

const is = ref(RouteList)
const tabsStore = useTabsStore()
const drawer = ref(true)
</script>

<template>
    <v-navigation-drawer v-model="drawer" :rail="true" permanent>
        <TheSidebar />
    </v-navigation-drawer>

    <v-navigation-drawer v-model="drawer" permanent>
        <component :is="is" />
    </v-navigation-drawer>

    <v-main>
        <v-app-bar flat class="border-b">
            <v-tabs height="50">
                <template v-for="tab in tabsStore.tabs" :key="tab.label">
                    <v-tab :text="tab.label" class="text-capitalize" :to="tab.to"></v-tab>
                    <v-divider vertical class="my-4 mx-1"></v-divider>
                </template>
            </v-tabs>

            <template v-slot:append>
                <v-menu>
                    <template v-slot:activator="{ props }">
                        <v-btn class="align-self-center" variant="text" v-bind="props">
                            <v-icon icon="mdi-plus"></v-icon>
                        </v-btn>
                    </template>

                    <v-list class="bg-grey-lighten-3">
                        <v-list-item></v-list-item>
                    </v-list>
                </v-menu>
            </template>
        </v-app-bar>
        <v-container>
            <RouterView />
        </v-container>
    </v-main>
</template>
