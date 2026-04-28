<template>
    <div class="min-h-[70vh] bg-white text-black">
        <section class="mx-auto max-w-7xl px-6 py-10 md:px-10 md:py-14">
            <div class="mb-8 flex flex-wrap items-center gap-2 text-sm text-black/50">
                <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.HOME }" class="transition hover:text-black">Inicio</RouterLink>
                <span>/</span>
                <span class="text-black">Carrito</span>
            </div>

            <div class="mb-8 flex flex-wrap items-end justify-between gap-4 border-b border-black/10 pb-5">
                <div>
                    <p class="text-xs font-semibold uppercase tracking-[0.18em] text-black/45">Shopping cart</p>
                    <h1 class="mt-2 text-4xl font-semibold text-black md:text-5xl">Tu carrito</h1>
                </div>
                <button
                    v-if="cartItems.length > 0"
                    type="button"
                    class="rounded-full border border-black/10 px-4 py-2 text-sm font-semibold text-black transition hover:bg-black/5"
                    @click="clearCartAndRefresh"
                >
                    Vaciar carrito
                </button>
            </div>

            <div v-if="cartItems.length === 0" class="rounded-4xl border border-black/10 bg-[#f8f8f6] p-10 text-center">
                <p class="text-2xl font-semibold text-black">Tu carrito esta vacio</p>
                <p class="mt-3 text-sm text-black/60">Agrega productos desde el catalogo o desde el detalle para continuar con la compra.</p>
                <RouterLink
                    :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }"
                    class="mt-6 inline-flex rounded-full bg-black px-6 py-3 text-sm font-semibold text-white transition hover:bg-black/85"
                >
                    Ir al catalogo
                </RouterLink>
            </div>

            <div v-else class="grid gap-8 lg:grid-cols-[1.05fr_0.95fr]">
                <div class="space-y-4">
                    <article
                        v-for="item in cartItems"
                        :key="item.id"
                        class="flex flex-col gap-4 rounded-[1.75rem] border border-black/10 bg-white p-4 shadow-sm md:flex-row md:items-center"
                    >
                        <img
                            :src="item.image || fallbackImage"
                            :alt="item.name"
                            class="h-28 w-full rounded-[1.25rem] bg-[#f6f6f4] object-cover md:w-28"
                        >

                        <div class="flex-1">
                            <div class="flex flex-wrap items-start justify-between gap-4">
                                <div>
                                    <h2 class="text-lg font-semibold text-black">{{ item.name }}</h2>
                                    <p class="mt-1 text-xs uppercase tracking-[0.15em] text-black/45">{{ item.sku ?? item.id }}</p>
                                </div>
                                <p class="text-lg font-semibold text-black">{{ formatCurrency(item.price) }}</p>
                            </div>

                            <div class="mt-4 flex flex-wrap items-center gap-3">
                                <button
                                    type="button"
                                    class="rounded-full border border-black/10 px-3 py-1.5 text-sm font-semibold text-black transition hover:bg-black/5"
                                    @click="updateQuantity(item.id, item.quantity - 1)"
                                >
                                    -
                                </button>
                                <span class="min-w-8 text-center text-sm font-semibold">{{ item.quantity }}</span>
                                <button
                                    type="button"
                                    class="rounded-full border border-black/10 px-3 py-1.5 text-sm font-semibold text-black transition hover:bg-black/5"
                                    @click="updateQuantity(item.id, item.quantity + 1)"
                                >
                                    +
                                </button>
                                <button
                                    type="button"
                                    class="ml-auto text-sm font-semibold text-black/55 transition hover:text-black"
                                    @click="removeItem(item.id)"
                                >
                                    Eliminar
                                </button>
                            </div>
                        </div>
                    </article>
                </div>

                <aside class="h-fit rounded-4xl bg-black p-6 text-white md:p-7">
                    <p class="text-xs font-semibold uppercase tracking-[0.18em] text-white/50">Resumen</p>
                    <h2 class="mt-2 text-2xl font-semibold">Orden lista para continuar</h2>

                    <div class="mt-6 space-y-3 border-y border-white/10 py-5 text-sm">
                        <div class="flex items-center justify-between">
                            <span class="text-white/70">Productos</span>
                            <span>{{ itemsCount }}</span>
                        </div>
                        <div class="flex items-center justify-between">
                            <span class="text-white/70">Subtotal</span>
                            <span>{{ formatCurrency(cartTotal) }}</span>
                        </div>
                        <div class="flex items-center justify-between">
                            <span class="text-white/70">Envio</span>
                            <span>A convenir</span>
                        </div>
                    </div>

                    <div class="mt-5 flex items-center justify-between">
                        <span class="text-lg font-semibold">Total estimado</span>
                        <span class="text-2xl font-semibold">{{ formatCurrency(cartTotal) }}</span>
                    </div>

                    <div class="mt-6 space-y-4 border-t border-white/10 pt-5">
                        <div>
                            <label class="text-xs font-semibold uppercase tracking-[0.15em] text-white/55" for="customer-name">Nombre</label>
                            <input
                                id="customer-name"
                                v-model="checkoutForm.name"
                                type="text"
                                class="mt-2 w-full rounded-2xl border border-white/10 bg-white/8 px-4 py-3 text-sm text-white outline-none placeholder:text-white/35"
                                placeholder="Tu nombre"
                            >
                        </div>
                        <div>
                            <label class="text-xs font-semibold uppercase tracking-[0.15em] text-white/55" for="customer-phone">Telefono</label>
                            <input
                                id="customer-phone"
                                v-model="checkoutForm.phone"
                                type="text"
                                class="mt-2 w-full rounded-2xl border border-white/10 bg-white/8 px-4 py-3 text-sm text-white outline-none placeholder:text-white/35"
                                placeholder="Numero de contacto"
                            >
                        </div>
                        <div>
                            <label class="text-xs font-semibold uppercase tracking-[0.15em] text-white/55" for="customer-notes">Notas</label>
                            <textarea
                                id="customer-notes"
                                v-model="checkoutForm.notes"
                                rows="3"
                                class="mt-2 w-full rounded-2xl border border-white/10 bg-white/8 px-4 py-3 text-sm text-white outline-none placeholder:text-white/35"
                                placeholder="Color, ciudad o detalle adicional"
                            />
                        </div>
                    </div>

                    <button
                        type="button"
                        class="mt-6 w-full rounded-full bg-white px-5 py-3 text-sm font-semibold text-black transition hover:bg-white/85"
                        @click="checkoutByWhatsApp"
                    >
                        Finalizar por WhatsApp
                    </button>
                </aside>
            </div>
        </section>
    </div>
