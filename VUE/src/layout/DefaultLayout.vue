<template>
    <div class="flex flex-col w-full min-h-screen h-auto">
        <Navbar v-model="isMenuOpen" />
        <main class="flex-grow">
            <router-view />
        </main>
    </div>
</template>

<script setup lang="ts">
import Navbar from '../components/layout/Navbar.vue';
import { ref } from 'vue';
import { useBodyScrollLock } from '../composables/useBodyScrollLock';

const isMenuOpen = ref(false);

useBodyScrollLock(isMenuOpen);
</script>