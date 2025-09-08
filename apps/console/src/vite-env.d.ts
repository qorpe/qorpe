/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_API_BASE?: string;      // e.g. https://localhost:5243 or /api
    readonly VITE_TIMEOUT_MS?: string;    // e.g. "15000"
}
interface ImportMeta {
    readonly env: ImportMetaEnv;
}
