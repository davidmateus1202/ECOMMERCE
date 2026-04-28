<template>
    <section class="border-y border-black/8 bg-[#fafafa] py-14 md:py-18">
        <div class="mx-auto max-w-7xl px-6 md:px-10">
            <div class="mb-8 flex flex-wrap items-center justify-between gap-4">
                <div>
                    <p class="text-xs font-semibold uppercase tracking-[0.18em] text-black/50">Catalogo visual</p>
                    <h2 class="mt-2 text-3xl font-semibold text-black md:text-4xl">Recomendaciones por estilo</h2>
                </div>
                <p class="max-w-lg text-sm leading-7 text-black/60 md:text-base">
                    Selecciona una categoria para enfocar el grid de productos destacados.
                </p>
            </div>

            <div class="grid gap-5 md:grid-cols-3">
                <button
                    v-for="category in categories"
                    :key="category.name"
                    type="button"
                    class="group overflow-hidden rounded-3xl border border-black/10 bg-white text-left shadow-sm transition hover:-translate-y-1 hover:shadow-lg hover:shadow-black/8"
                    @click="$emit('select-category', category.name)"
                >
                    <img
                        :src="category.image"
                        :alt="category.name"
                        class="h-52 w-full object-cover transition duration-300 group-hover:scale-[1.03]"
                    >
                    <div class="p-5">
                        <p class="text-xs font-semibold uppercase tracking-[0.14em] text-black/50">{{ category.tagline }}</p>
                        <h3 class="mt-2 text-xl font-semibold text-black">{{ category.name }}</h3>
                        <p class="mt-2 text-sm leading-6 text-black/60">{{ category.description }}</p>
                    </div>
                </button>
            </div>
        </div>
    </section>
</template>

<script setup lang="ts">
interface CatalogCategory {
    name: string;
    description: string;
    tagline: string;
    image: string;
}

defineProps<{
    categories: CatalogCategory[];
}>();

defineEmits<{
    "select-category": [categoryName: string];
}>();
</script>
