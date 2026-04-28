<template>
  <nav
    class="sticky top-0 z-40 flex items-center justify-between border-b border-black/10 bg-white/95 px-6 py-4 backdrop-blur-md transition-all md:px-16 lg:px-24 xl:px-32"
  >
    <div class="hidden lg:flex items-center gap-4 text-[11px] font-semibold uppercase tracking-[0.18em] text-black/35">
      <span>Retail catalog</span>
      <span class="h-1 w-1 rounded-full bg-black/20"></span>
      <span>WhatsApp orders</span>
    </div>

    <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.HOME }" class="shrink-0" @click="closeMenu">
      <img
        class="h-16 w-auto object-contain md:h-18"
        :src="Logo"
        alt="Logo"
      />
    </RouterLink>

    <!-- Mobile menu button -->
    <button
      aria-label="Menu"
      :aria-expanded="isOpen"
      class="rounded-full border border-black/10 p-2 sm:hidden"
      @click="toggleMenu"
    >
      <svg width="21" height="15" viewBox="0 0 21 15" fill="none">
        <rect width="21" height="1.5" rx=".75" fill="#111111" />
        <rect x="8" y="6" width="13" height="1.5" rx=".75" fill="#111111" />
        <rect x="6" y="13" width="15" height="1.5" rx=".75" fill="#111111" />
      </svg>
    </button>

    <div
      v-show="isOpen"
      class="fixed inset-0 z-30 flex flex-col items-center justify-center gap-5 bg-white/96 px-5 text-sm backdrop-blur-md sm:hidden"
    >
      <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.HOME }" class="block text-lg font-semibold" @click="closeMenu">Inicio</RouterLink>
      <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }" class="block text-lg font-semibold" @click="closeMenu">Catalogo</RouterLink>
      <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.ACCESSORIES }" class="block text-black/70" @click="closeMenu">Accesorios</RouterLink>
      <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.BAGS }" class="block text-black/70" @click="closeMenu">Bolsos</RouterLink>

      <button
        class="mt-3 cursor-pointer rounded-full bg-black px-6 py-2 text-sm text-white transition hover:bg-black/70"
        type="button"
        @click="goToLogin"
      >
        Login
      </button>
    </div>

    <div class="hidden sm:flex items-center gap-3 lg:gap-4">
      <RouterLink
        :to="{ name: ROUTER_NAME.CUSTOMER.HOME }"
        class="rounded-full px-4 py-2 text-sm font-semibold transition"
        :class="isHomeRoute ? 'bg-black text-white' : 'text-black/65 hover:bg-black/5 hover:text-black'"
      >
        Inicio
      </RouterLink>
      <RouterLink
        :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }"
        class="rounded-full px-4 py-2 text-sm font-semibold transition"
        :class="isStorefrontRoute ? 'bg-black text-white' : 'text-black/65 hover:bg-black/5 hover:text-black'"
      >
        Catalogo
      </RouterLink>
      <RouterLink
        :to="{ name: ROUTER_NAME.CUSTOMER.ACCESSORIES }"
        class="rounded-full px-4 py-2 text-sm font-semibold transition"
        :class="isAccessoriesRoute ? 'bg-black text-white' : 'text-black/65 hover:bg-black/5 hover:text-black'"
        @click="closeMenu"
      >
        Accesorios
      </RouterLink>
      <RouterLink
        :to="{ name: ROUTER_NAME.CUSTOMER.BAGS }"
        class="rounded-full px-4 py-2 text-sm font-semibold transition"
        :class="isBagsRoute ? 'bg-black text-white' : 'text-black/65 hover:bg-black/5 hover:text-black'"
        @click="closeMenu"
      >
        Bolsos
      </RouterLink>

      <form
        class="hidden lg:flex items-center gap-2 rounded-full border border-black/10 px-3"
        @submit.prevent="submitCatalogSearch"
      >
        <input
          v-model="searchQuery"
          class="w-full bg-transparent py-1.5 text-sm outline-none placeholder:text-black/35"
          type="text"
          placeholder="Buscar catalogo"
        />
        <button type="submit" class="cursor-pointer" aria-label="Buscar en catalogo">
        <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
          <path
            d="M10.836 10.615 15 14.695"
            stroke="#111111"
            stroke-width="1.2"
            stroke-linecap="round"
          />
          <path
            d="M9.141 11.738c2.729-1.136 4.001-4.224
               2.841-6.898S7.67.921 4.942 2.057
               C2.211 3.193.94 6.281 2.1 8.955
               s4.312 3.92 7.041 2.783"
            stroke="#111111"
            stroke-width="1.2"
            stroke-linecap="round"
          />
        </svg>
        </button>
      </form>

      <RouterLink :to="{ name: ROUTER_NAME.CUSTOMER.CART }" class="relative cursor-pointer rounded-full border border-black/10 p-2.5 transition hover:bg-black/5">
        <svg width="18" height="18" viewBox="0 0 14 14" fill="none">
          <path
            d="M.583.583h2.333l1.564 7.81a1.17
               1.17 0 0 0 1.166.94h5.67a1.17
               1.17 0 0 0 1.167-.94l.933-4.893H3.5"
            stroke="#111111"
            stroke-linecap="round"
            stroke-linejoin="round"
          />
        </svg>
        <span
          class="absolute -right-1 -top-1 flex h-4.5 w-4.5 items-center justify-center rounded-full bg-black text-xs text-white"
        >
          {{ cartCount }}
        </span>
      </RouterLink>

      <button
        class="cursor-pointer rounded-full bg-black px-8 py-2 text-white transition hover:bg-black/70"
        type="button"
        @click="goToLogin"
      >
        Login
      </button>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { ROUTER_NAME } from '../../routes/router-name'
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getCartItemsCount, subscribeToCartUpdates } from '../../service/cartService';

