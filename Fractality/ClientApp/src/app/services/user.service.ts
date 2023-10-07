import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, NgZone } from "@angular/core";
import { Guid } from "guid-typescript";
import { AppConfig } from "../app.config";
import { IUserModel } from "../models/user.model";
import { BaseService } from "./base.service";

@Injectable()
export class UserService extends BaseService {

    constructor(
        http: HttpClient,
        zone: NgZone,
        @Inject('BASE_URL') baseUrl: string,
        protected config: AppConfig
    ) {
        super(http, zone, baseUrl);
    }

    public createUser(user: IUserModel): Promise<Guid> {
        return this.post(`${this.config.userApi}/create`, user);
    }
    
}