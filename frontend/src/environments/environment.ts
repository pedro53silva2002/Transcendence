const viteEnv = (import.meta as any).env ?? {};
const apiUrl =
  (viteEnv.VITE_API_URL as string | undefined) ??
  (globalThis as any).__ENV?.VITE_API_URL ??
  'http://localhost:5024/api';

export const environment = {
  production: false,
  apiUrl: 'http://localhost:5024/api',
};
