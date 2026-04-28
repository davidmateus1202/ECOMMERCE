<template>
    <div class="min-h-full bg-[#f7f7fb] p-5 md:p-8">
        <div v-if="loading" class="rounded-4xl bg-white p-8 text-center shadow-sm shadow-slate-200/70">
            <p class="text-lg font-semibold text-slate-900">Cargando editor...</p>
            <p class="mt-2 text-sm text-slate-500">Recuperando la información más reciente del producto.</p>
        </div>

        <div v-else-if="!formState" class="rounded-4xl bg-white p-8 text-center shadow-sm shadow-slate-200/70">
            <h1 class="text-2xl font-bold text-slate-900">No encontramos el producto</h1>
            <p class="mt-3 text-sm text-slate-500">
                Regresa al catálogo y selecciona un producto válido, o crea uno nuevo desde el menú lateral.
            </p>
            <button
                type="button"
                class="mt-6 rounded-full bg-slate-900 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-700 cursor-pointer"
                @click="goBack"
            >
                Volver al catálogo
            </button>
        </div>

        <div v-else class="mx-auto flex max-w-7xl flex-col gap-6">
            <section class="rounded-4xl bg-white p-6 shadow-sm shadow-slate-200/70 md:p-8">
                <div class="flex flex-col gap-5 xl:flex-row xl:items-start xl:justify-between">
                    <div>
                        <p class="text-sm font-semibold text-slate-400">Productos / {{ isCreateMode ? 'Nuevo producto' : 'Editar producto' }}</p>
                        <h1 class="mt-3 text-3xl font-bold tracking-tight text-slate-900 md:text-4xl">
                            {{ isCreateMode ? 'Crear nuevo producto' : `Editar Producto: ${formState.name}` }}
                        </h1>
                        <p class="mt-3 max-w-3xl text-sm text-slate-500 md:text-base">
                            {{ isCreateMode ? 'Completa la información para publicar un nuevo producto y dejarlo disponible para la tienda.' : 'Actualiza el producto, sus imágenes y su visibilidad. Los cambios se guardan directamente en el backend.' }}
                        </p>
                    </div>

                    <div class="flex flex-col gap-3 sm:flex-row">
                        <button
                            v-if="!isCreateMode"
                            type="button"
                            class="rounded-full border border-rose-200 px-5 py-3 text-sm font-semibold text-rose-600 transition hover:border-rose-300 hover:bg-rose-50 cursor-pointer"
                            :disabled="saving || deleting"
                            @click="deleteCurrentProduct"
                        >
                            {{ deleting ? 'Eliminando...' : 'Eliminar' }}
                        </button>
                        <button
                            type="button"
                            class="rounded-full border border-slate-200 px-5 py-3 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-50 cursor-pointer"
                            @click="goBack"
                        >
                            Cancelar
                        </button>
                        <button
                            type="button"
                            class="rounded-2xl bg-indigo-600 px-6 py-3 text-sm font-semibold text-white shadow-lg shadow-indigo-200 transition hover:bg-indigo-500 disabled:cursor-not-allowed disabled:bg-indigo-300 cursor-pointer"
                            :disabled="saving || !canSave"
                            @click="saveChanges"
                        >
                            {{ saving ? 'Guardando...' : isCreateMode ? 'Crear producto' : 'Guardar cambios' }}
                        </button>
                    </div>
                </div>
            </section>

            <section class="grid gap-6 xl:grid-cols-[minmax(0,1.75fr)_380px]">
                <div class="space-y-6">
                    <article class="rounded-4xl bg-white p-6 shadow-sm shadow-slate-200/70 md:p-7">
                        <div class="flex items-center gap-3">
                            <div class="flex h-11 w-11 items-center justify-center rounded-2xl bg-indigo-50 text-indigo-600">
                                <IconInfoCircle class="h-5 w-5" />
                            </div>
                            <div>
                                <h2 class="text-xl font-semibold text-slate-900">Información del producto</h2>
                                <p class="text-sm text-slate-500">Datos visibles en catálogo, detalle y panel de administración.</p>
                            </div>
                        </div>

                        <div class="mt-6 grid gap-5 md:grid-cols-2">
                            <label class="block md:col-span-2">
                                <span class="mb-2 block text-sm font-semibold text-slate-600">Nombre del producto</span>
                                <input v-model="formState.name" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100" placeholder="Nombre del producto">
                            </label>

                            <label class="block">
                                <span class="mb-2 block text-sm font-semibold text-slate-600">Precio</span>
                                <input v-model.number="formState.price" type="number" min="0" step="0.01" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100" placeholder="0.00">
                            </label>

                            <label class="block">
                                <span class="mb-2 block text-sm font-semibold text-slate-600">Stock</span>
                                <input v-model.number="formState.stock" type="number" min="0" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100" placeholder="0">
                            </label>

                            <label class="block">
                                <span class="mb-2 block text-sm font-semibold text-slate-600">Categoría</span>
                                <input v-model="formState.category" list="product-category-options" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100" placeholder="Bolsos, Accesorios, Catalogo...">
                                <datalist id="product-category-options">
                                    <option v-for="category in categoryOptions" :key="category" :value="category"></option>
                                </datalist>
                            </label>

                            <label class="block">
                                <span class="mb-2 block text-sm font-semibold text-slate-600">SKU</span>
                                <input v-model="formState.sku" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100" placeholder="SKU del producto">
                            </label>

                            <label class="block md:col-span-2">
                                <span class="mb-2 block text-sm font-semibold text-slate-600">Descripción</span>
                                <textarea v-model="formState.description" rows="9" class="w-full rounded-[28px] border border-slate-200 bg-slate-50 px-4 py-4 text-sm leading-7 text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100" placeholder="Describe el producto"></textarea>
                            </label>
                        </div>
                    </article>
                </div>

                <div class="space-y-6">
                    <article class="rounded-4xl bg-white p-6 shadow-sm shadow-slate-200/70">
                        <div class="flex items-center gap-3">
                            <div class="flex h-11 w-11 items-center justify-center rounded-2xl bg-indigo-50 text-indigo-600">
                                <IconPhoto class="h-5 w-5" />
                            </div>
                            <div>
                                <h2 class="text-xl font-semibold text-slate-900">Imágenes</h2>
                                <p class="text-sm text-slate-500">Puedes registrar hasta 6 URLs y marcar una imagen principal.</p>
                            </div>
                        </div>

                        <div class="mt-6 overflow-hidden rounded-[28px] border border-slate-200 bg-slate-50 p-4">
                            <img :src="activeImage" :alt="formState.name || 'Vista previa'" class="aspect-square w-full rounded-3xl object-cover">
                        </div>

                        <div class="mt-4 flex gap-3">
                            <input v-model="newImageUrl" type="url" class="h-12 flex-1 rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100" placeholder="https://...">
                            <button type="button" class="rounded-2xl border border-slate-200 px-4 py-3 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-50 cursor-pointer" :disabled="formState.images.length >= 6" @click="addImageUrl">
                                Añadir
                            </button>
                        </div>

                        <div class="mt-3 rounded-2xl border border-dashed border-slate-300 bg-slate-50 p-4">
                            <div class="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
                                <div>
                                    <p class="text-sm font-semibold text-slate-800">Subir archivos</p>
                                    <p class="mt-1 text-xs text-slate-500">Selecciona varias imágenes locales. Se subirán al backend y se agregarán al producto.</p>
                                </div>
                                <div class="flex flex-wrap gap-2">
                                    <input
                                        ref="fileInput"
                                        type="file"
                                        accept="image/png,image/jpeg,image/jpg,image/webp,image/gif"
                                        multiple
                                        class="hidden"
                                        @change="handleFileSelection"
                                    >
                                    <button type="button" class="rounded-2xl border border-slate-200 bg-white px-4 py-3 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-50 cursor-pointer" @click="openFilePicker">
                                        Seleccionar archivos
                                    </button>
                                    <button type="button" class="rounded-2xl bg-slate-900 px-4 py-3 text-sm font-semibold text-white transition hover:bg-slate-700 disabled:cursor-not-allowed disabled:bg-slate-300 cursor-pointer" :disabled="uploadingImages || selectedFiles.length === 0 || remainingImageSlots === 0" @click="uploadSelectedFiles">
                                        {{ uploadingImages ? 'Subiendo...' : `Subir ${selectedFiles.length || ''} archivo(s)` }}
                                    </button>
                                </div>
                            </div>
                            <p v-if="selectedFiles.length > 0" class="mt-3 text-xs text-slate-500">
                                Seleccionados: {{ selectedFileNames }}
                            </p>
                            <p class="mt-2 text-xs text-slate-400">Espacios disponibles: {{ remainingImageSlots }} de 6.</p>
                        </div>

                        <div class="mt-4 space-y-3">
                            <div
                                v-for="(image, index) in formState.images"
                                :key="image.key"
                                class="rounded-2xl border border-slate-200 bg-slate-50 p-3"
                            >
                                <div class="flex items-start gap-3">
                                    <button type="button" class="shrink-0 overflow-hidden rounded-2xl border border-slate-200 bg-white p-1 cursor-pointer" @click="selectedImageIndex = index">
                                        <img :src="image.imageUrl" :alt="`Imagen ${index + 1}`" class="h-16 w-16 rounded-xl object-cover">
                                    </button>
                                    <div class="min-w-0 flex-1">
                                        <p class="truncate text-sm font-semibold text-slate-800">{{ image.imageUrl }}</p>
                                        <div class="mt-3 flex flex-wrap gap-2">
                                            <button type="button" class="rounded-full px-3 py-1 text-xs font-semibold transition cursor-pointer" :class="image.isMain ? 'bg-emerald-100 text-emerald-700' : 'bg-white text-slate-600 border border-slate-200 hover:border-slate-300'" @click="setMainImage(index)">
                                                {{ image.isMain ? 'Imagen principal' : 'Marcar principal' }}
                                            </button>
                                            <button type="button" class="rounded-full bg-rose-50 px-3 py-1 text-xs font-semibold text-rose-600 transition hover:bg-rose-100 cursor-pointer" @click="removeImage(index)">
                                                Quitar
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <p v-if="formState.images.length === 0" class="rounded-2xl border border-dashed border-slate-300 bg-slate-50 px-4 py-5 text-sm text-slate-500">
                                Aún no hay imágenes registradas. Puedes crear el producto sin imágenes, pero no es recomendable para la tienda pública.
                            </p>
                        </div>
                    </article>

                    <article class="rounded-4xl bg-white p-6 shadow-sm shadow-slate-200/70">
                        <div class="flex items-center gap-3">
                            <div class="flex h-11 w-11 items-center justify-center rounded-2xl bg-indigo-50 text-indigo-600">
                                <IconPackage class="h-5 w-5" />
                            </div>
                            <div>
                                <h2 class="text-xl font-semibold text-slate-900">Estado comercial</h2>
                                <p class="text-sm text-slate-500">Define visibilidad, novedad y contexto de inventario.</p>
                            </div>
                        </div>

                        <div class="mt-6 space-y-5">
                            <label class="flex items-center justify-between gap-4 rounded-2xl border border-slate-200 bg-slate-50 px-4 py-3 cursor-pointer">
                                <div>
                                    <p class="text-sm font-semibold text-slate-800">Producto visible</p>
                                    <p class="text-xs text-slate-500">Controla si el producto se muestra en la tienda pública.</p>
                                </div>
                                <input v-model="formState.isActive" type="checkbox" class="h-5 w-5 rounded border-slate-300 text-indigo-600 focus:ring-indigo-500">
                            </label>

                            <label class="flex items-center justify-between gap-4 rounded-2xl border border-slate-200 bg-slate-50 px-4 py-3 cursor-pointer">
                                <div>
                                    <p class="text-sm font-semibold text-slate-800">Marcar como nuevo</p>
                                    <p class="text-xs text-slate-500">Aparecerá destacado en novedades y storefront.</p>
                                </div>
                                <input v-model="formState.isNew" type="checkbox" class="h-5 w-5 rounded border-slate-300 text-indigo-600 focus:ring-indigo-500">
                            </label>

                            <div class="rounded-[28px] border border-slate-200 bg-slate-50 p-4">
                                <div class="flex items-center gap-3">
                                    <span class="inline-flex h-3 w-3 rounded-full" :class="inventoryStatusDotClass"></span>
                                    <div>
                                        <p class="text-sm font-semibold text-slate-900">{{ inventoryStatus.label }}</p>
                                        <p class="text-xs text-slate-500">{{ formState.isActive ? 'Publicado y visible en el catálogo' : 'Oculto del catálogo hasta nueva activación' }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </article>
                </div>
            </section>
        </div>
    </div>
</template>

<script setup lang="ts">
import { IconInfoCircle, IconPackage, IconPhoto } from '@tabler/icons-vue';
import { computed, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { toast } from 'vue3-toastify';
import {
    createAdminProduct,
    deleteAdminProduct,
    fetchAdminProductById,
    type Product,
    type ProductImageInput,
    type ProductUpsertPayload,
    uploadAdminProductImages,
    updateAdminProduct,
} from '../../api/productApi';
import { ROUTER_NAME } from '../../routes/router-name';
import {
    getAdminProductPageState,
    getSelectedAdminProduct,
    removeCachedAdminProduct,
    upsertCachedAdminProduct,
} from '../../service/adminProductStore';
import { FALLBACK_PRODUCT_IMAGE, getInventoryStatus } from '../../service/productService';
import type { UploadedProductImage } from '../../api/productApi';

interface EditableProductImage {
    key: string;
    imageUrl: string;
    isMain: boolean;
}

interface EditableProductFormState {
    productId: number;
    name: string;
    description: string;
    price: number;
    stock: number;
    category: string;
    sku: string;
    isActive: boolean;
    isNew: boolean;
    createdDate: string;
    updatedDate: string;
    images: EditableProductImage[];
}

const route = useRoute();
const router = useRouter();

const loading = ref(false);
const saving = ref(false);
const deleting = ref(false);
const uploadingImages = ref(false);
const formState = ref<EditableProductFormState | null>(null);
const selectedImageIndex = ref(0);
const newImageUrl = ref('');
const initialFormSnapshot = ref('');
const selectedFiles = ref<File[]>([]);
const fileInput = ref<HTMLInputElement | null>(null);

const isCreateMode = computed(() => route.name === ROUTER_NAME.ADMIN.CREATE_PRODUCT);

const productId = computed(() => {
    const rawProductId = route.params.id;
    const normalizedProductId = Array.isArray(rawProductId) ? rawProductId[0] : rawProductId;
    const numericProductId = Number(normalizedProductId);
    return Number.isFinite(numericProductId) ? numericProductId : null;
});

const createEmptyFormState = (): EditableProductFormState => ({
    productId: 0,
    name: '',
    description: '',
    price: 0,
    stock: 0,
    category: '',
    sku: '',
    isActive: true,
    isNew: false,
    createdDate: new Date().toISOString(),
    updatedDate: new Date().toISOString(),
    images: [],
});

const buildProductSku = (product: Product) => product.sku?.trim() || `PRD-${String(product.productId).padStart(4, '0')}`;

const buildEditableImages = (product: Product): EditableProductImage[] => {
    return product.productImages.map((image, index) => ({
        key: `${product.productId}-${image.productImageId}-${index}`,
        imageUrl: image.imageUrl,
        isMain: image.isMain,
    }));
};

const buildFormState = (product: Product): EditableProductFormState => ({
    productId: product.productId,
    name: product.name,
    description: product.description,
    price: product.price,
    stock: product.stock,
    category: product.category,
    sku: buildProductSku(product),
    isActive: product.isActive,
    isNew: product.isNew,
    createdDate: product.createdDate,
    updatedDate: product.updatedDate,
    images: buildEditableImages(product),
});

const serializeFormState = (state: EditableProductFormState | null) => JSON.stringify(state);

const buildPayload = (state: EditableProductFormState): ProductUpsertPayload => ({
    name: state.name.trim(),
    sku: state.sku.trim() || null,
    description: state.description.trim(),
    price: Number.isFinite(state.price) ? Math.max(0, state.price) : 0,
    stock: Number.isFinite(state.stock) ? Math.max(0, Math.trunc(state.stock)) : 0,
    category: state.category.trim(),
    isActive: state.isActive,
    isNew: state.isNew,
    productImages: state.images.map<ProductImageInput>((image) => ({
        imageUrl: image.imageUrl.trim(),
        isMain: image.isMain,
    })),
});

const loadProduct = async () => {
    loading.value = true;

    try {
        if (isCreateMode.value) {
            formState.value = createEmptyFormState();
            initialFormSnapshot.value = serializeFormState(formState.value);
            return;
        }

        const cachedProduct = getSelectedAdminProduct(productId.value);
        const product = cachedProduct ?? (productId.value === null ? null : await fetchAdminProductById(productId.value));

        if (!product) {
            formState.value = null;
            initialFormSnapshot.value = '';
            return;
        }

        formState.value = buildFormState(product);
        initialFormSnapshot.value = serializeFormState(formState.value);
    } catch (error) {
        formState.value = null;
        toast.error(error instanceof Error ? error.message : 'No fue posible cargar el producto.');
    } finally {
        selectedImageIndex.value = 0;
        loading.value = false;
    }
};

watch([isCreateMode, productId], () => {
    void loadProduct();
}, { immediate: true });

const cachedProducts = computed(() => getAdminProductPageState().items);

const categoryOptions = computed(() => {
    const categories = new Set(['Accesorios', 'Bolsos', 'Catalogo']);

    cachedProducts.value.forEach((product) => {
        if (product.category.trim()) {
            categories.add(product.category);
        }
    });

    if (formState.value?.category.trim()) {
        categories.add(formState.value.category.trim());
    }

    return Array.from(categories).sort((left, right) => left.localeCompare(right, 'es'));
});

const activeImage = computed(() => {
    if (!formState.value || formState.value.images.length === 0) {
        return FALLBACK_PRODUCT_IMAGE;
    }

    const safeIndex = Math.min(selectedImageIndex.value, formState.value.images.length - 1);
    return formState.value.images[safeIndex]?.imageUrl ?? FALLBACK_PRODUCT_IMAGE;
});

const remainingImageSlots = computed(() => {
    return Math.max(0, 6 - (formState.value?.images.length ?? 0));
});

const selectedFileNames = computed(() => {
    return selectedFiles.value.map((file) => file.name).join(', ');
});

const inventoryStatus = computed(() => {
    if (!formState.value) {
        return { label: 'Sin estado', badgeClass: 'bg-slate-200 text-slate-700' };
    }

    const transientProduct: Product = {
        productId: formState.value.productId,
        name: formState.value.name,
        description: formState.value.description,
        price: formState.value.price,
        stock: formState.value.stock,
        category: formState.value.category,
        sku: formState.value.sku,
        isActive: formState.value.isActive,
        isNew: formState.value.isNew,
        createdDate: formState.value.createdDate,
        updatedDate: formState.value.updatedDate,
        productImages: formState.value.images.map((image, index) => ({
            productImageId: index + 1,
            productId: formState.value?.productId ?? 0,
            imageUrl: image.imageUrl,
            isMain: image.isMain,
        })),
    };

    return getInventoryStatus(transientProduct);
});

const inventoryStatusDotClass = computed(() => {
    if (inventoryStatus.value.badgeClass.includes('emerald')) return 'bg-emerald-500';
    if (inventoryStatus.value.badgeClass.includes('amber')) return 'bg-amber-500';
    if (inventoryStatus.value.badgeClass.includes('rose')) return 'bg-rose-500';
    return 'bg-slate-400';
});

const hasChanges = computed(() => serializeFormState(formState.value) !== initialFormSnapshot.value);

const isFormValid = computed(() => {
    if (!formState.value) {
        return false;
    }

    return formState.value.name.trim().length > 0
        && formState.value.description.trim().length > 0
        && formState.value.category.trim().length > 0
        && formState.value.price >= 0
        && formState.value.stock >= 0;
});

const canSave = computed(() => isCreateMode.value ? isFormValid.value : isFormValid.value && hasChanges.value);

const goBack = () => {
    router.push({ name: ROUTER_NAME.ADMIN.PRODUCTS });
};

const setMainImage = (index: number) => {
    if (!formState.value) {
        return;
    }

    formState.value.images = formState.value.images.map((image, currentIndex) => ({
        ...image,
        isMain: currentIndex === index,
    }));
    selectedImageIndex.value = index;
};

const addImageUrl = () => {
    if (!formState.value) {
        return;
    }

    const normalizedUrl = newImageUrl.value.trim();

    if (!normalizedUrl) {
        toast.error('Ingresa una URL de imagen válida.');
        return;
    }

    if (formState.value.images.length >= 6) {
        toast.error('Solo se permiten hasta 6 imágenes por producto.');
        return;
    }

    formState.value.images.push({
        key: `${Date.now()}-${formState.value.images.length}`,
        imageUrl: normalizedUrl,
        isMain: formState.value.images.length === 0,
    });

    selectedImageIndex.value = formState.value.images.length - 1;
    newImageUrl.value = '';
};

const openFilePicker = () => {
    fileInput.value?.click();
};

const handleFileSelection = (event: Event) => {
    const input = event.target as HTMLInputElement;
    const files = Array.from(input.files ?? []);

    if (files.length === 0) {
        selectedFiles.value = [];
        return;
    }

    selectedFiles.value = files.slice(0, remainingImageSlots.value);

    if (files.length > remainingImageSlots.value) {
        toast.info(`Solo se cargarán ${remainingImageSlots.value} archivo(s) para respetar el límite de 6 imágenes.`);
    }
};

const appendUploadedImages = (uploadedImages: UploadedProductImage[]) => {
    if (!formState.value) {
        return;
    }

    uploadedImages.forEach((image) => {
        if (formState.value && formState.value.images.length < 6) {
            formState.value.images.push({
                key: `${Date.now()}-${image.fileName}`,
                imageUrl: image.imageUrl,
                isMain: formState.value.images.length === 0,
            });
        }
    });

    if (formState.value.images.length > 0 && !formState.value.images.some((image) => image.isMain)) {
        formState.value.images[0]!.isMain = true;
    }
};

const uploadSelectedFiles = async () => {
    if (selectedFiles.value.length === 0 || uploadingImages.value || !formState.value) {
        return;
    }

    uploadingImages.value = true;

    try {
        const uploadedImages = await uploadAdminProductImages(selectedFiles.value);
        appendUploadedImages(uploadedImages);
        selectedFiles.value = [];

        if (fileInput.value) {
            fileInput.value.value = '';
        }

        toast.success('Imágenes subidas correctamente.');
    } catch (error) {
        toast.error(error instanceof Error ? error.message : 'No fue posible subir las imágenes.');
    } finally {
        uploadingImages.value = false;
    }
};

const removeImage = (index: number) => {
    if (!formState.value) {
        return;
    }

    const removedWasMain = formState.value.images[index]?.isMain;
    formState.value.images.splice(index, 1);

    if (removedWasMain && formState.value.images.length > 0) {
        const firstImage = formState.value.images[0];

        if (firstImage) {
            firstImage.isMain = true;
        }
    }

    selectedImageIndex.value = Math.max(0, Math.min(selectedImageIndex.value, formState.value.images.length - 1));
};

const saveChanges = async () => {
    if (!formState.value || saving.value || !isFormValid.value) {
        return;
    }

    saving.value = true;

    try {
        const payload = buildPayload(formState.value);

        if (isCreateMode.value) {
            const createdProduct = await createAdminProduct(payload);
            upsertCachedAdminProduct(createdProduct);
            toast.success('Producto creado correctamente.');
            await router.push({ name: ROUTER_NAME.ADMIN.EDIT_PRODUCT, params: { id: createdProduct.productId } });
            return;
        }

        const updatedProduct = await updateAdminProduct(formState.value.productId, payload);
        upsertCachedAdminProduct(updatedProduct);
        formState.value = buildFormState(updatedProduct);
        initialFormSnapshot.value = serializeFormState(formState.value);
        toast.success('Producto actualizado correctamente.');
    } catch (error) {
        toast.error(error instanceof Error ? error.message : 'No fue posible guardar el producto.');
    } finally {
        saving.value = false;
    }
};

const deleteCurrentProduct = async () => {
    if (!formState.value || isCreateMode.value || deleting.value) {
        return;
    }

    const confirmed = window.confirm(`Se eliminará el producto "${formState.value.name}". ¿Deseas continuar?`);

    if (!confirmed) {
        return;
    }

    deleting.value = true;

    try {
        await deleteAdminProduct(formState.value.productId);
        removeCachedAdminProduct(formState.value.productId);
        toast.success('Producto eliminado correctamente.');
        await router.push({ name: ROUTER_NAME.ADMIN.PRODUCTS });
    } catch (error) {
        toast.error(error instanceof Error ? error.message : 'No fue posible eliminar el producto.');
    } finally {
        deleting.value = false;
    }
};
</script>
