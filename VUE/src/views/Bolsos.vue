<template>
    <CategoryCollectionView
        category="Bolsos"
        section-key="bags"
        fallback-title="Bolsos disponibles"
        fallback-description="Explora la seleccion activa de bolsos publicada actualmente en la tienda."
        featured-eyebrow="Featured bags"
        all-eyebrow="All bags"
        all-title="Todos los bolsos"
        all-description="Catalogo completo de bolsos activos publicado en la API."
        empty-title="No hay bolsos publicados"
        empty-description="Cuando existan productos activos en la categoria Bolsos, apareceran automaticamente aqui."
    />
</template>

<script setup lang="ts">
import CategoryCollectionView from "../components/shop/CategoryCollectionView.vue";
</script>