<template>
    <div class="flex min-h-full w-full flex-col bg-[#f7f7fb] p-5 md:p-8">
        <div class="flex flex-col gap-5 rounded-[28px] bg-white p-6 shadow-sm shadow-slate-200/70">
            <div class="flex flex-col gap-5 lg:flex-row lg:items-start lg:justify-between">
                <div>
                    <h1 class="text-2xl font-bold tracking-tight text-slate-900 md:text-4xl">Catálogo de Productos</h1>
                    <p class="mt-2 text-sm font-medium text-slate-400 md:text-base">
                        Gestiona y organiza tu inventario de productos en tiempo real.
                    </p>
                </div>

                <div class="flex flex-col gap-3 sm:flex-row sm:items-center">
                    <button
                        type="button"
                        class="rounded-2xl bg-slate-900 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-700 cursor-pointer"
                        @click="goToCreateProduct"
                    >
                        Nuevo producto
                    </button>

                    <div class="flex items-center rounded-2xl border border-slate-200 bg-slate-50 p-1 shadow-sm shadow-slate-200/60">
                        <button
                            type="button"
                            class="rounded-xl p-3 transition cursor-pointer"
                            :class="viewMode === 'grid' ? 'bg-white text-indigo-600 shadow-sm shadow-indigo-100' : 'text-slate-400 hover:text-slate-600'"
                            :aria-pressed="viewMode === 'grid'"
                            @click="viewMode = 'grid'"
                        >
                            <span class="sr-only">Vista en cuadrícula</span>
                            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path stroke-linecap="round" stroke-linejoin="round" d="M4 4h6v6H4zM14 4h6v6h-6zM4 14h6v6H4zM14 14h6v6h-6z" />
                            </svg>
                        </button>
                        <button
                            type="button"
                            class="rounded-xl p-3 transition cursor-pointer"
                            :class="viewMode === 'list' ? 'bg-white text-indigo-600 shadow-sm shadow-indigo-100' : 'text-slate-400 hover:text-slate-600'"
                            :aria-pressed="viewMode === 'list'"
                            @click="viewMode = 'list'"
                        >
                            <span class="sr-only">Vista en lista</span>
                            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path stroke-linecap="round" stroke-linejoin="round" d="M8 6h12M8 12h12M8 18h12M4 6h.01M4 12h.01M4 18h.01" />
                            </svg>
                        </button>
                    </div>
                </div>
            </div>

            <div class="flex flex-col gap-4 border-t border-slate-100 pt-5 xl:flex-row xl:items-center xl:justify-between">
                <div class="flex flex-1 flex-col gap-3 lg:flex-row lg:flex-wrap lg:items-center">
                    <label class="relative block min-w-55 flex-1 lg:max-w-60">
                        <span class="sr-only">Filtrar por categoría</span>
                        <select
                            v-model="selectedCategory"
                            class="h-12 w-full appearance-none rounded-2xl border border-slate-200 bg-white px-4 pr-12 text-sm font-semibold text-slate-700 shadow-sm outline-none transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100"
                        >
                            <option
                                v-for="option in categoryOptions"
                                :key="option.value"
                                :value="option.value"
                            >
                                {{ option.label }}
                            </option>
                        </select>
                        <svg xmlns="http://www.w3.org/2000/svg" class="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path stroke-linecap="round" stroke-linejoin="round" d="m6 9 6 6 6-6" />
                        </svg>
                    </label>

                    <label class="relative block min-w-55 flex-1 lg:max-w-60">
                        <span class="sr-only">Filtrar por rango de precio</span>
                        <select
                            v-model="selectedPriceRange"
                            class="h-12 w-full appearance-none rounded-2xl border border-slate-200 bg-white px-4 pr-12 text-sm font-semibold text-slate-700 shadow-sm outline-none transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100"
                        >
                            <option
                                v-for="option in priceOptions"
                                :key="option.value"
                                :value="option.value"
                            >
                                {{ option.label }}
                            </option>
                        </select>
                        <svg xmlns="http://www.w3.org/2000/svg" class="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path stroke-linecap="round" stroke-linejoin="round" d="m6 9 6 6 6-6" />
                        </svg>
                    </label>

                    <label class="relative block min-w-55 flex-1 lg:max-w-60">
                        <span class="sr-only">Filtrar por estado</span>
                        <select
                            v-model="selectedStatus"
                            class="h-12 w-full appearance-none rounded-2xl border border-slate-200 bg-white px-4 pr-12 text-sm font-semibold text-slate-700 shadow-sm outline-none transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100"
                        >
                            <option
                                v-for="option in statusOptions"
                                :key="option.value"
                                :value="option.value"
                            >
                                {{ option.label }}
                            </option>
                        </select>
                        <svg xmlns="http://www.w3.org/2000/svg" class="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path stroke-linecap="round" stroke-linejoin="round" d="m6 9 6 6 6-6" />
                        </svg>
                    </label>

                    <button
                        type="button"
                        class="h-12 shrink-0 text-sm font-semibold text-indigo-600 transition hover:text-indigo-500 disabled:cursor-not-allowed disabled:text-slate-300 cursor-pointer"
                        :disabled="activeFilterCount === 0"
                        @click="resetFilters"
                    >
                        Limpiar filtros
                    </button>
                </div>

                <p class="text-sm text-slate-500">
                    {{ filteredProducts.length }} productos
                    <span v-if="activeFilterCount > 0"> con {{ activeFilterCount }} filtro(s) activos</span>
                </p>
            </div>
        </div>

        <div v-if="loading" class="mt-6 grid gap-5 sm:grid-cols-2 2xl:grid-cols-3">
            <div
                v-for="card in skeletonCards"
                :key="card"
                class="overflow-hidden rounded-[28px] border border-slate-200 bg-white p-5 shadow-sm shadow-slate-200/70"
            >
                <div class="aspect-4/5 animate-pulse rounded-3xl bg-slate-200"></div>
                <div class="mt-5 space-y-3">
                    <div class="h-4 w-1/3 animate-pulse rounded bg-slate-200"></div>
                    <div class="h-6 w-2/3 animate-pulse rounded bg-slate-200"></div>
                    <div class="h-4 w-full animate-pulse rounded bg-slate-100"></div>
                    <div class="h-4 w-4/5 animate-pulse rounded bg-slate-100"></div>
                </div>
            </div>
        </div>

        <div
            v-else-if="errorMessage"
            class="mt-6 rounded-[28px] border border-rose-100 bg-white p-8 text-center shadow-sm shadow-slate-200/70"
        >
            <p class="text-lg font-semibold text-slate-900">No se pudo cargar el catálogo</p>
            <p class="mt-2 text-sm text-slate-500">{{ errorMessage }}</p>
            <button
                type="button"
                class="mt-5 rounded-full bg-slate-900 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-700 cursor-pointer"
                @click="loadProducts()"
            >
                Reintentar
            </button>
        </div>

        <div
            v-else-if="filteredProducts.length === 0"
            class="mt-6 rounded-[28px] border border-slate-200 bg-white p-10 text-center shadow-sm shadow-slate-200/70"
        >
            <p class="text-lg font-semibold text-slate-900">
                {{ products.length === 0 && activeFilterCount === 0 ? 'No hay productos disponibles en el catálogo' : 'No hay coincidencias para los filtros seleccionados' }}
            </p>
            <p class="mt-2 text-sm text-slate-500">
                {{ products.length === 0 && activeFilterCount === 0 ? 'Cuando el backend tenga registros, aparecerán aquí automáticamente.' : 'Ajusta los filtros o vuelve a cargar todos los productos.' }}
            </p>
            <button
                type="button"
                class="mt-5 text-sm font-semibold text-indigo-600 transition hover:text-indigo-500 cursor-pointer"
                @click="resetFilters"
            >
                Restablecer filtros
            </button>
        </div>

        <div v-else class="mt-6">
            <div v-if="viewMode === 'grid'" class="grid gap-5 sm:grid-cols-2 2xl:grid-cols-3">
                <article
                    v-for="product in filteredProducts"
                    :key="product.productId"
                    class="group flex h-full flex-col overflow-hidden rounded-[28px] border border-slate-200 bg-white p-4 shadow-sm shadow-slate-200/70 transition hover:-translate-y-1 hover:shadow-lg"
                >
                    <div class="relative overflow-hidden rounded-3xl bg-slate-100">
                        <img
                            :src="getProductImage(product)"
                            :alt="product.name"
                            class="aspect-4/5 w-full object-cover transition duration-300 group-hover:scale-[1.02]"
                        >

                        <div class="absolute left-4 top-4 flex flex-wrap gap-2">
                            <span
                                v-if="product.isNew"
                                class="rounded-full bg-white/90 px-3 py-1 text-xs font-semibold text-slate-700 backdrop-blur"
                            >
                                Nuevo
                            </span>
                            <span
                                class="rounded-full px-3 py-1 text-xs font-semibold"
                                :class="getInventoryStatus(product).badgeClass"
                            >
                                {{ getInventoryStatus(product).label }}
                            </span>
                        </div>
                    </div>

                    <div class="flex flex-1 flex-col px-2 pb-2 pt-5">
                        <div class="flex items-start justify-between gap-4">
                            <div>
                                <p class="text-xs font-semibold uppercase tracking-[0.22em] text-slate-400">{{ product.category }}</p>
                                <h2 class="mt-2 text-lg font-semibold text-slate-900">{{ product.name }}</h2>
                            </div>
                            <span class="rounded-full bg-slate-100 px-3 py-1 text-xs font-semibold text-slate-600">
                                Stock {{ product.stock }}
                            </span>
                        </div>

                        <p class="mt-3 text-sm leading-6 text-slate-500">{{ product.description }}</p>

                        <div class="mt-auto flex flex-col gap-4 pt-6 sm:flex-row sm:items-end sm:justify-between">
                            <div>
                                <p class="text-xs font-medium uppercase tracking-[0.2em] text-slate-400">Precio</p>
                                <p class="mt-2 text-2xl font-semibold text-slate-900">{{ formatCurrency(product.price) }}</p>
                            </div>

                            <div class="flex flex-wrap gap-3">
                                <button
                                    type="button"
                                    class="rounded-full border border-slate-200 px-4 py-2.5 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-50 cursor-pointer"
                                    @click="goToEditProduct(product)"
                                >
                                    Editar
                                </button>

                                <button
                                    type="button"
                                    class="rounded-full bg-slate-900 px-4 py-2.5 text-sm font-semibold text-white transition hover:bg-slate-700 cursor-pointer"
                                    @click="goToProduct(product.productId)"
                                >
                                    Ver detalle
                                </button>
                            </div>
                        </div>
                    </div>
                </article>
            </div>

            <div v-else class="space-y-4">
                <article
                    v-for="product in filteredProducts"
                    :key="product.productId"
                    class="rounded-[28px] border border-slate-200 bg-white p-4 shadow-sm shadow-slate-200/70"
                >
                    <div class="flex flex-col gap-4 md:flex-row md:items-center">
                        <div class="h-28 w-full shrink-0 overflow-hidden rounded-3xl bg-slate-100 md:w-28">
                            <img
                                :src="getProductImage(product)"
                                :alt="product.name"
                                class="h-full w-full object-cover"
                            >
                        </div>

                        <div class="flex-1">
                            <div class="flex flex-col gap-4 lg:flex-row lg:items-start lg:justify-between">
                                <div>
                                    <div class="flex flex-wrap items-center gap-2">
                                        <h2 class="text-lg font-semibold text-slate-900">{{ product.name }}</h2>
                                        <span class="rounded-full bg-slate-100 px-3 py-1 text-xs font-semibold text-slate-600">
                                            {{ product.category }}
                                        </span>
                                        <span
                                            class="rounded-full px-3 py-1 text-xs font-semibold"
                                            :class="getInventoryStatus(product).badgeClass"
                                        >
                                            {{ getInventoryStatus(product).label }}
                                        </span>
                                    </div>

                                    <p class="mt-3 text-sm leading-6 text-slate-500">{{ product.description }}</p>
                                </div>

                                <div class="text-left lg:text-right">
                                    <p class="text-xs font-medium uppercase tracking-[0.2em] text-slate-400">Actualizado</p>
                                    <p class="mt-2 text-sm font-semibold text-slate-700">{{ formatDate(product.updatedDate || product.createdDate) }}</p>
                                </div>
                            </div>

                            <div class="mt-5 flex flex-col gap-4 xl:flex-row xl:items-center xl:justify-between">
                                <div class="flex flex-wrap items-center gap-3 text-sm text-slate-500">
                                    <span>Stock disponible: {{ product.stock }}</span>
                                    <span>{{ product.isActive ? 'Visible en tienda' : 'Oculto en tienda' }}</span>
                                    <span v-if="product.isNew">Recién agregado</span>
                                </div>

                                <div class="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-end">
                                    <button
                                        type="button"
                                        class="rounded-full border border-slate-200 px-4 py-2.5 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-50 cursor-pointer"
                                        @click="goToEditProduct(product)"
                                    >
                                        Editar
                                    </button>

                                    <p class="text-xl font-semibold text-slate-900">{{ formatCurrency(product.price) }}</p>
                                    <button
                                        type="button"
                                        class="rounded-full bg-slate-900 px-4 py-2.5 text-sm font-semibold text-white transition hover:bg-slate-700 cursor-pointer"
                                        @click="goToProduct(product.productId)"
                                    >
                                        Ver detalle
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </article>
            </div>

            <div class="mt-6 flex flex-col gap-4 rounded-[28px] border border-slate-200 bg-white px-5 py-4 shadow-sm shadow-slate-200/70 sm:flex-row sm:items-center sm:justify-between">
                <div class="text-sm text-slate-500">
                    Página {{ currentPage }} de {{ totalPages }}
                    <span class="ml-2 inline-block">· {{ totalItems }} producto(s) en total</span>
                </div>

                <div class="flex items-center gap-3">
                    <button
                        type="button"
                        class="rounded-full border border-slate-200 px-4 py-2 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-50 disabled:cursor-not-allowed disabled:border-slate-100 disabled:text-slate-300 cursor-pointer"
                        :disabled="loading || !hasPreviousPage"
                        @click="changePage(currentPage - 1)"
                    >
                        Anterior
                    </button>

                    <span class="min-w-24 text-center text-sm font-semibold text-slate-700">
                        {{ currentPage }} / {{ totalPages }}
                    </span>

                    <button
                        type="button"
                        class="rounded-full border border-slate-200 px-4 py-2 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-50 disabled:cursor-not-allowed disabled:border-slate-100 disabled:text-slate-300 cursor-pointer"
                        :disabled="loading || !hasNextPage"
                        @click="changePage(currentPage + 1)"
                    >
                        Siguiente
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { toast } from 'vue3-toastify';
import { fetchAdminProducts, type PaginatedResponse, type Product } from '../../api/productApi';
import { ROUTER_NAME } from '../../routes/router-name';
import {
    getAdminProductPageState,
    hasAdminProductPageState,
    setAdminProductPageState,
    setSelectedAdminProduct,
} from '../../service/adminProductStore';
import { formatCurrency } from '../../service/service';
import {
    DEFAULT_PRODUCT_FILTERS,
    PRODUCT_PRICE_OPTIONS,
    PRODUCT_STATUS_OPTIONS,
    filterProducts,
    formatProductDate as formatDate,
    getActiveProductFilterCount,
    getInventoryStatus,
    getProductCategoryOptions,
    getProductImage,
    type PriceRange,
    type ProductFilterState,
    type StockFilter,
} from '../../service/productService';