</template>

<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import { ROUTER_NAME } from "../routes/router-name";
import { clearCart, getCartItems, getCartTotal, removeItemFromCart, subscribeToCartUpdates, updateCartItemQuantity, type CartItem } from "../service/cartService";
import { formatCurrency } from "../service/service";
import { buildWhatsAppContactLink } from "../service/whatsappService";
import { toast } from "vue3-toastify";

const fallbackImage = "https://www.shutterstock.com/image-vector/missing-picture-page-website-design-600nw-1552421075.jpg";
const cartItems = ref<CartItem[]>([]);
const checkoutForm = ref({
    name: "",
    phone: "",
    notes: "",
});

const syncCart = () => {
    cartItems.value = getCartItems();
};

const cartTotal = computed(() => getCartTotal());
const itemsCount = computed(() => cartItems.value.reduce((total, item) => total + item.quantity, 0));

const updateQuantity = (itemId: string, quantity: number) => {
    updateCartItemQuantity(itemId, quantity);
    syncCart();
};

const removeItem = (itemId: string) => {
    removeItemFromCart(itemId);
    syncCart();
};

const clearCartAndRefresh = () => {
    clearCart();
    syncCart();
};

const checkoutByWhatsApp = () => {
    if (checkoutForm.value.name.trim().length === 0 || checkoutForm.value.phone.trim().length === 0) {
        toast.error("Completa nombre y telefono para continuar.");
        return;
    }

    const lines = [
        "Hola, quiero finalizar esta compra:",
        `Nombre: ${checkoutForm.value.name}`,
        `Telefono: ${checkoutForm.value.phone}`,
        ...cartItems.value.map((item) => `- ${item.name} x${item.quantity} (${formatCurrency(item.price)})`),
        `Total estimado: ${formatCurrency(cartTotal.value)}`,
        checkoutForm.value.notes.trim().length > 0 ? `Notas: ${checkoutForm.value.notes}` : "",
    ];

    window.open(
        buildWhatsAppContactLink(lines.filter((line) => line.length > 0).join("\n")),
        "_blank",
        "noopener,noreferrer",
    );
};

let unsubscribe: () => void = () => {};

onMounted(() => {
    syncCart();
    unsubscribe = subscribeToCartUpdates(syncCart);
});

onBeforeUnmount(() => {
    unsubscribe();
});
</script>