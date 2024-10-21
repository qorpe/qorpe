<template>
    <v-card>
        <v-toolbar flat density="compact" color="white" class="border-b">
            <v-tabs v-model="currentItem" fixed-tabs density="compact" flat>
                <template v-for="item in items" :key="item">
                    <v-tab :text="item" :value="'tab-' + item" class="" tile to="/apis"></v-tab>
                    <v-divider class="my-4" vertical></v-divider>
                </template>

                <v-menu v-if="more.length">
                    <template v-slot:activator="{ props }">
                        <v-btn class="align-self-center " height="100%" rounded="0" variant="plain"
                            v-bind="props">
                            More
                            <v-icon icon="mdi-menu-down" end></v-icon>
                        </v-btn>
                    </template>

                    <v-list class="bg-grey-lighten-3">
                        <v-list-item v-for="item in more" :key="item" :title="item"
                            @click="addItem(item)"></v-list-item>
                    </v-list>
                </v-menu>
                <v-menu v-if="more.length">
                    <template v-slot:activator="{ props }">
                        <v-btn class="align-self-center " height="100%" rounded="0" variant="tonal"
                            v-bind="props">
                            <v-icon icon="mdi-plus"></v-icon>
                        </v-btn>
                    </template>

                    <v-list class="bg-grey-lighten-3">
                        <v-list-item v-for="item in more" :key="item" :title="item"
                            @click="addItem(item)"></v-list-item>
                    </v-list>
                </v-menu>
            </v-tabs>
        </v-toolbar>

        <v-tabs-window v-model="currentItem" flat>
            <v-tabs-window-item v-for="item in items.concat(more)" :key="item" :value="'tab-' + item">
                <v-card flat>
                    <v-card-text>
                        <h2>{{ item }}</h2>
                        {{ text }}
                    </v-card-text>
                </v-card>
            </v-tabs-window-item>
        </v-tabs-window>
    </v-card>
</template>
<script>
export default {
    data: () => ({
        currentItem: 'tab-Web',
        items: [
            'Web', 'Shopping', 'Videos', 'Images',
        ],
        more: [
            'News', 'Maps', 'Books', 'Flights', 'Apps',
        ],
        text: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.',
    }),

    methods: {
        addItem(item) {
            const removed = this.items.splice(0, 1)
            this.items.push(
                ...this.more.splice(this.more.indexOf(item), 1),
            )
            this.more.push(...removed)
            this.$nextTick(() => { this.currentItem = 'tab-' + item })
        },
    },
}
</script>