type ViewMode = 'grid' | 'list';

const router = useRouter();
const cachedProductPage = getAdminProductPageState();

const loading = ref(false);
const errorMessage = ref('');
const products = ref<Product[]>(cachedProductPage.items);
const pageSize = 10;
const currentPage = ref(cachedProductPage.page);
const totalPages = ref(cachedProductPage.totalPages);
const totalItems = ref(cachedProductPage.totalItems);
const hasPreviousPage = ref(cachedProductPage.hasPreviousPage);
const hasNextPage = ref(cachedProductPage.hasNextPage);

const viewMode = ref<ViewMode>('grid');
const selectedCategory = ref<string>(DEFAULT_PRODUCT_FILTERS.category);
const selectedPriceRange = ref<PriceRange>(DEFAULT_PRODUCT_FILTERS.priceRange);
const selectedStatus = ref<StockFilter>(DEFAULT_PRODUCT_FILTERS.status);

const skeletonCards = Array.from({ length: pageSize }, (_, index) => index);
const priceOptions = PRODUCT_PRICE_OPTIONS;
const statusOptions = PRODUCT_STATUS_OPTIONS;

const currentFilters = computed<ProductFilterState>(() => ({
    category: selectedCategory.value,
    priceRange: selectedPriceRange.value,
    status: selectedStatus.value,
}));

