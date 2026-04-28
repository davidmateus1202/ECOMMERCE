<template>
    <div class="flex flex-col w-full h-auto overflow-x-hidden">
        <Header />
        <NewProducts/>
        <StyleSection/>
        <MadeSection />
    </div>
</template>

<script setup lang="ts">
import { Header, NewProducts, StyleSection, MadeSection } from '../components';

</script>