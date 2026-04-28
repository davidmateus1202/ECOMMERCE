
const isDevelopment = true;

export const BASE_URL = isDevelopment
  ? 'http://localhost:5174/api/'
  : 'https://your-production-api.com/api';

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