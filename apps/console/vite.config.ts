import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import path from "node:path";

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 5173
    },
    base: '/console',
    resolve: {
        alias: {
            '@': path.resolve(__dirname, 'src'),
        },
    },
})
