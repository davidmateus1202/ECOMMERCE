import { ref } from 'vue';
import type { PaginatedResponse, Product } from '../api/productApi';

const PRODUCT_PAGE_STORAGE_KEY = 'admin_product_page_cache';
const SELECTED_PRODUCT_STORAGE_KEY = 'admin_selected_product';

export interface AdminProductPageState extends PaginatedResponse<Product> {
    isHydrated: boolean;
}

const createEmptyPageState = (): AdminProductPageState => ({
    totalItems: 0,
    page: 1,
    pageSize: 10,
    totalPages: 1,
    hasPreviousPage: false,
    hasNextPage: false,
    items: [],
    isHydrated: false,
});

const adminProductPageState = ref<AdminProductPageState>(createEmptyPageState());
const selectedAdminProduct = ref<Product | null>(null);

let storeHydrated = false;

const getSessionStorage = () => {
    if (typeof window === 'undefined') {
        return null;
    }

    return window.sessionStorage;
};

const cloneProduct = (product: Product): Product => ({
    ...product,
    productImages: product.productImages.map((image) => ({ ...image })),
});

const clonePageState = (state: AdminProductPageState): AdminProductPageState => ({
    ...state,
    items: state.items.map(cloneProduct),
});

const writeToStorage = <T>(key: string, value: T) => {
    const storage = getSessionStorage();

    if (!storage) {
        return;
    }

    storage.setItem(key, JSON.stringify(value));
};

const readFromStorage = <T>(key: string): T | null => {
    const storage = getSessionStorage();

    if (!storage) {
        return null;
    }

    const rawValue = storage.getItem(key);

    if (!rawValue) {
        return null;
    }

    try {
        return JSON.parse(rawValue) as T;
    } catch {
        storage.removeItem(key);
        return null;
    }
};

const persistSelectedProduct = () => {
    writeToStorage(SELECTED_PRODUCT_STORAGE_KEY, selectedAdminProduct.value);
};

const persistPageState = () => {
    writeToStorage(PRODUCT_PAGE_STORAGE_KEY, adminProductPageState.value);
};

const hydrateStore = () => {
    if (storeHydrated) {
        return;
    }

    const storedPageState = readFromStorage<AdminProductPageState>(PRODUCT_PAGE_STORAGE_KEY);
    const storedSelectedProduct = readFromStorage<Product | null>(SELECTED_PRODUCT_STORAGE_KEY);

    if (storedPageState) {
        adminProductPageState.value = {
            ...createEmptyPageState(),
            ...storedPageState,
            items: (storedPageState.items ?? []).map(cloneProduct),
            isHydrated: true,
        };
    }

    if (storedSelectedProduct) {
        selectedAdminProduct.value = cloneProduct(storedSelectedProduct);
    }

    storeHydrated = true;
};

export const hasAdminProductPageState = () => {
    hydrateStore();
    return adminProductPageState.value.isHydrated;
};

export const getAdminProductPageState = () => {
    hydrateStore();
    return clonePageState(adminProductPageState.value);
};

export const setAdminProductPageState = (pageState: PaginatedResponse<Product>) => {
    hydrateStore();

    adminProductPageState.value = {
        ...pageState,
        items: pageState.items.map(cloneProduct),
        isHydrated: true,
    };

    if (selectedAdminProduct.value) {
        const matchingProduct = adminProductPageState.value.items.find(
            (product) => product.productId === selectedAdminProduct.value?.productId,
        );

        if (matchingProduct) {
            selectedAdminProduct.value = cloneProduct(matchingProduct);
            persistSelectedProduct();
        }
    }

    persistPageState();
};

export const setSelectedAdminProduct = (product: Product | null) => {
    hydrateStore();
    selectedAdminProduct.value = product ? cloneProduct(product) : null;
    persistSelectedProduct();
};

export const getSelectedAdminProduct = (productId?: number | null) => {
    hydrateStore();

    if (productId !== null && productId !== undefined) {
        if (selectedAdminProduct.value?.productId === productId) {
            return cloneProduct(selectedAdminProduct.value);
        }

        const productFromCache = adminProductPageState.value.items.find((product) => product.productId === productId);

        if (productFromCache) {
            selectedAdminProduct.value = cloneProduct(productFromCache);
            persistSelectedProduct();
            return cloneProduct(productFromCache);
        }

        return null;
    }

    return selectedAdminProduct.value ? cloneProduct(selectedAdminProduct.value) : null;
};

export const updateCachedAdminProduct = (product: Product) => {
    hydrateStore();

    const nextProduct = cloneProduct(product);

    selectedAdminProduct.value = nextProduct;
    adminProductPageState.value = {
        ...adminProductPageState.value,
        items: adminProductPageState.value.items.map((currentProduct) =>
            currentProduct.productId === nextProduct.productId ? cloneProduct(nextProduct) : currentProduct,
        ),
    };

    persistSelectedProduct();
    persistPageState();

    return cloneProduct(nextProduct);
};

export const upsertCachedAdminProduct = (product: Product) => {
    hydrateStore();

    const nextProduct = cloneProduct(product);
    const existingIndex = adminProductPageState.value.items.findIndex((item) => item.productId === nextProduct.productId);
    const nextItems = [...adminProductPageState.value.items];

    if (existingIndex >= 0) {
        nextItems.splice(existingIndex, 1, nextProduct);
    } else {
        nextItems.unshift(nextProduct);
    }

    adminProductPageState.value = {
        ...adminProductPageState.value,
        totalItems: existingIndex >= 0 ? adminProductPageState.value.totalItems : adminProductPageState.value.totalItems + 1,
        items: nextItems,
        isHydrated: true,
    };

    selectedAdminProduct.value = nextProduct;

    persistSelectedProduct();
    persistPageState();

    return cloneProduct(nextProduct);
};

export const removeCachedAdminProduct = (productId: number) => {
    hydrateStore();

    const existed = adminProductPageState.value.items.some((item) => item.productId === productId);

    adminProductPageState.value = {
        ...adminProductPageState.value,
        totalItems: existed ? Math.max(0, adminProductPageState.value.totalItems - 1) : adminProductPageState.value.totalItems,
        items: adminProductPageState.value.items.filter((item) => item.productId !== productId),
        isHydrated: true,
    };

    if (selectedAdminProduct.value?.productId === productId) {
        selectedAdminProduct.value = null;
    }

    persistSelectedProduct();
    persistPageState();
};