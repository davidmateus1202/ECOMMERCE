import type { ShopProduct } from "../types/shopProduct";

export const WHATSAPP_PHONE_NUMBER = "573001234567";

const formatCopPrice = (price: number): string => {
    return new Intl.NumberFormat("es-CO", {
        style: "currency",
        currency: "COP",
        maximumFractionDigits: 0,
    }).format(price);
};

export const buildWhatsAppMessage = (product: ShopProduct): string => {
    return [
        "Hola, estoy interesado en comprar este producto:",
        `Nombre: ${product.nombre}`,
        `Precio: ${formatCopPrice(product.precio)}`,
        `Referencia: ${product.sku ?? `ID-${product.id}`}`,
        "Me puedes dar mas informacion, por favor?",
    ].join("\n");
};

export const buildWhatsAppLink = (
    product: ShopProduct,
    phoneNumber = WHATSAPP_PHONE_NUMBER,
): string => {
    const encodedMessage = encodeURIComponent(buildWhatsAppMessage(product));
    return `https://wa.me/${phoneNumber}?text=${encodedMessage}`;
};

export const buildWhatsAppContactLink = (
    message: string,
    phoneNumber = WHATSAPP_PHONE_NUMBER,
): string => {
    return `https://wa.me/${phoneNumber}?text=${encodeURIComponent(message)}`;
};

export const openWhatsAppForProduct = (
    product: ShopProduct,
    phoneNumber = WHATSAPP_PHONE_NUMBER,
): void => {
    const url = buildWhatsAppLink(product, phoneNumber);
    window.open(url, "_blank", "noopener,noreferrer");
};
