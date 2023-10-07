import { Component } from "@angular/core";
import { Guid } from "guid-typescript";
import { UserService } from "src/app/services/user.service";

@Component({
    selector: "main-page",
    templateUrl: "./main-page.component.html",
    styleUrls: [ "./main-page.component.scss" ]
})
export class MainPageComponent {
    
    constructor(private readonly userServices: UserService) {}

    public async createUser(): Promise<void> {
        const id =  await this.userServices.createUser(
        {
            id: Guid.create().toString(),
            name: "test",
            email: "test@test.ru",
            isActive: true,
            dateCreated: new Date(),
            dateUpdated: new Date() 
        });
        console.warn(id);
    }

}