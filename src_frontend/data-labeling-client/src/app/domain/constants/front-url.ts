import { ExpandOperator } from "rxjs/internal/operators/expand";
import { UserType } from "../enums/user-type";

export namespace FrontUrl {
    export const defaultPage = (userType: UserType | number): any[] => {        
        if (userType === UserType.Customer) {
            return ['/orders/customer']
        }
        if (userType === UserType.Performer) {
            return ['/orders/performer']
        }
        
        return ['/orders/performer']
    }

    export const ordersPage = (userType: UserType): any[] => {
        if (userType === UserType.Customer) {
            return orderForCustomerPage()
        } else {
            return ordersForPerformerPage()
        }
    }

    export const profile = (userType: UserType): any[] => {
        if (userType === UserType.Customer) {
            return customerProfilePage()
        } else {
            return performerProfilePage()
        }
    }

    export const orderForCustomerPage = (): any[] => ['/orders/customer']
    export const ordersForPerformerPage = (): any[] => ['/orders/performer']
    export const reviewOrder = (orderId: number): any[] => ['/orders/review/', orderId]
    export const performOrderPage = (orderId: number): any[] => ['/orders/performer/perform', orderId]

    export const customerProfilePage = (): any[] => ['user/customer']
    export const performerProfilePage = (): any[] => ['user/performer']

    export const loginPage = (): any[] => ['/authentication/login']
}