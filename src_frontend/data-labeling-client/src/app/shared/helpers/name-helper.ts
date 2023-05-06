import { OrderPriority } from "src/app/domain/enums/order-priority"
import { orderTypeToName } from "src/app/domain/enums/order-type"
import { ratingToName } from "src/app/domain/enums/rating"
import { userTypeToName } from "src/app/domain/enums/user-type"
import { enumSelector } from "./enum-selector"

export function toPriorityTitle(value: number): string {
    return enumSelector(OrderPriority).find(p => p.value === value).title
}

export function toTypeTitle(value: number): string {
    return orderTypeToName(value)
}

export function toYesNo(value: any) {
    return !!value ? 'Yes' : 'No'
}

export function toUserType(value: number): string {
    return userTypeToName(value)
}

export function toUserRating(value: number): string {
    return ratingToName(value)
}