const categoryOptions = computed(() => getProductCategoryOptions(products.value));

const activeFilterCount = computed(() => {
    return getActiveProductFilterCount(currentFilters.value);
});

const filteredProducts = computed(() => {
    return filterProducts(products.value, currentFilters.value);
});

const resetFilters = () => {
    selectedCategory.value = DEFAULT_PRODUCT_FILTERS.category;
    selectedPriceRange.value = DEFAULT_PRODUCT_FILTERS.priceRange;
    selectedStatus.value = DEFAULT_PRODUCT_FILTERS.status;
};

const syncProductsFromPageState = (pageState: PaginatedResponse<Product> | ReturnType<typeof getAdminProductPageState>) => {
    products.value = pageState.items;
    totalItems.value = pageState.totalItems;
    totalPages.value = pageState.totalPages;
    currentPage.value = pageState.page;
    hasPreviousPage.value = pageState.hasPreviousPage;
    hasNextPage.value = pageState.hasNextPage;
};

const goToProduct = (productId: number) => {
    router.push({
        name: ROUTER_NAME.ADMIN.EDIT_PRODUCT,
        params: { id: productId },
    });
};

const goToCreateProduct = () => {
    router.push({ name: ROUTER_NAME.ADMIN.CREATE_PRODUCT });
};

