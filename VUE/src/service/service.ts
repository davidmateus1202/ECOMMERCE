const ensureTrailingSlash = (value: string) => value.endsWith('/') ? value : `${value}/`;

const configuredApiUrl = import.meta.env.VITE_API_BASE_URL?.trim();

export const BASE_URL = configuredApiUrl
    ? ensureTrailingSlash(configuredApiUrl)
    : import.meta.env.DEV
        ? 'http://localhost:5174/api/'
        : 'https://api.iapi.com.co/api/';

// global constants for the service can be added here
export const formatCurrency = (
    value: number,
    currency: string = 'COP',
    locale: string = 'es-CO'
) => {
    return new Intl.NumberFormat(locale, {
        style: 'currency',
        currency,
    }).format(value);
}