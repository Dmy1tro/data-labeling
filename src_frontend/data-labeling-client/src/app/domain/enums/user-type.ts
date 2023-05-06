
export enum UserType {
    Customer = 1,
    Performer = 2
}

export function userTypeToName(type: UserType) {
    if (type === UserType.Customer) return 'Customer'
    if (type === UserType.Performer) return 'Performer'
}