const goToEditProduct = (product: Product) => {
    setSelectedAdminProduct(product);
    router.push({
        name: ROUTER_NAME.ADMIN.EDIT_PRODUCT,
        params: { id: product.productId },
    });
};

const changePage = (page: number) => {
    if (loading.value || page < 1 || page > totalPages.value || page === currentPage.value) {
        return;
    }

    void loadProducts(page);
};

const loadProducts = async (page: number = currentPage.value) => {
    const safePage = Math.max(1, page);

    loading.value = true;
    errorMessage.value = '';

    try {
        const response = await fetchAdminProducts({
            page: safePage,
            pageSize,
        });
        const normalizedTotalPages = Math.max(response.totalPages, 1);

        if (response.totalItems > 0 && safePage > normalizedTotalPages) {
            currentPage.value = normalizedTotalPages;
            await loadProducts(normalizedTotalPages);
            return;
        }

        const nextPageState: PaginatedResponse<Product> = {
            ...response,
            totalPages: normalizedTotalPages,
            page: Math.min(safePage, normalizedTotalPages),
            hasPreviousPage: Math.min(safePage, normalizedTotalPages) > 1,
            hasNextPage: Math.min(safePage, normalizedTotalPages) < normalizedTotalPages,
        };

        setAdminProductPageState(nextPageState);
        syncProductsFromPageState(nextPageState);
    } catch (error) {
        const message = error instanceof Error ? error.message : 'No fue posible recuperar los productos.';

        errorMessage.value = 'Ocurrió un problema al consultar la API. Verifica la conexión con el backend e intenta nuevamente.';
        toast.error(message);
    } finally {
        loading.value = false;
    }
};

onMounted(() => {
    if (hasAdminProductPageState()) {
        syncProductsFromPageState(getAdminProductPageState());
        return;
    }

    void loadProducts();
});
</script>