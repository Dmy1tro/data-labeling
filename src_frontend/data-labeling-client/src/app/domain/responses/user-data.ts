import { UserType } from "../enums/user-type";

export interface IUserData {
    id: number,
    token: string,
    email: string,
    fullName: string,
    userType: UserType,
    roles: string
}
