import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { IRoute } from '@/interfaces'
import axios from 'axios'

export const useTabsStore = defineStore('tabs', () => {
    const routes = ref<IRoute[]>([])
    const totalCount = ref<number>(0)
    const page = ref<number>(1)
    const pageSize = ref<number>(10)
    const totalPages = ref<number>(0)
    const hasPrevious = ref<boolean>()
    const allLoaded = ref(false);

    const getRoutesByPagination = async () => {
        try {
            const response = await axios.get('https://localhost:44303/api/routes', {
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

    return { routes }
})
