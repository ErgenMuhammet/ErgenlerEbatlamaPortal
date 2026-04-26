import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/portal': {
        target: 'http://localhost:5293',
        changeOrigin: true,
        secure: false,
      },
      '/portalHub': {
        target: 'http://localhost:5293',
        changeOrigin: true,
        secure: false,
        ws: true,
      },
    },
  },
})
