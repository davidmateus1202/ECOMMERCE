<template>
    <div
        v-if="product"
        class="fixed inset-0 z-50 flex items-center justify-center bg-black/45 px-4"
        @click.self="$emit('close')"
    >
        <div class="w-full max-w-3xl overflow-hidden rounded-3xl bg-white shadow-2xl shadow-black/20">
            <div class="grid md:grid-cols-[0.95fr_1.05fr]">
                <img
                    :src="product.imagen"
                    :alt="product.nombre"
                    class="h-full min-h-62.5 w-full object-cover"
                >
                <div class="p-6 md:p-8">
                    <div class="flex items-start justify-between gap-4">
                        <div>
                            <p class="text-xs font-semibold uppercase tracking-[0.14em] text-black/50">{{ product.categoria }}</p>
                            <h3 class="mt-2 text-2xl font-semibold text-black">{{ product.nombre }}</h3>
                        </div>
                        <button
                            type="button"
                            class="rounded-full border border-black/12 px-3 py-1 text-sm text-black/70"
                            @click="$emit('close')"
                        >
                            Cerrar
                        </button>
                    </div>

                    <p class="mt-4 text-sm leading-7 text-black/60">{{ product.descripcion }}</p>

                    <div class="mt-5 rounded-2xl bg-black/4 p-4">
                        <p class="text-xs uppercase tracking-[0.13em] text-black/50">Precio</p>
                        <p class="mt-2 text-3xl font-semibold text-black">{{ formattedPrice }}</p>
                        <p class="mt-1 text-xs text-black/45">Referencia {{ product.sku ?? `ID-${product.id}` }}</p>
                    </div>

                    <button
                        type="button"
                        class="mt-6 w-full rounded-full bg-black px-5 py-3 text-sm font-semibold text-white transition hover:bg-black/85"
                        @click="openWhatsAppForProduct(product)"
                    >
                        Comprar por WhatsApp
                    </button>

                    <RouterLink
                        :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT_PRODUCT_DETAILS, params: { id: product.id } }"
                        class="mt-3 block w-full rounded-full border border-black/12 px-5 py-3 text-center text-sm font-semibold text-black transition hover:bg-black/5"
                        @click="$emit('close')"
                    >
                        Ver detalle completo
                    </RouterLink>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { ROUTER_NAME } from "../../routes/router-name";
import { openWhatsAppForProduct } from "../../service/whatsappService";
import type { ShopProduct } from "../../types/shopProduct";

const props = defineProps<{
    product: ShopProduct | null;
}>();

defineEmits<{
    close: [];
}>();

const formattedPrice = computed(() => {
    if (!props.product) {
        return "";
    }

    return new Intl.NumberFormat("es-CO", {
        style: "currency",
        currency: "COP",
        maximumFractionDigits: 0,
    }).format(props.product.precio);
});
</script>
