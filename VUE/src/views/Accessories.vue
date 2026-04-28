<template>
    <CategoryCollectionView
        category="Accesorios"
        section-key="accessories"
        fallback-title="Accesorios disponibles"
        fallback-description="Explora la seleccion activa de accesorios publicada actualmente en la tienda."
        featured-eyebrow="Featured accessories"
        all-eyebrow="All accessories"
        all-title="Todos los accesorios"
        all-description="Catalogo completo de accesorios activos publicado en la API."
        empty-title="No hay accesorios publicados"
        empty-description="Cuando existan productos activos en la categoria Accesorios, apareceran automaticamente aqui."
    />
</template>

<script setup lang="ts">
import CategoryCollectionView from "../components/shop/CategoryCollectionView.vue";
</script>