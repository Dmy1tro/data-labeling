export enum OrderType {
    CollectData = 1,
    LabelData = 2
}

export function orderTypeToName(type: OrderType) {
    if (type === OrderType.CollectData) return 'Collect data'
    if (type === OrderType.LabelData) return 'Label data'
}
