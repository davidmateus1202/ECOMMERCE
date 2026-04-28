import { httpClient } from '../service/httpClient';
import { getProductImage } from '../service/productService';
import { resolveProductImageUrl } from '../service/productService';
import type { ShopProduct } from '../types/shopProduct';

// E-COMMERCE/src/api/productApi.ts - API functions related to products
export interface ProductImage {
    productImageId: number;
    imageUrl: string;
    isMain: boolean;
    productId: number;
}

export interface ProductImageInput {
    imageUrl: string;
    isMain: boolean;
}

export interface UploadedProductImage {
    fileName: string;
    imageUrl: string;
}

export interface Product {
  productId: number;
  name: string;
  price: number;
  category: string;
    sku?: string | null;
  stock: number;
  isActive: boolean;
  isNew: boolean;
  description: string;
  createdDate: string;
  updatedDate: string;
  productImages: ProductImage[];
}

export interface PaginatedResponse<T> {
    totalItems: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
    items: T[];
}

export interface ProductPageParams {
    page?: number;
    pageSize?: number;
}

export interface StorefrontCategoryCard {
    name: string;
    tagline: string;
    image: string;
}

export interface StorefrontHighlightCard {
    title: string;
    description: string;
    tag: string;
    image: string;
}

interface StorefrontApiResponse {
    products: Product[];
    categories: StorefrontCategoryCard[];
    leftFeature: StorefrontHighlightCard;
    mosaicCards: StorefrontHighlightCard[];
    latestProducts: Product[];
    bestSellerProducts: Product[];
    newCollectionProducts: Product[];
}

export interface StorefrontResponse {
    products: ShopProduct[];
    categories: StorefrontCategoryCard[];
    leftFeature: StorefrontHighlightCard;
    mosaicCards: StorefrontHighlightCard[];
    latestProducts: ShopProduct[];
    bestSellerProducts: ShopProduct[];
    newCollectionProducts: ShopProduct[];
}

export interface CategorySectionConfig {
    slug: string;
    category: string;
    title: string;
    description: string;
    featuredEyebrow: string;
    allEyebrow: string;
    allTitle: string;
    allDescription: string;
    emptyTitle: string;
    emptyDescription: string;
}

export interface ProductUpsertPayload {
    name: string;
    sku?: string | null;
    description: string;
    price: number;
    stock: number;
    category: string;
    isActive: boolean;
    isNew: boolean;
    productImages: ProductImageInput[];
}

interface CategoryProductsApiResponse {
    category: string;
    title: string;
    description: string;
    sectionConfig: CategorySectionConfig;
    products: Product[];
    featuredProducts: Product[];
}

export interface CategoryProductsResponse {
    category: string;
    title: string;
    description: string;
    sectionConfig: CategorySectionConfig;
    products: ShopProduct[];
    featuredProducts: ShopProduct[];
}

const mapProductToShopProduct = (product: Product): ShopProduct => {
    return {
        id: product.productId,
        nombre: product.name,
        descripcion: product.description,
        precio: product.price,
        imagen: getProductImage(product),
        categoria: product.category,
        sku: product.sku ?? undefined,
    };
};

const mapStorefrontCategoryCard = (category: StorefrontCategoryCard): StorefrontCategoryCard => ({
    ...category,
    image: resolveProductImageUrl(category.image),
});

const mapStorefrontHighlightCard = (card: StorefrontHighlightCard): StorefrontHighlightCard => ({
    ...card,
    image: resolveProductImageUrl(card.image),
});

const mapStorefrontResponse = (payload: StorefrontApiResponse): StorefrontResponse => {
    return {
        products: payload.products.map(mapProductToShopProduct),
        categories: payload.categories.map(mapStorefrontCategoryCard),
        leftFeature: mapStorefrontHighlightCard(payload.leftFeature),
        mosaicCards: payload.mosaicCards.map(mapStorefrontHighlightCard),
        latestProducts: payload.latestProducts.map(mapProductToShopProduct),
        bestSellerProducts: payload.bestSellerProducts.map(mapProductToShopProduct),
        newCollectionProducts: payload.newCollectionProducts.map(mapProductToShopProduct),
    };
};

const mapCategoryProductsResponse = (payload: CategoryProductsApiResponse): CategoryProductsResponse => {
    return {
        category: payload.category,
        title: payload.title,
        description: payload.description,
        sectionConfig: payload.sectionConfig,
        products: payload.products.map(mapProductToShopProduct),
        featuredProducts: payload.featuredProducts.map(mapProductToShopProduct),
    };
};

export const fetchProducts = async () => {
    const response = await httpClient.get<Product[]>(
        `products/newproducts`
    );
    return response.data;
}

export const fetchAdminProducts = async ({ page = 1, pageSize = 10 }: ProductPageParams = {}) => {
    const response = await httpClient.get<PaginatedResponse<Product>>('products/all', {
        params: { page, pageSize },
    });

    return response.data;
}

// Additional product-related API functions can be added here
export const fetchProductById = async (id: number) => {
    const response = await httpClient.get<Product>(`products/${id}`);
    return response.data;
}

export const fetchAdminProductById = async (id: number) => {
    const response = await httpClient.get<Product>(`products/admin/${id}`);
    return response.data;
}

export const fetchStorefrontData = async () => {
    const response = await httpClient.get<StorefrontApiResponse>('products/storefront');
    return mapStorefrontResponse(response.data);
}

export const fetchStorefrontProductById = async (id: number) => {
    const product = await fetchProductById(id);
    return mapProductToShopProduct(product);
}

export const fetchRelatedStorefrontProducts = async (id: number, take = 3) => {
    const response = await httpClient.get<Product[]>(`products/${id}/related`, {
        params: { take },
    });

    return response.data.map(mapProductToShopProduct);
}

export const fetchProductsByCategory = async (category: string, featuredTake = 4) => {
    const response = await httpClient.get<CategoryProductsApiResponse>(`products/category/${encodeURIComponent(category)}`, {
        params: { featuredTake },
    });

    return mapCategoryProductsResponse(response.data);
}

export const createAdminProduct = async (payload: ProductUpsertPayload) => {
    const response = await httpClient.post<Product>('products', payload);
    return response.data;
}

export const updateAdminProduct = async (id: number, payload: ProductUpsertPayload) => {
    const response = await httpClient.put<Product>(`products/${id}`, payload);
    return response.data;
}

export const deleteAdminProduct = async (id: number) => {
    await httpClient.delete(`products/${id}`);
}

export const uploadAdminProductImages = async (files: File[]) => {
    const formData = new FormData();

    files.forEach((file) => {
        formData.append('files', file);
    });

    const response = await httpClient.post<UploadedProductImage[]>('products/images/upload', formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });

    return response.data;
}