// images
import Logo from '../../assets/photos/Logo.png'

const router = useRouter();
const route = useRoute();
const searchQuery = ref('');
const cartCount = ref(0);
const isOpen = defineModel<boolean>({
  default: false,
  required: true,
})

const toggleMenu = () => {
  isOpen.value = !isOpen.value
}

const closeMenu = () => {
  isOpen.value = false
}

const goToLogin = () => {
  closeMenu()
  router.push({ name: ROUTER_NAME.ADMIN.LOGIN })
}

const submitCatalogSearch = () => {
  const normalizedQuery = searchQuery.value.trim()

  router.push({
    name: ROUTER_NAME.CUSTOMER.STOREFRONT,
    query: normalizedQuery.length > 0 ? { q: normalizedQuery } : {},
  })
}

const isStorefrontRoute = computed(() => {
  return route.name === ROUTER_NAME.CUSTOMER.STOREFRONT || route.name === ROUTER_NAME.CUSTOMER.STOREFRONT_PRODUCT_DETAILS
})

const isAccessoriesRoute = computed(() => route.name === ROUTER_NAME.CUSTOMER.ACCESSORIES)

const isBagsRoute = computed(() => route.name === ROUTER_NAME.CUSTOMER.BAGS)

const isHomeRoute = computed(() => route.name === ROUTER_NAME.CUSTOMER.HOME)

const syncCartCount = () => {
  cartCount.value = getCartItemsCount()
}

let unsubscribeCart: () => void = () => {}

onMounted(() => {
  syncCartCount()
  unsubscribeCart = subscribeToCartUpdates(syncCartCount)
})

onBeforeUnmount(() => {
  unsubscribeCart()
})

watch(
  () => route.fullPath,
  () => {
    closeMenu()

    const currentQuery = Array.isArray(route.query.q) ? route.query.q[0] : route.query.q
    searchQuery.value = currentQuery ?? ''
  }
)
</script>
