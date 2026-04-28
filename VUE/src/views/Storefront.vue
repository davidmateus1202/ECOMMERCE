<template>
    <div class="overflow-x-hidden bg-white text-black">
        <StorePromoBar />
        <StoreHeroRetail />
        <StoreFeatureStrip />
        <StoreEditorialMosaic :left-feature="leftFeature" :mosaic-cards="mosaicCards" />
        <StoreCategoryGallery :categories="catalogCategories" @select-category="handleCategorySelection" />
        <StoreTopProducts
            :tabs="productTabs"
            :active-tab="activeTab"
            :products="tabProducts"
            @change-tab="activeTab = $event"
            @quick-view="openQuickView"
        />
        <ProductGrid
            section-id="featured-catalog"
            eyebrow="Featured product"
            title="Nueva coleccion"
            description="Una segunda banda de producto para conservar profundidad visual y mantener el catalogo con ritmo comercial."
            :products="newCollectionProducts"
            cta-label="Comprar"
            @quick-view="openQuickView"
        />
        <PromoSection />
        <FooterSection />

        <ProductQuickViewModal
            :product="selectedProduct"
            @close="selectedProduct = null"
        />
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import ProductGrid from "../components/shop/ProductGrid.vue";
import PromoSection from "../components/shop/PromoSection.vue";
import FooterSection from "../components/shop/FooterSection.vue";
import ProductQuickViewModal from "../components/shop/ProductQuickViewModal.vue";
import StorePromoBar from "../components/shop/StorePromoBar.vue";
import StoreHeroRetail from "../components/shop/StoreHeroRetail.vue";
import StoreFeatureStrip from "../components/shop/StoreFeatureStrip.vue";
import StoreEditorialMosaic from "../components/shop/StoreEditorialMosaic.vue";
import StoreCategoryGallery from "../components/shop/StoreCategoryGallery.vue";
import StoreTopProducts from "../components/shop/StoreTopProducts.vue";
import { fetchStorefrontData, type StorefrontResponse } from "../api/productApi";
import { hasDedicatedCategoryRoute } from "../routes/category-navigation";
import { ROUTER_NAME } from "../routes/router-name";
import type { ShopProduct } from "../types/shopProduct";

const route = useRoute();
const router = useRouter();

const productTabs = [
    { label: "Latest", value: "latest" },
    { label: "Best seller", value: "best-seller" },
    { label: "Featured", value: "featured" },
];

const storefrontData = ref<StorefrontResponse | null>(null);
const selectedCategory = ref("");
const selectedProduct = ref<ShopProduct | null>(null);
const activeTab = ref("latest");
const searchTerm = ref("");

const fallbackHighlight = {
    title: "Catalogo disponible",
    description: "Explora las referencias activas publicadas actualmente en la tienda.",
    tag: "Storefront",
    image: "",
};

const catalogCategories = computed(() => storefrontData.value?.categories ?? []);
const leftFeature = computed(() => storefrontData.value?.leftFeature ?? fallbackHighlight);
const mosaicCards = computed(() => storefrontData.value?.mosaicCards ?? []);
const allProducts = computed(() => storefrontData.value?.products ?? []);
const availableCategories = computed(() => new Set(catalogCategories.value.map((category) => category.name)));

const filteredProducts = computed(() => {
    return allProducts.value.filter((product) => {
        const matchesCategory = !selectedCategory.value || product.categoria === selectedCategory.value;
        const normalizedQuery = searchTerm.value.trim().toLowerCase();

        if (!normalizedQuery) {
            return matchesCategory;
        }

        const haystack = [product.nombre, product.descripcion, product.categoria, product.sku ?? ""]
            .join(" ")
            .toLowerCase();

        return matchesCategory && haystack.includes(normalizedQuery);
    });
});

const newCollectionProducts = computed(() => storefrontData.value?.newCollectionProducts ?? []);
const latestProducts = computed(() => storefrontData.value?.latestProducts ?? []);
const bestSellerProducts = computed(() => storefrontData.value?.bestSellerProducts ?? []);
const featuredProducts = computed(() => {
    const baseProducts = filteredProducts.value.length > 0 ? filteredProducts.value : allProducts.value;
    return baseProducts.slice(0, 4);
});

const tabProducts = computed(() => {
    if (activeTab.value === "best-seller") {
        return bestSellerProducts.value;
    }

    if (activeTab.value === "featured") {
        return featuredProducts.value;
    }

    return latestProducts.value;
});

const openQuickView = (product: ShopProduct) => {
    selectedProduct.value = product;
};

const handleCategorySelection = (categoryName: string) => {
    if (hasDedicatedCategoryRoute(categoryName)) {
        return;
    }

    selectedCategory.value = categoryName;
};

const loadStorefrontData = async () => {
    storefrontData.value = await fetchStorefrontData();
};

onMounted(() => {
    loadStorefrontData();
});

watch(
    [() => route.query.category, catalogCategories],
    ([categoryQuery]) => {
        const categoryValue = Array.isArray(categoryQuery) ? categoryQuery[0] : categoryQuery;
        const defaultCategory = catalogCategories.value[0]?.name ?? "";
        selectedCategory.value = categoryValue && availableCategories.value.has(categoryValue) ? categoryValue : defaultCategory;
    },
    { immediate: true },
);

watch(
    () => route.query.q,
    (queryValue) => {
        const normalizedValue = Array.isArray(queryValue) ? queryValue[0] : queryValue;
        searchTerm.value = normalizedValue ?? "";
    },
    { immediate: true },
);

watch(selectedCategory, (categoryValue) => {
    if (!categoryValue || route.query.category === categoryValue) {
        return;
    }

    const nextQuery = { ...route.query, category: categoryValue };

    router.replace({
        name: ROUTER_NAME.CUSTOMER.STOREFRONT,
        query: nextQuery,
    });
});
</script>
