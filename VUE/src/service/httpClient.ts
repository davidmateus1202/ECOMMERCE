import axios, { AxiosHeaders } from 'axios';
import { clearStoredAuthToken, getStoredAuthToken } from './authService';
import { BASE_URL } from './service';

export const httpClient = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000,
});

httpClient.interceptors.request.use(
  (config) => {
    const token = getStoredAuthToken();

    if (token) {
      if (config.headers && typeof config.headers.set === 'function') {
        config.headers.set('Authorization', `Bearer ${token}`);
      } else {
        config.headers = AxiosHeaders.from({
          ...config.headers,
          Authorization: `Bearer ${token}`,
        });
      }
    }

    return config;
  },
  (error) => Promise.reject(error)
);

httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error?.response?.status === 401) {
      clearStoredAuthToken();

      if (typeof window !== 'undefined' && window.location.pathname.startsWith('/dashboard')) {
        window.location.assign('/auth/login');
      }
    }

    const message =
      error?.response?.data?.message ||
      error?.message ||
      'Request failed';
    return Promise.reject(new Error(message));
  }
);
