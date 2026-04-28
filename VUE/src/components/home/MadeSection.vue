<template>
  <section class="bg-[#f7f7f8] text-black py-20 px-6 sm:px-10">
    <div class="mx-auto max-w-7xl">
      <div class="grid gap-10 lg:grid-cols-[1.1fr_1.1fr_0.8fr] items-center">
        <div class="order-2 lg:order-1">
          <span class="inline-flex items-center rounded-full bg-black/5 px-4 py-1 text-xs tracking-widest uppercase text-black/70">
            Coleccion
          </span>
          <h2 class="mt-5 text-4xl md:text-5xl font-semibold leading-tight">
            Bolsos de cuero
            <span class="block text-black/60 italic">comodisimos y confiables</span>
          </h2>
          <p class="mt-5 text-sm md:text-base text-black/60 leading-relaxed">
            Estan hechos en cuero autentico y pensados para acompanarte todo el dia
            con ligereza, soporte y estilo.
          </p>
          <p class="mt-4 text-sm md:text-base text-black/60 leading-relaxed">
            Suaves al tacto, resistentes al uso y con detalles minimalistas.
          </p>

          <div class="mt-7 flex flex-wrap items-center gap-4">
            <RouterLink
              :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT, query: { category: 'Bolsos' } }"
              class="rounded-full bg-black text-white px-6 py-3 text-sm shadow hover:bg-black/90 transition-colors"
            >
              Ver coleccion
            </RouterLink>
            <RouterLink
              :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT }"
              class="rounded-full border border-black/20 px-6 py-3 text-sm text-black/70 hover:bg-black/5 transition-colors"
            >
              Conocer detalles
            </RouterLink>
          </div>
        </div>

        <div class="order-1 lg:order-2 relative flex justify-center">
          <div class="absolute -inset-4 sm:-inset-8 rounded-full bg-black/5" />
          <img
            :src="mainImage"
            alt="Bolso de cuero"
            class="relative w-full max-w-md object-contain drop-shadow-[0_40px_60px_rgba(0,0,0,0.25)] rounded-3xl"
          />
        </div>

        <div class="order-3 space-y-6">
          <div class="rounded-2xl border border-black/10 bg-white px-5 py-4 shadow-sm">
            <p class="text-sm font-semibold">Cuero premium</p>
            <p class="text-xs text-black/60">Grano fino, textura elegante</p>
          </div>
          <div class="rounded-2xl border border-black/10 bg-white px-5 py-4 shadow-sm">
            <p class="text-sm font-semibold">Comodidad real</p>
            <p class="text-xs text-black/60">Correas suaves y soporte firme</p>
          </div>
          <div class="rounded-2xl border border-black/10 bg-white px-5 py-4 shadow-sm">
            <p class="text-sm font-semibold">Diseno limpio</p>
            <p class="text-xs text-black/60">Minimalista y facil de combinar</p>
          </div>
        </div>
      </div>

      <div class="mt-12 grid grid-cols-3 gap-6 text-center border-t border-black/10 pt-6">
        <div>
          <p class="text-2xl font-semibold">100%</p>
          <p class="text-xs text-black/60 tracking-widest uppercase">Cuero real</p>
        </div>
        <div>
          <p class="text-2xl font-semibold">25+</p>
          <p class="text-xs text-black/60 tracking-widest uppercase">Modelos</p>
        </div>
        <div>
          <p class="text-2xl font-semibold">Top</p>
          <p class="text-xs text-black/60 tracking-widest uppercase">Confort</p>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ROUTER_NAME } from '../../routes/router-name';

const mainImage = new URL("../../assets/photos/bolso2.png", import.meta.url).href;
</script>
