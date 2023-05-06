import { IOrder } from "./order";

export interface IOrderPaymentHistory {
    id: number,
    orderId: number,
    customerId: number,
    price: number,
    createdData: string,
    order: IOrder
}