export interface Medicine {
  id?: number;
  name: string;
  description: string;
  manufacturer: string;
  price: number;
  isRequiredPrescription: boolean;
}

export interface CreateOrderRequest {
  items: { medicineId: number }[];
}

export interface OrderResponse {
  orderId: number;
  totalPrice: number;
}
