import type { RouteLocationRaw } from "vue-router";
import { ROUTER_NAME } from "./router-name";

const normalizeCategory = (category: string) => category.trim().toLowerCase();

export const getCategoryRouteLocation = (category: string): RouteLocationRaw => {
    const normalizedCategory = normalizeCategory(category);

    if (normalizedCategory === "accesorios") {
        return { name: ROUTER_NAME.CUSTOMER.ACCESSORIES };
    }

    if (normalizedCategory === "bolsos") {
        return { name: ROUTER_NAME.CUSTOMER.BAGS };
    }

    return {
        name: ROUTER_NAME.CUSTOMER.STOREFRONT,
        query: { category },
    };
};

export const hasDedicatedCategoryRoute = (category: string) => {
    const normalizedCategory = normalizeCategory(category);
    return normalizedCategory === "accesorios" || normalizedCategory === "bolsos";
};