<template>
    <div class="bg-white text-black">
        <section class="border-b border-black/10 bg-[#f6f6f4]">
            <div class="mx-auto max-w-7xl px-6 py-10 md:px-10 md:py-14">
                <div class="mb-8 flex flex-wrap items-center gap-2 text-sm text-black/50">
                    <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.HOME }" class="transition hover:text-black">Inicio</RouterLink>
                    <span>/</span>
                    <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }" class="transition hover:text-black">Catalogo</RouterLink>
                    <span>/</span>
                    <span class="text-black">{{ product?.nombre ?? 'Detalle' }}</span>
                </div>

                <div v-if="product" class="grid gap-10 lg:grid-cols-[1fr_0.95fr] lg:items-start">
                    <div class="grid gap-4 lg:sticky lg:top-8">
                        <div class="overflow-hidden rounded-[2.2rem] border border-black/8 bg-white shadow-sm shadow-black/5">
                            <div class="flex items-center justify-between border-b border-black/8 px-5 py-4 text-[11px] font-semibold uppercase tracking-[0.18em] text-black/45">
                                <span>{{ product.categoria }}</span>
                                <span>{{ product.sku ?? `ID-${product.id}` }}</span>
                            </div>
                            <img
                                :src="product.imagen"
                                :alt="product.nombre"
                                class="aspect-4/5 w-full object-cover"
                            >
                        </div>

                        <div class="grid gap-4 sm:grid-cols-3">
                            <div class="rounded-3xl border border-black/10 bg-white p-4">
                                <p class="text-[11px] uppercase tracking-[0.16em] text-black/45">Envio</p>
                                <p class="mt-2 text-sm font-semibold text-black">Coordinado por chat</p>
                            </div>
                            <div class="rounded-3xl border border-black/10 bg-white p-4">
                                <p class="text-[11px] uppercase tracking-[0.16em] text-black/45">Respuesta</p>
                                <p class="mt-2 text-sm font-semibold text-black">Rapida por WhatsApp</p>
                            </div>
                            <div class="rounded-3xl border border-black/10 bg-white p-4">
                                <p class="text-[11px] uppercase tracking-[0.16em] text-black/45">Estado</p>
                                <p class="mt-2 text-sm font-semibold text-black">Disponible</p>
                            </div>
                        </div>
                    </div>

                    <div>
                        <p class="text-xs font-semibold uppercase tracking-[0.2em] text-black/45">Featured product</p>
                        <h1 class="mt-3 max-w-xl text-4xl font-semibold leading-[1.02] text-black md:text-6xl">{{ product.nombre }}</h1>
                        <p class="mt-5 max-w-xl text-base leading-8 text-black/62">{{ product.descripcion }}</p>

                        <div class="mt-8 flex items-end gap-4 border-b border-black/10 pb-6">
                            <div>
                                <p class="text-xs uppercase tracking-[0.15em] text-black/40">Precio actual</p>
                                <p class="mt-2 text-4xl font-semibold text-black">{{ formattedPrice }}</p>
                            </div>
                            <p class="pb-1 text-sm text-black/35 line-through">{{ previousPrice }}</p>
                        </div>

                        <div class="mt-8 grid gap-4 sm:grid-cols-3">
                            <div class="rounded-3xl border border-black/10 bg-white p-4">
                                <p class="text-xs uppercase tracking-[0.14em] text-black/50">Referencia</p>
                                <p class="mt-2 text-lg font-semibold text-black">{{ product.sku ?? `ID-${product.id}` }}</p>
                            </div>
                            <div class="rounded-3xl border border-black/10 bg-white p-4">
                                <p class="text-xs uppercase tracking-[0.14em] text-black/50">Categoria</p>
                                <p class="mt-2 text-lg font-semibold text-black">{{ product.categoria }}</p>
                            </div>
                            <div class="rounded-3xl border border-black/10 bg-white p-4">
                                <p class="text-xs uppercase tracking-[0.14em] text-black/50">Canal</p>
                                <p class="mt-2 text-lg font-semibold text-black">Compra guiada</p>
                            </div>
                        </div>

                        <div class="mt-8 rounded-4xl bg-black p-6 text-white md:p-7">
                            <p class="text-xs font-semibold uppercase tracking-[0.2em] text-white/55">Compra facil</p>
                            <h2 class="mt-2 text-2xl font-semibold">Ordena por WhatsApp con la referencia lista</h2>
                            <p class="mt-3 max-w-xl text-sm leading-7 text-white/72">
                                El mensaje se genera automaticamente con nombre, precio y referencia para acelerar disponibilidad, colores y entrega.
                            </p>

                            <div class="mt-6 flex flex-wrap gap-3">
                                <button
                                    type="button"
                                    class="rounded-full bg-white px-6 py-3 text-sm font-semibold text-black transition hover:bg-white/85"
                                    @click="openWhatsAppForProduct(product)"
                                >
                                    Comprar por WhatsApp
                                </button>
                                <RouterLink
                                    :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }"
                                    class="rounded-full border border-white/15 px-6 py-3 text-sm font-semibold text-white transition hover:bg-white/8"
                                >
                                    Volver al catalogo
                                </RouterLink>
                            </div>

                            <p class="mt-4 text-xs uppercase tracking-[0.16em] text-white/45">WhatsApp {{ whatsappNumberLabel }}</p>
                        </div>

                        <div class="mt-10 grid gap-6 border-t border-black/10 pt-8 md:grid-cols-2">
                            <div>
                                <p class="text-sm font-semibold uppercase tracking-[0.16em] text-black/45">Descripcion comercial</p>
                                <p class="mt-3 text-sm leading-7 text-black/60">
                                    Este producto hace parte de una presentacion mas editorial, pensada para que la imagen tenga peso comercial y la accion de compra sea directa.
                                </p>
                            </div>
                            <div>
                                <p class="text-sm font-semibold uppercase tracking-[0.16em] text-black/45">Ideal para</p>
                                <ul class="mt-3 space-y-2 text-sm leading-7 text-black/60">
                                    <li>Catalogos visuales premium</li>
                                    <li>Ventas por conversacion en WhatsApp</li>
                                    <li>Exhibicion limpia de referencias</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

                <div v-else class="rounded-4xl border border-black/10 bg-white p-10 text-center shadow-sm">
                    <p class="text-2xl font-semibold text-black">Producto no encontrado</p>
                    <p class="mt-3 text-sm text-black/60">La referencia solicitada no existe dentro del catalogo local.</p>
                    <RouterLink
                        :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }"
                        class="mt-6 inline-flex rounded-full bg-black px-6 py-3 text-sm font-semibold text-white transition hover:bg-black/85"
                    >
                        Ir al catalogo
                    </RouterLink>
                </div>
            </div>
        </section>

        <section v-if="relatedProducts.length > 0" class="border-t border-black/8 bg-white">
            <div class="mx-auto max-w-7xl px-6 py-12 md:px-10 md:py-16">
                <div class="mb-8 flex items-end justify-between gap-4 border-b border-black/10 pb-5">
                    <div>
                        <p class="text-xs font-semibold uppercase tracking-[0.18em] text-black/50">Recomendaciones</p>
                        <h2 class="mt-2 text-3xl font-semibold text-black">Tambien te puede interesar</h2>
                    </div>
                    <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }" class="text-sm font-semibold text-black/70 transition hover:text-black">
                        Ver todo
                    </RouterLink>
                </div>

                <div class="grid gap-6 sm:grid-cols-2 xl:grid-cols-3">
                    <ProductCard
                        v-for="relatedProduct in relatedProducts"
                        :key="relatedProduct.id"
                        :product="relatedProduct"
                        cta-label="Comprar"
                        @quick-view="goToProduct"
                    />
                </div>
            </div>
        </section>
    </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import ProductCard from "../components/shop/ProductCard.vue";
