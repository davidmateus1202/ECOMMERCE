<template>
    <div
        class="flex items-center w-full bg-white border border-gray-300/80 h-12 rounded-full overflow-hidden pl-6 gap-2">
        <slot name="icon"></slot>

        <input :value="modelValue" @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
            :type="type" :placeholder="placeholder"
            class="bg-transparent text-gray-500 placeholder-gray-500 outline-none text-sm w-full h-full"
            :required="required" />
    </div>
</template>

<script setup lang="ts">
defineProps<{
    modelValue: string
    type?: string
    placeholder?: string
    required?: boolean
}>()

defineEmits<{
    (e: 'update:modelValue', value: string): void
}>()
</script>