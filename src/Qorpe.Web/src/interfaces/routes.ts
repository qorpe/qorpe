export interface IRoute {
    id: string,
    tenantId: string,
    name: string,
    description: string,
    createdAt: Date,
    updatedAt: Date,
    createdBy: string,
    updatedBy: string,
    routeId: string,
    match: IMatch,
    order: number,
    clusterId: string,
    authorizationPolicy: string,
    rateLimiterPolicy: string,
    outputCachePolicy: string,
    timeoutPolicy: string,
    timeout: string,
    corsPolicy: string,
    maxRequestBodySize: number,
    metadata: IMetadata,
    transforms: ITransform[]
}

export interface IMetadata {
    [key: string]: string
}

export interface ITransform {
    [key: string]: string
}

export interface ITransformGroup {
    index: number
    description: string
}

export interface IQueryParameter {
    name: string
    values: string[]
    mode: number
    isCaseSensitive: boolean
}

export interface IHeader {
    name: string
    values: string[]
    mode: number
    isCaseSensitive: boolean
}

export interface IMatch {
    methods: string[]
    hosts: string[]
    path: string
    queryParameters: IQueryParameter[]
    headers: IHeader[]
}
