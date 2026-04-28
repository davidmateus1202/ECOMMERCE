<template>
  <div class="flex flex-col h-screen bg-white">
    <!-- Top bar -->
    <div
      class="flex items-center justify-between px-4 md:px-8 border-b border-gray-300 py-3 bg-white transition-all duration-300">
      <a href="https://prebuiltui.com">
        <img class="h-12" :src="Logo" alt="dummyLogoColored" />
      </a>

      <div class="flex items-center px-5 bg-gray-100 w-2/3 h-10 rounded-3xl shadow">
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
          class="icon icon-tabler icons-tabler-outline icon-tabler-search">
          <path stroke="none" d="M0 0h24v24H0z" fill="none" />
          <path d="M3 10a7 7 0 1 0 14 0a7 7 0 1 0 -14 0" />
          <path d="M21 21l-6 -6" />
        </svg>
        <input type="text" placeholder="Buscar productos, marcas y más..." class="flex-1 bg-transparent outline-none text-sm" />
      </div>

      <div class="flex items-center gap-5 text-gray-500">
        <p class="hidden md:block">¡Hola! Admin</p>
        <button @click="logout" class="hidden md:block border rounded-full text-sm px-4 py-1 hover:bg-gray-100 cursor-pointer">Log out</button>
      </div>
    </div>

    <!-- Main Content with Sidebar -->
    <div class="flex flex-1 overflow-hidden">
      <!-- Sidebar -->
      <div class="md:w-64 w-20 border-r text-base border-gray-300 pt-4 flex flex-col bg-white overflow-y-auto">
        <RouterLink v-for="(item, index) in sidebarLinks" :key="index" :to="item.to" :class="[
          'flex items-center justify-center md:justify-start py-4 px-4 gap-3 transition-colors',
          item.activeRouteNames.includes(String(route.name ?? ''))
            ? 'border-r-4 md:border-r-[6px] bg-black/7 border-black text-black'
            : 'hover:bg-gray-100/90 border-white text-gray-700'
        ]">
          <component :is="item.icon" class="w-7 h-7 md:w-6 md:h-6 shrink-0" />
          <p class="md:block hidden">{{ item.name }}</p>
        </RouterLink>
      </div>

      <!-- Main Content Area -->
      <div ref="contentScrollContainer" class="flex-1 overflow-y-auto bg-white">
        <RouterView />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { IconLayoutDashboard, IconPlus, IconSettings2, IconShoppingBag } from "@tabler/icons-vue";
import type { Component } from "vue";
import { nextTick, ref, watch } from "vue";
import Logo from "../assets/photos/Logo.png";
import { clearStoredAuthToken } from "../service/authService";
import { useRoute, useRouter } from "vue-router";
import { ROUTER_NAME } from "../routes/router-name";

const router = useRouter();
const route = useRoute();
const contentScrollContainer = ref<HTMLElement | null>(null);

type SidebarLink = {
  name: string;
  to: { name: string };
  activeRouteNames: string[];
  icon: Component;
};

const sidebarLinks: SidebarLink[] = [
  {
    name: "Productos",
    to: { name: ROUTER_NAME.ADMIN.PRODUCTS },
    activeRouteNames: [ROUTER_NAME.ADMIN.PRODUCTS, ROUTER_NAME.ADMIN.EDIT_PRODUCT],
    icon: IconLayoutDashboard,
  },
  {
    name: "Nuevo producto",
    to: { name: ROUTER_NAME.ADMIN.CREATE_PRODUCT },
    activeRouteNames: [ROUTER_NAME.ADMIN.CREATE_PRODUCT],
    icon: IconPlus,
  },
  {
    name: "Seccion Accesorios",
    to: { name: ROUTER_NAME.ADMIN.ACCESSORIES_SECTION },
    activeRouteNames: [ROUTER_NAME.ADMIN.ACCESSORIES_SECTION],
    icon: IconSettings2,
  },
  {
    name: "Seccion Bolsos",
    to: { name: ROUTER_NAME.ADMIN.BAGS_SECTION },
    activeRouteNames: [ROUTER_NAME.ADMIN.BAGS_SECTION],
    icon: IconShoppingBag,
  },
];

const logout = () => {
  clearStoredAuthToken();
  router.push({ name: ROUTER_NAME.ADMIN.LOGIN })
}

watch(
  () => route.fullPath,
  async () => {
    await nextTick();
    contentScrollContainer.value?.scrollTo({ top: 0, left: 0, behavior: "auto" });
  },
  { immediate: true }
);
</script>
