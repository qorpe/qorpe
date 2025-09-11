/** Route meta helps to map the path <-> module / section. */
export interface RouteMeta {
    module: "gate" | "scheduler" | "hub";
    title: string;
    tabClosable?: boolean;
}
