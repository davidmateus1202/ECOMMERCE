<template>
    <section id="shop-category" class="mx-auto w-full max-w-7xl px-6 py-10 md:px-10 md:py-14">
        <div class="mb-8 flex flex-wrap items-end justify-between gap-4">
            <div>
                <p class="text-xs font-semibold uppercase tracking-[0.2em] text-black/45">Shop by category</p>
                <h2 class="mt-2 text-3xl font-semibold text-black md:text-4xl">Categorias visuales</h2>
            </div>
            <p class="max-w-lg text-sm leading-7 text-black/58">Una estructura mas cercana a marketplace: bloques de categoria amplios, limpios y faciles de leer.</p>
        </div>

        <div class="grid gap-4 md:grid-cols-3">
            <RouterLink
                v-for="category in categories"
                :key="category.name"
                :to="getCategoryRouteLocation(category.name)"
                class="group overflow-hidden rounded-4xl border border-black/8 bg-[#f8f8f6] text-left"
                @click="$emit('select-category', category.name)"
            >
                <img
                    :src="category.image"
                    :alt="category.name"
                    class="h-72 w-full object-cover transition duration-300 group-hover:scale-[1.03]"
                >
                <div class="flex items-center justify-between gap-4 px-5 py-4">
                    <div>
                        <p class="text-lg font-semibold text-black">{{ category.name }}</p>
                        <p class="mt-1 text-xs uppercase tracking-[0.15em] text-black/45">{{ category.tagline }}</p>
                    </div>
                    <span class="text-lg text-black/55">+</span>
                </div>
            </RouterLink>
        </div>
    </section>
</template>

<script setup lang="ts">
import { getCategoryRouteLocation } from "../../routes/category-navigation";

interface CategoryCard {
    name: string;
    tagline: string;
    image: string;
}

defineProps<{
    categories: CategoryCard[];
}>();

defineEmits<{
    "select-category": [categoryName: string];
}>();
</script>
