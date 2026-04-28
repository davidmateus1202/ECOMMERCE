// routes

export const ROUTER_NAME = {
    CUSTOMER: {
        HOME: 'CustomerHome',
        STOREFRONT: 'CustomerStorefront',
        ACCESSORIES: 'CustomerAccessories',
        BAGS: 'CustomerBags',
        STOREFRONT_PRODUCT_DETAILS: 'CustomerStorefrontProductDetails',
        PRODUCT_DETAILS: 'ProductDetails',
        CART: 'CustomerCart',
        CHECKOUT: 'Checkout',
        ORDER_CONFIRMATION: 'OrderConfirmation',
    },
    ADMIN: {
        DASHBOARD: 'AdminDashboard',
        LOGIN: 'AdminLogin',
        FORGOT_PASSWORD: 'AdminForgotPassword',
        RESET_PASSWORD: 'AdminResetPassword',
        REGISTER: 'AdminRegister',
        PRODUCT_MANAGEMENT: 'ProductManagement',
        ORDER_MANAGEMENT: 'OrderManagement',
        PRODUCTS: 'Products',
        CREATE_PRODUCT: 'CreateProduct',
        EDIT_PRODUCT: 'EditProduct',
        ACCESSORIES_SECTION: 'AdminAccessoriesSection',
        BAGS_SECTION: 'AdminBagsSection',
    }
}

export type RouterName = typeof ROUTER_NAME[keyof typeof ROUTER_NAME];