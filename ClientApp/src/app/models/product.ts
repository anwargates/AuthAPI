export interface Product {
    id: number;
    productName: string;
    units: string;
    price: number;
}

export interface ProductAddDto {
    productName: string;
    units: string;
    price: number;
}

