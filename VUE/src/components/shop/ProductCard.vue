<template>
    <article class="group flex h-full flex-col overflow-hidden rounded-[1.75rem] border border-black/8 bg-white p-3 transition hover:-translate-y-1 hover:shadow-[0_18px_40px_rgba(0,0,0,0.08)]">
        <RouterLink
            :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT_PRODUCT_DETAILS, params: { id: product.id } }"
            class="relative overflow-hidden rounded-[1.4rem] bg-[#f6f6f4]"
        >
            <img
                :src="product.imagen"
                :alt="product.nombre"
                class="aspect-4/5 w-full object-cover transition duration-300 group-hover:scale-[1.03]"
            >
            <p class="absolute left-3 top-3 rounded-full bg-black px-3 py-1 text-[10px] font-semibold uppercase tracking-[0.16em] text-white">
                Sale
            </p>
        </RouterLink>

        <div class="flex flex-1 flex-col px-1 pb-2 pt-4">
            <p class="text-[11px] font-semibold uppercase tracking-[0.16em] text-black/40">{{ product.categoria }}</p>
            <RouterLink
                :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT_PRODUCT_DETAILS, params: { id: product.id } }"
                class="mt-2 text-lg font-semibold text-black transition hover:text-black/70"
            >
                {{ product.nombre }}
            </RouterLink>
            <p class="mt-2 line-clamp-2 min-h-12 text-sm leading-6 text-black/55">{{ product.descripcion }}</p>

            <div class="mt-4 flex items-end justify-between gap-3">
                <div>
                    <p class="text-xs text-black/30 line-through">{{ crossedPrice }}</p>
                    <p class="mt-1 text-xl font-semibold text-black">{{ formattedPrice }}</p>
                </div>
                <p class="text-[11px] uppercase tracking-[0.12em] text-black/40">{{ product.sku ?? `ID-${product.id}` }}</p>
            </div>

            <div class="mt-5 grid gap-2 sm:grid-cols-2">
                <button
                    type="button"
                    class="rounded-full border border-black/10 px-4 py-2.5 text-sm font-semibold text-black transition hover:bg-black/5"
                    @click="$emit('quick-view', product)"
                >
                    Ver mas
                </button>
                <button
                    type="button"
                    class="rounded-full bg-black px-4 py-2.5 text-sm font-semibold text-white transition hover:bg-black/85"
                    @click="openWhatsAppForProduct(product)"
                >
                    {{ ctaLabel }}
                </button>
            </div>
        </div>
    </article>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { ROUTER_NAME } from "../../routes/router-name";
import type { ShopProduct } from "../../types/shopProduct";
import { openWhatsAppForProduct } from "../../service/whatsappService";

const props = withDefaults(
    defineProps<{
        product: ShopProduct;
        ctaLabel?: string;
    }>(),
    {
        ctaLabel: "Ordenar por WhatsApp",
    },
);

defineEmits<{
    "quick-view": [product: ShopProduct];
}>();

const formattedPrice = computed(() => {
    return new Intl.NumberFormat("es-CO", {
        style: "currency",
        currency: "COP",
        maximumFractionDigits: 0,
    }).format(props.product.precio);
});

const crossedPrice = computed(() => {
    return new Intl.NumberFormat("es-CO", {
        style: "currency",
        currency: "COP",
        maximumFractionDigits: 0,
    }).format(Math.round(props.product.precio * 1.18));
});
</script>
