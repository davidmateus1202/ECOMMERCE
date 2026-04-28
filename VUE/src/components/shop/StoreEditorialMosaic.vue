<template>
    <section class="mx-auto w-full max-w-7xl px-6 py-12 md:px-10 md:py-16">
        <div class="grid gap-4 lg:grid-cols-[1.05fr_0.95fr]">
            <article class="relative overflow-hidden rounded-[2rem] bg-black text-white">
                <img :src="leftFeature.image" :alt="leftFeature.title" class="h-full min-h-96 w-full object-cover opacity-80">
                <div class="absolute inset-0 bg-gradient-to-tr from-black via-black/35 to-transparent" />
                <div class="absolute inset-x-0 bottom-0 p-7 md:p-8">
                    <p class="text-xs font-semibold uppercase tracking-[0.2em] text-white/55">{{ leftFeature.tag }}</p>
                    <h2 class="mt-2 max-w-sm text-3xl font-semibold leading-tight md:text-4xl">{{ leftFeature.title }}</h2>
                    <p class="mt-3 max-w-sm text-sm leading-6 text-white/72">{{ leftFeature.description }}</p>
                </div>
            </article>

            <div class="grid gap-4 sm:grid-cols-2">
                <article
                    v-for="card in mosaicCards"
                    :key="card.title"
                    class="group relative overflow-hidden rounded-[2rem] border border-black/8 bg-[#f7f7f5]"
                >
                    <img
                        :src="card.image"
                        :alt="card.title"
                        class="h-60 w-full object-cover transition duration-300 group-hover:scale-[1.03]"
                    >
                    <div class="absolute inset-0 bg-gradient-to-t from-black/80 to-transparent" />
                    <div class="absolute inset-x-0 bottom-0 p-5 text-white">
                        <p class="text-[11px] font-semibold uppercase tracking-[0.18em] text-white/60">{{ card.tag }}</p>
                        <h3 class="mt-2 text-xl font-semibold">{{ card.title }}</h3>
                        <p class="mt-2 text-sm text-white/75">{{ card.description }}</p>
                    </div>
                </article>
            </div>
        </div>
    </section>
</template>

<script setup lang="ts">
interface MosaicCard {
    title: string;
    description: string;
    tag: string;
    image: string;
}

defineProps<{
    leftFeature: MosaicCard;
    mosaicCards: MosaicCard[];
}>();
</script>
