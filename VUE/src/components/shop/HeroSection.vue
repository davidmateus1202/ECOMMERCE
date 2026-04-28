<template>
    <section class="relative overflow-hidden border-b border-black/10 bg-white">
        <div class="absolute inset-0 bg-[radial-gradient(circle_at_15%_25%,rgba(0,0,0,0.06),transparent_34%),radial-gradient(circle_at_85%_15%,rgba(0,0,0,0.08),transparent_28%)]" />
        <div class="relative mx-auto grid max-w-7xl gap-8 px-6 pb-16 pt-12 md:px-10 lg:grid-cols-[1.1fr_0.9fr] lg:items-center lg:pb-24 lg:pt-18">
            <div>
                <p class="inline-flex items-center rounded-full border border-black/15 bg-white px-4 py-1 text-xs font-semibold uppercase tracking-[0.18em] text-black/70">
                    Nueva coleccion 2026
                </p>
                <h1 class="mt-5 max-w-xl text-4xl font-semibold leading-tight text-black md:text-5xl">
                    Elegancia diaria para bolsos y accesorios de catalogo
                </h1>
                <p class="mt-5 max-w-lg text-base leading-7 text-black/65 md:text-lg">
                    Descubre una tienda visualmente limpia, con piezas listas para combinar estilo y practicidad en cada compra.
                </p>
                <div class="mt-8 flex flex-wrap gap-3">
                    <a
                        href="#destacados"
                        class="rounded-full bg-black px-6 py-3 text-sm font-semibold text-white transition hover:bg-black/85"
                    >
                        Explora nuestros productos
                    </a>
                    <a
                        href="#nueva-coleccion"
                        class="rounded-full border border-black/15 bg-white px-6 py-3 text-sm font-semibold text-black transition hover:bg-black/5"
                    >
                        Nueva coleccion
                    </a>
                </div>
                <div class="mt-8 grid max-w-md grid-cols-3 gap-4 border-t border-black/10 pt-5 text-sm text-black/60">
                    <div>
                        <p class="text-2xl font-semibold text-black">120+</p>
                        <p>Referencias</p>
                    </div>
                    <div>
                        <p class="text-2xl font-semibold text-black">24h</p>
                        <p>Atencion</p>
                    </div>
                    <div>
                        <p class="text-2xl font-semibold text-black">Top</p>
                        <p>Calidad</p>
                    </div>
                </div>
            </div>

            <div class="relative mx-auto w-full max-w-132.5">
                <div class="absolute -left-3 top-8 h-28 w-28 rounded-3xl border border-black/10 bg-black/5" />
                <img
                    :src="heroImage"
                    alt="Coleccion de bolsos"
                    class="relative z-10 w-full rounded-4xl border border-black/10 object-cover shadow-[0_24px_60px_rgba(0,0,0,0.14)]"
                >
                <div class="absolute -bottom-6 right-5 z-20 rounded-2xl border border-black/10 bg-white px-4 py-3 shadow-lg backdrop-blur-sm">
                    <p class="text-xs uppercase tracking-[0.15em] text-black/55">Compra facil por WhatsApp</p>
                    <p class="mt-1 text-sm font-semibold text-black">Un clic y ordenas directo</p>
                </div>
            </div>
        </div>
    </section>
</template>

<script setup lang="ts">
const heroImage = new URL("../../assets/photos/bolso2.png", import.meta.url).href;
</script>
