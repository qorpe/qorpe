<script setup lang="ts">
import { ref } from 'vue'

async function submit(event: Event) {
    // ...
}

async function checkApi(userName: String) {
    return new Promise(resolve => {
        // ...
    })
}
</script>

<template>
    <v-form validate-on="submit lazy" @submit.prevent="submit">

    </v-form>
</template>