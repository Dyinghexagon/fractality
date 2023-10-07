import { Injectable } from "@angular/core";

@Injectable()
export class AppConfig {
    public get userApi(): string { return "api/users" }
    public get fractalGenerateApi(): string { return "api/fractal" }
}