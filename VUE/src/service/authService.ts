const AUTH_TOKEN_KEY = 'auth_token';
const INVALID_TOKEN_VALUES = new Set(['', 'undefined', 'null']);

const getStorage = () => {
    if (typeof window === 'undefined') {
        return null;
    }

    return window.localStorage;
};

export const getStoredAuthToken = () => {
    const storage = getStorage();
    const rawToken = storage?.getItem(AUTH_TOKEN_KEY)?.trim() ?? '';

    if (INVALID_TOKEN_VALUES.has(rawToken)) {
        storage?.removeItem(AUTH_TOKEN_KEY);
        return null;
    }

    return rawToken;
};

export const hasStoredAuthToken = () => {
    return Boolean(getStoredAuthToken());
};

export const setStoredAuthToken = (token: string) => {
    const storage = getStorage();
    const normalizedToken = token.trim();

    if (!storage || INVALID_TOKEN_VALUES.has(normalizedToken)) {
        throw new Error('No se recibió un token de acceso válido.');
    }

    storage.setItem(AUTH_TOKEN_KEY, normalizedToken);
};

export const clearStoredAuthToken = () => {
    getStorage()?.removeItem(AUTH_TOKEN_KEY);
};