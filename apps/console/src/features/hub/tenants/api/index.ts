import { http } from "@/shared/lib/http";
import type { TenantInfo } from "@/shared/types/app";

export const tenantsApi = {
    async mine(): Promise<TenantInfo[]> {
        const { data } = await http.get<TenantInfo[]>("/hub/v1/tenants/mine");
        return data ?? [];
    },
};
