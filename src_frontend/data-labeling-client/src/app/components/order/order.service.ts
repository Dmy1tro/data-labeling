import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiUrl } from "src/app/domain/constants/api-url";
import { OrderPriority } from "src/app/domain/enums/order-priority";
import { IApiResponse } from "src/app/domain/responses/api-response";
import { IOrder } from "src/app/domain/responses/order";
import { IUserData } from "src/app/domain/responses/user-data";

@Injectable({
    providedIn: 'root'
})
export class OrderService {

    constructor(private httpClient: HttpClient) {}

    getOrderForCustomer(orderId: number): Observable<IApiResponse<IOrder>> {
        const uri = ApiUrl.getOrderForCustomer(orderId)
        return this.httpClient.get<IApiResponse<IOrder>>(uri)
    }

    create(data): Observable<any> {
        const uri = ApiUrl.createOrder()
        return this.httpClient.post<any>(uri, data)
    }

    cancel(orderId: number): Observable<any> {
        const uri = ApiUrl.cancelOrder(orderId)
        return this.httpClient.post(uri, {})
    }

    getProgress(orderId: number): Observable<IApiResponse<number>> {
        const uri = ApiUrl.getProgress(orderId)
        return this.httpClient.get<IApiResponse<number>>(uri)
    }

    getPrice(request: GetOrderPriceRequest): Observable<IApiResponse<number>> {
        const uri = ApiUrl.getOrderPrice(request.datSetRequiredCount, request.type, request.priority, request.deadline)
        return this.httpClient.get<IApiResponse<number>>(uri)
    }

    getOrderForPerformer(orderId: number): Observable<IApiResponse<IOrder>> {
        const uri = ApiUrl.getOrderForPerformer(orderId)
        return this.httpClient.get<IApiResponse<IOrder>>(uri)
    }

    getForCustomer(filter: OrderForCustomerFilter): Observable<IApiResponse<IOrder[]>> {
        const uri = ApiUrl.getOrdersForCustomer(filter.orderType, filter.minPriority, filter.maxPriority, filter.isCompleted, filter.isCanceled)
        return this.httpClient.get<IApiResponse<IOrder[]>>(uri)
    }

    getForPerformer(filter: OrderForPerformerFilter): Observable<IApiResponse<IOrder[]>> {
        const uri = ApiUrl.getOrdersForPerformer(filter.orderType)
        return this.httpClient.get<IApiResponse<IOrder[]>>(uri)
    }

    getAllowedPriorities(): Observable<IApiResponse<OrderPriority[]>> {
        const uri = ApiUrl.getAllowedPriorities()
        return this.httpClient.get<IApiResponse<OrderPriority[]>>(uri)
    }

    getPerformers(orderId: number): Observable<IApiResponse<IUserData[]>> {
        const uri = ApiUrl.getOrderPerformers(orderId)
        return this.httpClient.get<IApiResponse<IUserData[]>>(uri)
    }
}

export class GetOrderPriceRequest {
    datSetRequiredCount: number;
    type: string
    priority: string
    deadline: string
}

export class OrderForCustomerFilter {
    constructor() {
        this.orderType = ''
        this.minPriority = ''
        this.maxPriority = ''
        this.isCompleted = ''
        this.isCanceled = ''
    }

    orderType: string | null
    minPriority: string | null
    maxPriority: string | null
    isCompleted: boolean | string | null
    isCanceled: boolean | string | null
}

export class OrderForPerformerFilter {
    constructor() {
        this.orderType = ''
    }

    orderType: string | null
}