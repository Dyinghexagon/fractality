export abstract class FractalModel {
    public canvas: string[][];

    public abstract draw(context: CanvasRenderingContext2D, width: number, height: number): void;

    constructor(data: IFractalModel) {
        this.canvas = data.Canvas;
    }
}

export interface IFractalModel {
    Canvas: string[][];
}