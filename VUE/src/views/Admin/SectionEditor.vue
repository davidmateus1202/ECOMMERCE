<template>
    <div class="min-h-full bg-[#f7f7fb] p-5 md:p-8">
        <div class="mx-auto flex max-w-6xl flex-col gap-6">
            <section class="rounded-4xl bg-white p-6 shadow-sm shadow-slate-200/70 md:p-8">
                <div class="flex flex-col gap-5 lg:flex-row lg:items-start lg:justify-between">
                    <div>
                        <p class="text-sm font-semibold text-slate-400">Secciones / {{ sectionName }}</p>
                        <h1 class="mt-3 text-3xl font-bold tracking-tight text-slate-900 md:text-4xl">Configurar {{ sectionName }}</h1>
                        <p class="mt-3 max-w-3xl text-sm text-slate-500 md:text-base">
                            Edita los textos que se muestran en la landing pública de esta categoría. Los cambios impactan directamente las secciones que creamos para storefront.
                        </p>
                    </div>

                    <button
                        type="button"
                        class="rounded-2xl bg-indigo-600 px-6 py-3 text-sm font-semibold text-white shadow-lg shadow-indigo-200 transition hover:bg-indigo-500 disabled:cursor-not-allowed disabled:bg-indigo-300 cursor-pointer"
                        :disabled="loading || saving || !formState"
                        @click="saveSection"
                    >
                        {{ saving ? 'Guardando...' : 'Guardar sección' }}
                    </button>
                </div>
            </section>

            <section v-if="formState" class="grid gap-6 xl:grid-cols-[minmax(0,1.2fr)_minmax(320px,0.8fr)]">
                <article class="rounded-4xl bg-white p-6 shadow-sm shadow-slate-200/70 md:p-7">
                    <div class="grid gap-5 md:grid-cols-2">
                        <label class="block md:col-span-2">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Categoría</span>
                            <input v-model="formState.category" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100">
                        </label>

                        <label class="block md:col-span-2">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Título principal</span>
                            <input v-model="formState.title" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100">
                        </label>

                        <label class="block md:col-span-2">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Descripción principal</span>
                            <textarea v-model="formState.description" rows="4" class="w-full rounded-[28px] border border-slate-200 bg-slate-50 px-4 py-4 text-sm leading-7 text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100"></textarea>
                        </label>

                        <label class="block">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Eyebrow destacados</span>
                            <input v-model="formState.featuredEyebrow" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100">
                        </label>

                        <label class="block">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Eyebrow catálogo completo</span>
                            <input v-model="formState.allEyebrow" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100">
                        </label>

                        <label class="block md:col-span-2">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Título catálogo completo</span>
                            <input v-model="formState.allTitle" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100">
                        </label>

                        <label class="block md:col-span-2">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Descripción catálogo completo</span>
                            <textarea v-model="formState.allDescription" rows="4" class="w-full rounded-[28px] border border-slate-200 bg-slate-50 px-4 py-4 text-sm leading-7 text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100"></textarea>
                        </label>

                        <label class="block">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Título sin productos</span>
                            <input v-model="formState.emptyTitle" type="text" class="h-12 w-full rounded-2xl border border-slate-200 bg-slate-50 px-4 text-sm font-medium text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100">
                        </label>

                        <label class="block">
                            <span class="mb-2 block text-sm font-semibold text-slate-600">Descripción sin productos</span>
                            <textarea v-model="formState.emptyDescription" rows="4" class="w-full rounded-[28px] border border-slate-200 bg-slate-50 px-4 py-4 text-sm leading-7 text-slate-700 outline-none transition focus:border-indigo-500 focus:bg-white focus:ring-2 focus:ring-indigo-100"></textarea>
                        </label>
                    </div>
                </article>

                <article class="rounded-4xl bg-white p-6 shadow-sm shadow-slate-200/70 md:p-7">
                    <p class="text-sm font-semibold uppercase tracking-[0.2em] text-slate-400">Vista previa de textos</p>
                    <div class="mt-5 rounded-[28px] bg-slate-950 p-6 text-white">
                        <p class="text-xs font-semibold uppercase tracking-[0.18em] text-white/55">{{ formState.featuredEyebrow }}</p>
                        <h2 class="mt-3 text-3xl font-semibold leading-tight">{{ formState.title }}</h2>
                        <p class="mt-4 text-sm leading-7 text-white/72">{{ formState.description }}</p>
                    </div>
                    <div class="mt-5 rounded-[28px] border border-slate-200 bg-slate-50 p-5">
                        <p class="text-xs font-semibold uppercase tracking-[0.18em] text-slate-400">{{ formState.allEyebrow }}</p>
                        <h3 class="mt-3 text-2xl font-semibold text-slate-900">{{ formState.allTitle }}</h3>
                        <p class="mt-3 text-sm leading-7 text-slate-500">{{ formState.allDescription }}</p>
                    </div>
                    <p class="mt-5 text-xs text-slate-400">Slug técnico: {{ slug }}</p>
                </article>
            </section>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { toast } from 'vue3-toastify';
import {
    fetchAdminStorefrontSection,
    updateAdminStorefrontSection,
    type StorefrontSection,
    type StorefrontSectionPayload,
} from '../../api/storefrontSectionApi';

const props = defineProps<{
    slug: string;
}>();

const loading = ref(false);
const saving = ref(false);
const formState = ref<StorefrontSection | null>(null);

const slug = computed(() => props.slug);
const sectionName = computed(() => formState.value?.category ?? props.slug);

const loadSection = async () => {
    loading.value = true;

    try {
        formState.value = await fetchAdminStorefrontSection(slug.value);
    } catch (error) {
        toast.error(error instanceof Error ? error.message : 'No fue posible cargar la sección.');
    } finally {
        loading.value = false;
    }
};

const saveSection = async () => {
    if (!formState.value || saving.value) {
        return;
    }

    saving.value = true;

    try {
        const payload: StorefrontSectionPayload = {
            category: formState.value.category.trim(),
            title: formState.value.title.trim(),
            description: formState.value.description.trim(),
            featuredEyebrow: formState.value.featuredEyebrow.trim(),
            allEyebrow: formState.value.allEyebrow.trim(),
            allTitle: formState.value.allTitle.trim(),
            allDescription: formState.value.allDescription.trim(),
            emptyTitle: formState.value.emptyTitle.trim(),
            emptyDescription: formState.value.emptyDescription.trim(),
        };

        formState.value = await updateAdminStorefrontSection(slug.value, payload);
        toast.success('Sección actualizada correctamente.');
    } catch (error) {
        toast.error(error instanceof Error ? error.message : 'No fue posible guardar la sección.');
    } finally {
        saving.value = false;
    }
};

onMounted(() => {
    void loadSection();
});
</script>