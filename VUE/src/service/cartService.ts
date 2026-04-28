import { toast } from "vue3-toastify";

const CART_STORAGE_KEY = "atelier-market-cart";
const CART_UPDATED_EVENT = "atelier-market-cart-updated";

export interface CartItem {
    id: string;
    name: string;
    price: number;
    image?: string;
    quantity: number;
    sku?: string;
}

const getStorage = (): Storage | null => {
    if (typeof window === "undefined") {
        return null;
    }

    return window.localStorage;
};

export const getCartItems = (): CartItem[] => {
    const storage = getStorage();

    if (!storage) {
        return [];
    }

    const rawCart = storage.getItem(CART_STORAGE_KEY);

    if (!rawCart) {
        return [];
    }

    try {
        const parsedCart = JSON.parse(rawCart) as CartItem[];
        return Array.isArray(parsedCart) ? parsedCart : [];
    } catch {
        return [];
    }
};

export const saveCartItems = (items: CartItem[]) => {
    const storage = getStorage();

    if (!storage) {
        return;
    }

    storage.setItem(CART_STORAGE_KEY, JSON.stringify(items));

    if (typeof window !== "undefined") {
        window.dispatchEvent(new CustomEvent(CART_UPDATED_EVENT));
    }
};

export const addItemToCart = (item: CartItem) => {
    const items = getCartItems();
    const existingItem = items.find((cartItem) => cartItem.id === item.id);

    if (existingItem) {
        existingItem.quantity += item.quantity;
    } else {
        items.push(item);
    }

    saveCartItems(items);
    toast.success(`${item.name} fue agregado al carrito.`);
};

export const removeItemFromCart = (itemId: string) => {
    const items = getCartItems().filter((item) => item.id !== itemId);
    saveCartItems(items);
    toast.info("Producto eliminado del carrito.");
};

export const updateCartItemQuantity = (itemId: string, quantity: number) => {
    const items = getCartItems();
    const targetItem = items.find((item) => item.id === itemId);

    if (!targetItem) {
        return;
    }

    if (quantity <= 0) {
        removeItemFromCart(itemId);
        return;
    }

    targetItem.quantity = quantity;
    saveCartItems(items);
};

export const clearCart = () => {
    saveCartItems([]);
    toast.info("Carrito vaciado.");
};

export const getCartItemsCount = () => {
    return getCartItems().reduce((total, item) => total + item.quantity, 0);
};

export const getCartTotal = () => {
    return getCartItems().reduce((total, item) => total + item.price * item.quantity, 0);
};

export const subscribeToCartUpdates = (callback: () => void) => {
    if (typeof window === "undefined") {
        return () => undefined;
    }

    const handler = () => callback();
    window.addEventListener(CART_UPDATED_EVENT, handler);
    window.addEventListener("storage", handler);

    return () => {
        window.removeEventListener(CART_UPDATED_EVENT, handler);
        window.removeEventListener("storage", handler);
    };
};
