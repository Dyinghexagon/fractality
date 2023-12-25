import { Component } from "@angular/core";
import { MdbModalRef } from "mdb-angular-ui-kit/modal";

@Component({
    selector: "fractal-modal",
    templateUrl: "./fractal-modal.component.html"
})
export class FractalModal {
    constructor(public modalRef: MdbModalRef<FractalModal>) {}
}