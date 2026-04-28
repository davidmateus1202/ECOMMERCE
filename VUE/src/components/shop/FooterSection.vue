<template>
    <footer class="border-t border-black/10 bg-white">
        <div class="mx-auto grid max-w-7xl gap-8 px-6 py-10 md:grid-cols-3 md:px-10">
            <div>
                <p class="text-lg font-semibold text-black">Atelier Market</p>
                <p class="mt-3 max-w-sm text-sm leading-6 text-black/60">
                    Tienda de bolsos y accesorios con enfoque visual limpio, atencion cercana y compra simple por WhatsApp.
                </p>
            </div>

            <div>
                <p class="text-sm font-semibold uppercase tracking-[0.14em] text-black/50">Navegacion</p>
                <ul class="mt-3 space-y-2 text-sm text-black/65">
                    <li><a href="#featured-catalog" class="transition hover:text-black">Nueva coleccion</a></li>
                    <li><a href="#top-products" class="transition hover:text-black">Productos destacados</a></li>
                    <li><a :href="shippingPolicyLink" target="_blank" rel="noreferrer" class="transition hover:text-black">Politica de envios</a></li>
                </ul>
            </div>

            <div>
                <p class="text-sm font-semibold uppercase tracking-[0.14em] text-black/50">Contacto</p>
                <p class="mt-3 text-sm text-black/65">WhatsApp de ventas</p>
                <p class="mt-1 text-base font-semibold text-black">+57 300 123 4567</p>
                <p class="mt-3 text-xs text-black/45">Lunes a sabado · 9:00 AM a 7:00 PM</p>
            </div>
        </div>
        <div class="border-t border-black/8 py-4 text-center text-xs text-black/45">
            © 2026 Atelier Market. Todos los derechos reservados.
        </div>
    </footer>
</template>

<script setup lang="ts">
import { buildWhatsAppContactLink } from '../../service/whatsappService';

const shippingPolicyLink = buildWhatsAppContactLink('Hola, quiero informacion sobre la politica de envios.');
</script>
