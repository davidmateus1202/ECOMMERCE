<template>
    <section :id="sectionId" class="mx-auto w-full max-w-7xl px-6 py-12 md:px-10 md:py-16">
        <div class="mb-8 flex flex-wrap items-end justify-between gap-4">
            <div>
                <p class="text-xs font-semibold uppercase tracking-[0.18em] text-black/50">{{ eyebrow }}</p>
                <h2 class="mt-2 text-3xl font-semibold text-black md:text-4xl">{{ title }}</h2>
                <p v-if="description" class="mt-2 max-w-2xl text-sm leading-7 text-black/60 md:text-base">{{ description }}</p>
            </div>
            <p class="rounded-full border border-black/10 px-4 py-2 text-sm font-medium text-black/65">
                {{ products.length }} productos
            </p>
        </div>

        <div class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
            <ProductCard
                v-for="product in products"
                :key="product.id"
                :product="product"
                :cta-label="ctaLabel"
                @quick-view="$emit('quick-view', $event)"
            />
        </div>
    </section>
</template>

<script setup lang="ts">
import type { ShopProduct } from "../../types/shopProduct";
import ProductCard from "./ProductCard.vue";

withDefaults(
    defineProps<{
        sectionId?: string;
        eyebrow?: string;
        title: string;
        description?: string;
        products: ShopProduct[];
        ctaLabel?: string;
    }>(),
    {
        sectionId: undefined,
        eyebrow: "Tienda",
        description: "",
        ctaLabel: "Ordenar por WhatsApp",
    },
);

defineEmits<{
    "quick-view": [product: ShopProduct];
}>();
</script>
