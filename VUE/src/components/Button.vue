<template>
    <button :type="type" :disabled="loading"
        class="w-full h-11 rounded-full text-white bg-indigo-500 hover:opacity-90 transition-opacity flex items-center justify-center gap-2 disabled:opacity-70 disabled:cursor-not-allowed cursor-pointer">
        <slot v-if="!loading"></slot>
        <svg v-else xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none"
            stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
            class="animate-spin icon icon-tabler icons-tabler-outline icon-tabler-loader-2">
            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
            <path d="M12 3a9 9 0 1 0 9 9" />
        </svg>
    </button>
</template>

<script setup lang="ts">
interface Props {
    loading?: boolean
    type?: "button" | "submit" | "reset"
}

withDefaults(defineProps<Props>(), {
    loading: false,
    type: "button"
})
</script>