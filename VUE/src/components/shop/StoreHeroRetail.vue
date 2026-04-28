<template>
    <section class="relative overflow-hidden border-b border-black/10 bg-[#f3f1ed]">
        <div class="absolute inset-0 bg-[radial-gradient(circle_at_18%_32%,rgba(255,255,255,0.85),transparent_30%),radial-gradient(circle_at_82%_20%,rgba(0,0,0,0.07),transparent_26%),linear-gradient(120deg,rgba(255,255,255,0.55),rgba(0,0,0,0.03))]" />
        <div class="relative mx-auto max-w-400 px-0 py-0">
            <div class="grid min-h-88 gap-0 lg:min-h-92 xl:grid-cols-[1fr_1fr] xl:min-h-96">
                <div class="relative flex items-center px-6 py-8 md:px-10 lg:px-14 xl:px-18 xl:py-8">
                    <div class="max-w-2xl">
                        <p class="text-xs font-semibold uppercase tracking-[0.24em] text-black/40">Atelier Market</p>
                        <h1 class="mt-4 text-4xl font-semibold leading-[0.93] text-black md:text-[3.6rem] xl:text-[4.2rem]">
                            Bolsos con presencia editorial y lectura comercial inmediata
                        </h1>
                        <p class="mt-4 max-w-xl text-sm leading-6 text-black/60 md:text-base">
                            Un banner mas amplio y refinado para presentar la coleccion principal con impacto visual, contraste limpio y accion directa hacia el catalogo.
                        </p>

                        <div class="mt-6 flex flex-wrap gap-3">
                            <a
                                href="#top-products"
                                class="rounded-full bg-black px-7 py-3 text-sm font-semibold text-white transition hover:bg-black/85"
                            >
                                Explorar ahora
                            </a>
                            <a
                                href="#shop-category"
                                class="rounded-full border border-black/12 bg-white/80 px-7 py-3 text-sm font-semibold text-black transition hover:bg-white"
                            >
                                Ver categorias
                            </a>
                        </div>

                        <div class="mt-6 grid max-w-2xl gap-3 sm:grid-cols-3">
                            <div class="rounded-3xl border border-black/8 bg-white/70 px-5 py-4 backdrop-blur-sm">
                                <p class="text-2xl font-semibold text-black">07</p>
                                <p class="mt-1 text-xs uppercase tracking-[0.16em] text-black/45">Capsulas</p>
                            </div>
                            <div class="rounded-3xl border border-black/8 bg-white/70 px-5 py-4 backdrop-blur-sm">
                                <p class="text-2xl font-semibold text-black">24h</p>
                                <p class="mt-1 text-xs uppercase tracking-[0.16em] text-black/45">Respuesta</p>
                            </div>
                            <div class="rounded-3xl border border-black/8 bg-white/70 px-5 py-4 backdrop-blur-sm">
                                <p class="text-2xl font-semibold text-black">100%</p>
                                <p class="mt-1 text-xs uppercase tracking-[0.16em] text-black/45">Curado</p>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="relative flex min-h-72 items-end justify-center overflow-hidden px-4 pt-5 md:min-h-78 md:px-8 xl:min-h-84 xl:px-0">
                    <div class="absolute inset-y-6 right-0 hidden w-[84%] rounded-l-[4rem] border border-black/6 bg-white/72 shadow-[0_28px_72px_rgba(0,0,0,0.08)] xl:block" />
                    <div class="absolute left-[8%] top-8 hidden h-24 w-24 rounded-full border border-black/8 bg-white/75 backdrop-blur-sm xl:block" />
                    <div class="absolute bottom-8 left-[14%] hidden h-12 w-12 rounded-full bg-black xl:block" />
                    <div class="absolute right-[12%] top-12 hidden h-16 w-16 rounded-full border border-black/8 bg-white/70 xl:block" />

                    <img
                        :src="heroImage"
                        alt="Bolso principal de la coleccion"
                        class="relative z-10 -mb-3 w-full max-w-145 object-contain drop-shadow-[0_30px_52px_rgba(0,0,0,0.2)] md:max-w-160 xl:max-w-170 xl:translate-x-3 xl:scale-[1.03]"
                    >

                    <div class="absolute bottom-5 left-4 z-20 rounded-full bg-black px-4 py-2 text-xs font-semibold uppercase tracking-[0.18em] text-white md:left-8 xl:left-10">
                        Nuevo drop
                    </div>
                    <div class="absolute right-4 top-5 z-20 rounded-3xl border border-black/8 bg-white/85 px-4 py-3 backdrop-blur-sm md:right-8 xl:right-10 xl:px-5 xl:py-4">
                        <p class="text-[11px] font-semibold uppercase tracking-[0.18em] text-black/40">Signature line</p>
                        <p class="mt-2 text-lg font-semibold text-black">Pieza protagonista</p>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script setup lang="ts">
const heroImage = new URL("../../assets/photos/bolso4.png", import.meta.url).href;
</script>
