<script setup lang="ts">
import { useRoute } from "vue-router";

const route = useRoute();
</script>

<template>
    <v-app>
        <v-app-bar flat class="border-b">
            <v-app-bar-title>
                <v-btn variant="text" class="text-capitalize">Qorpe</v-btn>
            </v-app-bar-title>
        </v-app-bar>

        <RouterView :key="route.fullPath" />
    </v-app>
</template>
