<template>
    <section id="top-products" class="mx-auto w-full max-w-7xl px-6 py-12 md:px-10 md:py-16">
        <div class="mb-8 flex flex-wrap items-end justify-between gap-4 border-b border-black/10 pb-5">
            <div>
                <p class="text-xs font-semibold uppercase tracking-[0.2em] text-black/45">Top products</p>
                <h2 class="mt-2 text-3xl font-semibold text-black md:text-4xl">Productos destacados</h2>
            </div>

            <div class="flex flex-wrap gap-2">
                <button
                    v-for="tab in tabs"
                    :key="tab.value"
                    type="button"
                    class="rounded-full px-4 py-2 text-xs font-semibold uppercase tracking-[0.15em] transition"
                    :class="activeTab === tab.value ? 'bg-black text-white' : 'border border-black/10 bg-white text-black/55 hover:bg-black/5'"
                    @click="$emit('change-tab', tab.value)"
                >
                    {{ tab.label }}
                </button>
            </div>
        </div>

        <div class="grid gap-6 sm:grid-cols-2 xl:grid-cols-4">
            <ProductCard
                v-for="product in products"
                :key="product.id"
                :product="product"
                cta-label="Comprar"
                @quick-view="$emit('quick-view', $event)"
            />
        </div>
    </section>
</template>

<script setup lang="ts">
import ProductCard from "./ProductCard.vue";
import type { ShopProduct } from "../../types/shopProduct";

interface StoreTab {
    label: string;
    value: string;
}

defineProps<{
    tabs: StoreTab[];
    activeTab: string;
    products: ShopProduct[];
}>();

defineEmits<{
    "change-tab": [value: string];
    "quick-view": [product: ShopProduct];
}>();
</script>
