import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { IRoute } from '@/interfaces'
import axios from 'axios'

export const useRoutesStore = defineStore('routes', () => {
    const routes = ref<IRoute[]>([])
    const totalCount = ref<number>(0)
    const page = ref<number>(1)
    const pageSize = ref<number>(10)
    const totalPages = ref<number>(0)
    const hasPrevious = ref<boolean>()
    const hasNext = ref<boolean>()
    const allLoaded = ref(false)

    const getSelected = computed(() => {
        return (routeId: string): IRoute => {
            return routes.value.find(x => x.routeId === routeId) ?? {} as IRoute
        };
    });

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

    const addOrUpdateMetada = (key: string, value: string, index: number) => {
        if (!key || !value || !index || (index > routes.value.length)) return
        routes.value[index].metadata[key] = value
    }

    const removeMetada = (key: string, index: number) => {
        if (!key || !index || (index > routes.value.length)) return
        delete routes.value[index].metadata[key];
    }

    return {
        routes,
        page,
        loadRoutes,
        getSelected,
        allLoaded,
        hasPrevious,
        hasNext,
        totalCount,
        totalPages,
        addOrUpdateMetada,
        removeMetada
    }
})
