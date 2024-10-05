<script setup lang="ts">
import { ref, nextTick } from 'vue'
import TheSidebar from '@/components/TheSidebar.vue'

const drawer = ref(true)
const rail = ref(false)

const currentItem = ref('tab-Web')
const items = ref([
    'Web', 'Shopping', 'Videos', 'Images',
])
const more = ref([
    'News', 'Maps', 'Books', 'Flights', 'Apps',
])
function addItem(item: any) {
    const removed = items.value.splice(0, 1)
    items.value.push(
        ...more.value.splice(more.value.indexOf(item), 1),
    )
    more.value.push(...removed)
    nextTick(() => { currentItem.value = 'tab-' + item })
}
</script>

<template>
    <v-navigation-drawer v-model="drawer" :rail="true" permanent>
        <TheSidebar :rail="rail" />
    </v-navigation-drawer>

    <v-navigation-drawer v-model="drawer" :rail="rail" permanent>
        <v-list lines="one" class="mt-5">
            <v-list-item v-for="n in 10" :key="n" :title="'Item ' + n" link
                ></v-list-item>
        </v-list>
    </v-navigation-drawer>

    <v-main>
        <v-app-bar flat class="border-b">
            <v-tabs v-model="currentItem" fixed-tabs>
                <template v-for="item in items" :key="item">
                    <v-tab :text="item" :value="'tab-' + item" class="text-capitalize"></v-tab>
                    <v-divider vertical class="my-4 mx-1"></v-divider>
                </template>
            </v-tabs>

            <v-menu v-if="more.length">
                <template v-slot:activator="{ props }">
                    <v-btn class="align-self-center text-capitalize" variant="text" v-bind="props">
                        <v-icon icon="mdi-plus"></v-icon>
                    </v-btn>
                </template>

                <v-list class="bg-grey-lighten-3">
                    <v-list-item v-for="item in more" :key="item" :title="item" @click="addItem(item)"></v-list-item>
                </v-list>
            </v-menu>
            <template v-slot:append>
                <v-menu v-if="more.length">
                    <template v-slot:activator="{ props }">
                        <v-btn class="align-self-center text-capitalize" variant="text" v-bind="props">
                            <v-icon icon="mdi-menu-down"></v-icon>
                        </v-btn>
                    </template>

                    <v-list class="bg-grey-lighten-3">
                        <v-list-item v-for="item in more" :key="item" :title="item"
                            @click="addItem(item)"></v-list-item>
                    </v-list>
                </v-menu>
            </template>
        </v-app-bar>
        <v-container>
            <RouterView />
        </v-container>
    </v-main>
</template>
