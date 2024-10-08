export interface IPaginatedResponse<T> {
    data: T[]
    totalCount: number
    page: number
    pageSize: number
    totalPages: number
    hasPrevious: boolean
}

export * from './routes'
export * from './tabs'