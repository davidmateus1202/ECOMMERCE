<template>
    <div class="bg-gray-50 flex flex-col items-start h-auto min-h-125 mt-32 p-10 md:p-20">
        <div class="flex flex-col max-w-7xl mx-auto">
            <span class="text-black font-light">@StyleByYou</span>
            <h1 class="text-4xl font-bold mt-2">Estilo a tu manera</h1>
            <h3 class="font-light text-gray-400 mt-2">Catalago de productos para personalizar tu estilo y expresar tu
                individualidad.</h3>
            <div class="hidden lg:flex flex-wrap items-center  justify-center gap-6 mt-10 w-full">
                <RouterLink v-for="(image, index) in Images" :key="index" :to="{ name: ROUTER_NAME.CUSTOMER.STOREFRONT, query: { category: categories[index] } }" class='group w-56'>
                    <img class='rounded-lg w-full group-hover:shadow-xl hover:-translate-y-0.5 duration-300 transition-all h-96 object-cover object-top'
                        :src="image" alt="image">
                </RouterLink>
            </div>
        </div>
        <div class="mt-10 w-full lg:hidden">
            <Slider :images="Images" />
        </div>
    </div>
</template>

<script setup lang="ts">
import Slider from './Slider.vue';
import { ROUTER_NAME } from '../../routes/router-name';

const Images = [
    "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8cGVyc29uYWwlMjBzdHlsZXxlbnwwfHwwfHx8MA%3D%3D&auto=format&fit=crop&w=500&q=60",
    "https://images.unsplash.com/photo-1521334884684-d80222895322?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8cGVyc29uYWwlMjBzdHlsZXxlbnwwfHwwfHx8MA%3D%3D&auto=format&fit=crop&w=500&q=60",
    "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8cGVyc29uYWwlMjBzdHlsZXxlbnwwfHwwfHx8MA%3D%3D&auto=format&fit=crop&w=500&q=60",
    "https://images.unsplash.com/photo-1521334884684-d80222895322?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8cGVyc29uYWwlMjBzdHlsZXxlbnwwfHwwfHx8MA%3D%3D&auto=format&fit=crop&w=500&q=60",
]

const categories = ["Bolsos", "Accesorios", "Bolsos", "Catalogo"];

</script>