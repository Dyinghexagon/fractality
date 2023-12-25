export abstract class FractalModel {
    public canvas: string[][];
    public readonly type: FractalType = FractalType.fractal;

    public abstract draw(context: CanvasRenderingContext2D, width: number, height: number): void;

    constructor(data: IFractalModel) {
        this.canvas = data.Canvas;
    }
}

export interface IFractalModel {
    Canvas: string[][];
}

export enum FractalType {
    mandelbrotSet = "MANDELBROT_SET",
    juliaSet = "JULIA_SET",
    fractal = "FRACTAL"
}