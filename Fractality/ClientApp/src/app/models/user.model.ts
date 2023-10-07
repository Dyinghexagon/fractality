import { IEntity } from "./IEntity";

export interface IUserModel extends IEntity {
    name: string;
    email: string;
}