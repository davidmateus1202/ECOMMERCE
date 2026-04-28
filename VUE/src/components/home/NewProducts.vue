<template>
    <h1 class="text-3xl font-medium text-slate-800 text-center mb-2 mt-32">Nueva Coleccíon</h1>
    <p class="text-slate-600 mb-10 text-center">Explora las últimas incorporaciones a nuestra colección.</p>
    <section class="flex flex-wrap items-center justify-center gap-6">
        <RouterLink v-for="(item, index) in products" :key="index" :to="{ name: ROUTER_NAME.CUSTOMER.PRODUCT_DETAILS, params: { id: item.productId } }" class='group w-56 md:w-72'>
            <img class='rounded-lg w-full group-hover:shadow-xl hover:-translate-y-0.5 duration-300 transition-all h-96 object-cover object-top bg-gray-200 cursor-pointer'
            :src="getProductImage(item)" alt="image">
            <p class='text-sm mt-2'>{{ item.name }}</p>
            <p class="text-sm text-gray-400 mb-5">{{ item.description }}</p>
            <p class='text-xl'>{{ formatCurrency(item.price) }}</p>
        </RouterLink>
    </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { fetchProducts, type Product } from '../../api/productApi';
import { ROUTER_NAME } from '../../routes/router-name';
import { getProductImage } from '../../service/productService';
import { formatCurrency } from '../../service/service';

const products = ref<Product[] | null>(null);

const getProducts = async () => {
    products.value = await fetchProducts();
};

onMounted(() => {
    getProducts();
});


</script>