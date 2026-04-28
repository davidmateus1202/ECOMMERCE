import type { ShopProduct } from "../types/shopProduct";

import bolso1 from "../assets/photos/bolso1.png";
import bolso2 from "../assets/photos/bolso2.png";
import bolso3 from "../assets/photos/bolso3.png";
import bolso4 from "../assets/photos/bolso4.png";
import bolso5 from "../assets/photos/bolso5.png";
import bolso6 from "../assets/photos/bolso6.png";
import bolso7 from "../assets/photos/bolso7.png";

export const shopProducts: ShopProduct[] = [
    {
        id: 1,
        nombre: "Bolso Aura Mini",
        descripcion: "Bolso compacto con textura premium y correa ajustable.",
        precio: 189000,
        imagen: bolso1,
        categoria: "Bolsos",
        sku: "REF-001",
    },
    {
        id: 2,
        nombre: "Tote Siena",
        descripcion: "Diseño amplio para uso diario con acabado mate elegante.",
        precio: 235000,
        imagen: bolso2,
        categoria: "Bolsos",
        sku: "REF-002",
    },
    {
        id: 3,
        nombre: "Crossbody Nara",
        descripcion: "Formato cruzado liviano ideal para ciudad y viajes.",
        precio: 164000,
        imagen: bolso3,
        categoria: "Accesorios",
        sku: "REF-003",
    },
    {
        id: 4,
        nombre: "Set Duo Clasico",
        descripcion: "Set de bolso y cartera con herrajes dorados discretos.",
        precio: 279000,
        imagen: bolso4,
        categoria: "Catalogo",
        sku: "REF-004",
    },
    {
        id: 5,
        nombre: "Shoulder Luna",
        descripcion: "Silueta curva con interior funcional y cierre metalico.",
        precio: 212000,
        imagen: bolso5,
        categoria: "Bolsos",
        sku: "REF-005",
    },
    {
        id: 6,
        nombre: "Cartera Vela",
        descripcion: "Cartera mediana con compartimentos para uso comercial.",
        precio: 148000,
        imagen: bolso6,
        categoria: "Accesorios",
        sku: "REF-006",
    },
    {
        id: 7,
        nombre: "Bolso Atelier",
        descripcion: "Modelo estructurado para looks de oficina y eventos.",
        precio: 259000,
        imagen: bolso7,
        categoria: "Catalogo",
        sku: "REF-007",
    },
];

export const getShopProductById = (productId: number): ShopProduct | undefined => {
    return shopProducts.find((product) => product.id === productId);
};
