
export function enumSelector(data): IEnumSelect[] {
    return Object.keys(data)
        .filter(key => isNaN(+key))
        .map(key => ({ title: key, value: data[key] }))
}

export interface IEnumSelect {
    title: string
    value: number
}