import { fetchRelatedStorefrontProducts, fetchStorefrontProductById } from "../api/productApi";
import { ROUTER_NAME } from "../routes/router-name";
import { openWhatsAppForProduct, WHATSAPP_PHONE_NUMBER } from "../service/whatsappService";
import type { ShopProduct } from "../types/shopProduct";

const route = useRoute();
const router = useRouter();
const product = ref<ShopProduct | null>(null);
const relatedProducts = ref<ShopProduct[]>([]);

const productId = computed(() => {
    const rawParam = Array.isArray(route.params.id) ? route.params.id[0] : route.params.id;
    const parsedValue = Number(rawParam);
    return Number.isFinite(parsedValue) ? parsedValue : null;
});

const loadProduct = async (id: number | null) => {
    if (id === null) {
        product.value = null;
        relatedProducts.value = [];
        return;
    }

    try {
        product.value = await fetchStorefrontProductById(id);
        relatedProducts.value = await fetchRelatedStorefrontProducts(id);
    } catch {
        product.value = null;
        relatedProducts.value = [];
    }
};

watch(productId, (id) => {
    loadProduct(id);
}, { immediate: true });

const formattedPrice = computed(() => {
    if (!product.value) {
        return "";
    }

    return new Intl.NumberFormat("es-CO", {
        style: "currency",
        currency: "COP",
        maximumFractionDigits: 0,
    }).format(product.value.precio);
});

const previousPrice = computed(() => {
    if (!product.value) {
        return "";
    }

    return new Intl.NumberFormat("es-CO", {
        style: "currency",
        currency: "COP",
        maximumFractionDigits: 0,
    }).format(Math.round(product.value.precio * 1.18));
});

const whatsappNumberLabel = computed(() => {
    const number = WHATSAPP_PHONE_NUMBER;
    return `+${number.slice(0, 2)} ${number.slice(2, 5)} ${number.slice(5, 8)} ${number.slice(8)}`;
});

const goToProduct = (selectedProduct: ShopProduct) => {
    router.push({
        name: ROUTER_NAME.CUSTOMER.STOREFRONT_PRODUCT_DETAILS,
        params: { id: selectedProduct.id },
    });
};
</script>