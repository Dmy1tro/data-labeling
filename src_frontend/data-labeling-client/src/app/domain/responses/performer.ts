import { Rating } from "../enums/rating";
import { IUserData } from "./user-data";

export interface IPerformer extends IUserData{
    balance: number,
    rating: Rating
}