import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { ITab } from '@/interfaces'

export const useTabsStore = defineStore('tabs', () => {
    const tabs = ref<ITab[]>([])

    function addTab(tab: ITab) {
        const exists = tabs.value.some(existingTab => existingTab.to === tab.to);

        if (!exists) {
            tabs.value.push(tab);
        }
    }

    function removeTab(tab: ITab) {
        const index = tabs.value.indexOf(tab);

        if (index !== -1) {
            tabs.value.splice(index, 1);
        }

        tabs.value.push(tab)
    }

    return { tabs, addTab, removeTab }
})
