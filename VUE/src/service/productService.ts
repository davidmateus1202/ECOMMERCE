import type { Product } from '../api/productApi';
import { BASE_URL } from './service';

export type PriceRange = 'all' | 'under-50000' | '50000-100000' | '100000-250000' | 'over-250000';
export type StockFilter = 'all' | 'available' | 'low-stock' | 'out-of-stock' | 'inactive';

export interface ProductFilterState {
    category: string;
    priceRange: PriceRange;
    status: StockFilter;
}

export interface ProductFilterOption<T extends string = string> {
    value: T;
    label: string;
}

export interface InventoryStatus {
    key: Exclude<StockFilter, 'all'>;
    label: string;
    badgeClass: string;
}

const API_ORIGIN = new URL(BASE_URL).origin;

export const FALLBACK_PRODUCT_IMAGE = 'data:image/svg+xml;charset=UTF-8,%3Csvg xmlns%3D%22http%3A//www.w3.org/2000/svg%22 width%3D%22640%22 height%3D%22800%22 viewBox%3D%220 0 640 800%22%3E%3Crect width%3D%22640%22 height%3D%22800%22 fill%3D%22%23f4f5f7%22/%3E%3Crect x%3D%2268%22 y%3D%22180%22 width%3D%22504%22 height%3D%22340%22 rx%3D%2232%22 fill%3D%22white%22 stroke%3D%22%23d9dde3%22 stroke-width%3D%2216%22/%3E%3Ccircle cx%3D%22174%22 cy%3D%22274%22 r%3D%2238%22 fill%3D%22%23d9dde3%22/%3E%3Cpath d%3D%22M130 448l112-158 101 147 51-70 116 151H130z%22 fill%3D%22%23d9dde3%22/%3E%3Ctext x%3D%22320%22 y%3D%22608%22 text-anchor%3D%22middle%22 font-family%3D%22Arial%2C sans-serif%22 font-size%3D%2248%22 fill%3D%22%23919aa6%22%3ESin imagen%3C/text%3E%3C/svg%3E';

export const DEFAULT_PRODUCT_FILTERS: Readonly<ProductFilterState> = {
    category: 'all',
    priceRange: 'all',
    status: 'all',
};

export const PRODUCT_PRICE_OPTIONS: readonly ProductFilterOption<PriceRange>[] = [
    { value: 'all', label: 'Rango de precio: Todos' },
    { value: 'under-50000', label: 'Rango de precio: Hasta $50.000' },
    { value: '50000-100000', label: 'Rango de precio: $50.000 a $100.000' },
    { value: '100000-250000', label: 'Rango de precio: $100.000 a $250.000' },
    { value: 'over-250000', label: 'Rango de precio: Más de $250.000' },
];

export const PRODUCT_STATUS_OPTIONS: readonly ProductFilterOption<StockFilter>[] = [
    { value: 'all', label: 'Estado: Todos' },
    { value: 'available', label: 'Estado: Disponible' },
    { value: 'low-stock', label: 'Estado: Stock bajo' },
    { value: 'out-of-stock', label: 'Estado: Agotado' },
    { value: 'inactive', label: 'Estado: Inactivo' },
];

export const getProductCategoryOptions = (products: Product[]): ProductFilterOption[] => {
    const categories = Array.from(
        new Set(products.map((product) => product.category).filter((category) => category.trim().length > 0)),
    ).sort((left, right) => left.localeCompare(right, 'es'));

    return [
        { value: 'all', label: 'Categorías: Todas' },
        ...categories.map((category) => ({
            value: category,
            label: `Categorías: ${category}`,
        })),
    ];
};

export const getActiveProductFilterCount = (filters: ProductFilterState) => {
    return [filters.category, filters.priceRange, filters.status].filter((value) => value !== 'all').length;
};

export const getInventoryStatus = (product: Product): InventoryStatus => {
    if (!product.isActive) {
        return {
            key: 'inactive',
            label: 'Inactivo',
            badgeClass: 'bg-slate-200 text-slate-700',
        };
    }

    if (product.stock <= 0) {
        return {
            key: 'out-of-stock',
            label: 'Agotado',
            badgeClass: 'bg-rose-100 text-rose-700',
        };
    }

    if (product.stock <= 5) {
        return {
            key: 'low-stock',
            label: 'Stock bajo',
            badgeClass: 'bg-amber-100 text-amber-700',
        };
    }

    return {
        key: 'available',
        label: 'Disponible',
        badgeClass: 'bg-emerald-100 text-emerald-700',
    };
};

export const matchesPriceRange = (price: number, range: PriceRange) => {
    if (range === 'all') return true;
    if (range === 'under-50000') return price < 50000;
    if (range === '50000-100000') return price >= 50000 && price <= 100000;
    if (range === '100000-250000') return price > 100000 && price <= 250000;
    return price > 250000;
};

export const matchesProductStatus = (product: Product, filter: StockFilter) => {
    if (filter === 'all') return true;
    return getInventoryStatus(product).key === filter;
};

export const filterProducts = (products: Product[], filters: ProductFilterState) => {
    return products.filter((product) => {
        const sameCategory = filters.category === 'all' || product.category === filters.category;
        const samePriceRange = matchesPriceRange(product.price, filters.priceRange);
        const sameStatus = matchesProductStatus(product, filters.status);

        return sameCategory && samePriceRange && sameStatus;
    });
};

export const resolveProductImageUrl = (imageUrl?: string | null, fallbackImage: string = FALLBACK_PRODUCT_IMAGE) => {
    if (!imageUrl || !imageUrl.trim()) {
        return fallbackImage;
    }

    const normalizedUrl = imageUrl.trim();

    if (/^https?:\/\//i.test(normalizedUrl) || normalizedUrl.startsWith('data:')) {
        return normalizedUrl;
    }

    if (normalizedUrl.startsWith('/')) {
        return `${API_ORIGIN}${normalizedUrl}`;
    }

    return `${API_ORIGIN}/${normalizedUrl.replace(/^\.\//, '')}`;
};

export const getProductImage = (product: Product, fallbackImage: string = FALLBACK_PRODUCT_IMAGE) => {
    const imageUrl = product.productImages.find((image) => image.isMain)?.imageUrl
        ?? product.productImages[0]?.imageUrl;

    return resolveProductImageUrl(imageUrl, fallbackImage);
};

export const formatProductDate = (value: string, locale: string = 'es-CO') => {
    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
        return 'Sin fecha';
    }

    return new Intl.DateTimeFormat(locale, {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
    }).format(date);
};