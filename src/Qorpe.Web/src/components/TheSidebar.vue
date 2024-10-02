<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps({
    rail: {
        type: Boolean
    }
});

const list = ref([
    { name: 'Routes', to: '/routes', icon: 'mdi-map-marker-path' },
    { name: 'Clusters', to: '/clusters', icon: 'mdi-server' },
    { name: 'Plugins', to: '/plugins', icon: 'mdi-puzzle' },
    { name: 'Api Management', to: '/apim', icon: 'mdi-api' }
])
</script>

<template>
    <v-list color="primary" nav class="mt-4">
        <template v-for="(item, i) in list" :key="i">
            <v-list-item :title="item.name" :to="item.to" link>
                <template v-slot:prepend>
                    <v-icon :icon="item.icon"></v-icon>
                </template>
                <v-tooltip v-if="props.rail" activator="parent" location="end">{{ item.name }}</v-tooltip>
            </v-list-item>
        </template>
    </v-list>
</template>