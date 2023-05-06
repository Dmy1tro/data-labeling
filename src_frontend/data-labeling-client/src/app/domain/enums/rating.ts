export enum Rating {
    Awful = 1,
    Acceptable = 2,
    Normal = 3,
    Good = 4,
    Perfect = 5
}

export function ratingToName(value: Rating) {
    if (value === Rating.Awful) return 'Awful'
    if (value === Rating.Acceptable) return 'Acceptable'
    if (value === Rating.Normal) return 'Normal'
    if (value === Rating.Good) return 'Good'
    if (value === Rating.Perfect) return 'Perfect'
}
