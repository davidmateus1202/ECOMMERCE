<template>
    <div class="overflow-x-hidden bg-white text-black">
        <section class="border-b border-black/10 bg-[#f6f6f4]">
            <div class="mx-auto max-w-7xl px-6 py-12 md:px-10 md:py-16">
                <p class="text-xs font-semibold uppercase tracking-[0.2em] text-black/45">Curated section</p>
                <div class="mt-4 grid gap-8 lg:grid-cols-[1.1fr_0.9fr] lg:items-end">
                    <div>
                        <h1 class="max-w-3xl text-4xl font-semibold leading-[1.02] text-black md:text-6xl">{{ sectionTitle }}</h1>
                        <p class="mt-5 max-w-2xl text-base leading-8 text-black/62">{{ sectionDescription }}</p>
                    </div>
                    <div class="rounded-4xl border border-black/8 bg-white p-6 shadow-sm shadow-black/5">
                        <p class="text-xs font-semibold uppercase tracking-[0.18em] text-black/45">Inventario activo</p>
                        <p class="mt-3 text-4xl font-semibold text-black">{{ products.length }}</p>
                        <p class="mt-2 text-sm leading-7 text-black/60">Productos conectados directamente al backend y listos para navegacion, detalle y compra por WhatsApp.</p>
                    </div>
                </div>
            </div>
        </section>

        <ProductGrid
            :section-id="`${sectionKey}-featured`"
            :eyebrow="featuredEyebrow"
            title="Seleccion destacada"
            description="Referencias priorizadas desde el backend para esta categoria."
            :products="featuredProducts"
            cta-label="Comprar"
            @quick-view="openQuickView"
        />

        <ProductGrid
            :section-id="`${sectionKey}-all`"
            :eyebrow="allEyebrow"
            :title="allTitle"
            :description="allDescription"
            :products="products"
            cta-label="Comprar"
            @quick-view="openQuickView"
        />

        <section v-if="products.length === 0" class="mx-auto max-w-7xl px-6 pb-16 md:px-10">
            <div class="rounded-4xl border border-black/10 bg-[#fafaf8] p-8 text-center">
                <p class="text-2xl font-semibold text-black">{{ emptyTitle }}</p>
                <p class="mt-3 text-sm leading-7 text-black/60">{{ emptyDescription }}</p>
            </div>
        </section>

        <ProductQuickViewModal
            :product="selectedProduct"
            @close="selectedProduct = null"
        />
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import ProductGrid from "./ProductGrid.vue";
import ProductQuickViewModal from "./ProductQuickViewModal.vue";
import { fetchProductsByCategory, type CategoryProductsResponse } from "../../api/productApi";
import type { ShopProduct } from "../../types/shopProduct";

const props = defineProps<{
    category: string;
    sectionKey: string;
    fallbackTitle: string;
    fallbackDescription: string;
    featuredEyebrow: string;
    allEyebrow: string;
    allTitle: string;
    allDescription: string;
    emptyTitle: string;
    emptyDescription: string;
}>();

const categoryData = ref<CategoryProductsResponse | null>(null);
const selectedProduct = ref<ShopProduct | null>(null);

const products = computed(() => categoryData.value?.products ?? []);
const featuredProducts = computed(() => categoryData.value?.featuredProducts ?? []);
const sectionConfig = computed(() => categoryData.value?.sectionConfig ?? null);
const sectionTitle = computed(() => sectionConfig.value?.title ?? categoryData.value?.title ?? props.fallbackTitle);
const sectionDescription = computed(() => sectionConfig.value?.description ?? categoryData.value?.description ?? props.fallbackDescription);
const featuredEyebrow = computed(() => sectionConfig.value?.featuredEyebrow ?? props.featuredEyebrow);
const allEyebrow = computed(() => sectionConfig.value?.allEyebrow ?? props.allEyebrow);
const allTitle = computed(() => sectionConfig.value?.allTitle ?? props.allTitle);
const allDescription = computed(() => sectionConfig.value?.allDescription ?? props.allDescription);
const emptyTitle = computed(() => sectionConfig.value?.emptyTitle ?? props.emptyTitle);
const emptyDescription = computed(() => sectionConfig.value?.emptyDescription ?? props.emptyDescription);

const loadCategory = async () => {
    categoryData.value = await fetchProductsByCategory(props.category);
};

const openQuickView = (product: ShopProduct) => {
    selectedProduct.value = product;
};

onMounted(() => {
    loadCategory();
});
</script>