import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";
import { hasStoredAuthToken } from "../service/authService";

//import routes
import { ROUTER_NAME } from "./router-name";

// import layouts
import DefaultLayout from "../layout/DefaultLayout.vue";
import AuthLayout from "../layout/AuthLayout.vue";
import DashboardLayout from "../layout/DashboardLayout.vue";

// import views
import Home from "../views/Home.vue";
import Storefront from "../views/Storefront.vue";
import Accessories from "../views/Accessories.vue";
import Bolsos from "../views/Bolsos.vue";
import StorefrontProductDetails from "../views/StorefrontProductDetails.vue";
import Cart from "../views/Cart.vue";
import Login from "../views/Auth/Login.vue";
import ProductDetails from "../components/ProductDetails.vue";
import ForgetPassword from "../views/Auth/ForgetPassword.vue";
import ResetPassword from "../views/Auth/ResetPassword.vue";
import Products from "../views/Admin/Products.vue";
import EditProducts from "../views/Admin/EditProducts.vue";
import SectionEditor from "../views/Admin/SectionEditor.vue";

const routes: RouteRecordRaw[] = [
    {
        path: "/",
        component: DefaultLayout,
        children: [{ path: "", name: ROUTER_NAME.CUSTOMER.HOME, component: Home }]
    },
    {
        path: "/catalogo",
        component: DefaultLayout,
        children: [{ path: "", name: ROUTER_NAME.CUSTOMER.STOREFRONT, component: Storefront }]
    },
    {
        path: "/accesorios",
        component: DefaultLayout,
        children: [{ path: "", name: ROUTER_NAME.CUSTOMER.ACCESSORIES, component: Accessories }]
    },
    {
        path: "/bolsos",
        component: DefaultLayout,
        children: [{ path: "", name: ROUTER_NAME.CUSTOMER.BAGS, component: Bolsos }]
    },
    {
        path: "/catalogo/producto/:id",
        component: DefaultLayout,
        children: [{ path: "", name: ROUTER_NAME.CUSTOMER.STOREFRONT_PRODUCT_DETAILS, component: StorefrontProductDetails }]
    },
    {
        path: "/carrito",
        component: DefaultLayout,
        children: [{ path: "", name: ROUTER_NAME.CUSTOMER.CART, component: Cart }]
    },
    {
        path: "/auth",
        component: AuthLayout,
        children: [
            {
                path: "login",
                name: ROUTER_NAME.ADMIN.LOGIN,
                component: Login,
            },
            {
                path: "forgot-password",
                name: ROUTER_NAME.ADMIN.FORGOT_PASSWORD,
                component: ForgetPassword,
            },
            {
                path: "/reset-password",
                name: ROUTER_NAME.ADMIN.RESET_PASSWORD,
                component: ResetPassword,
            }
        ]
    },
    {
        path: "/dashboard",
        name: ROUTER_NAME.ADMIN.DASHBOARD,
        meta: { requiresAuth: true },
        component: DashboardLayout,
        children: [
            {
                path: "",
                name: ROUTER_NAME.ADMIN.PRODUCTS,
                component: Products,
            },
            {
                path: "new-product",
                name: ROUTER_NAME.ADMIN.CREATE_PRODUCT,
                component: EditProducts,
            },
            {
                path: "edit-product/:id",
                name: ROUTER_NAME.ADMIN.EDIT_PRODUCT,
                component: EditProducts
            },
            {
                path: "sections/accesorios",
                name: ROUTER_NAME.ADMIN.ACCESSORIES_SECTION,
                component: SectionEditor,
                props: { slug: 'accesorios' },
            },
            {
                path: "sections/bolsos",
                name: ROUTER_NAME.ADMIN.BAGS_SECTION,
                component: SectionEditor,
                props: { slug: 'bolsos' },
            }
        ]
    },
    {
        path: "/product-details/:id",
        component: DefaultLayout,
        children: [{ path: "", name: ROUTER_NAME.CUSTOMER.PRODUCT_DETAILS, component: ProductDetails }]
    },
    {
        path: "/:pathMatch(.*)*",
        redirect: "/"
    }

]

const router = createRouter({
    history: createWebHistory(),
    routes,
    scrollBehavior(_to, _from) {
        return { left: 0, top: 0 };
    },
});

router.beforeEach((to, _from, next) => {
    document.title = to.name as string || "E-COMMERCE";

    const hasToken = hasStoredAuthToken();

    if (to.matched.some(record => record.meta.requiresAuth) && !hasToken) {
        next({ name: ROUTER_NAME.ADMIN.LOGIN });
    } else if (to.name === ROUTER_NAME.ADMIN.LOGIN && hasToken) {
        next({ name: ROUTER_NAME.ADMIN.PRODUCTS });
    } else if (to.name === ROUTER_NAME.ADMIN.FORGOT_PASSWORD && hasToken) {
        next({ name: ROUTER_NAME.ADMIN.PRODUCTS });
    } else if (to.name === ROUTER_NAME.ADMIN.RESET_PASSWORD && hasToken) {
        next({ name: ROUTER_NAME.ADMIN.PRODUCTS });
    } else {
        next();
    }
})

export default router;