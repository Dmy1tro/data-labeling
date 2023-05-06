export interface IOrder {
    id: number,
    name: string,
    requirements: string,
    datSetRequiredCount: number,
    currentProgress: number,
    isCompleted: boolean,
    isCanceled: boolean,
    variants: string[],
    price: number,
    type: number,
    priority: number,
    deadline: string,
    customerId: number,
    orderPaymentId: number | null
}
