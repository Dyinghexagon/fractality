import { FractalModel, FractalType, IFractalModel } from "./fractal.model";

export class MandelbrotSet extends FractalModel {
    public hx: number;
    public hy: number;
    public x: number;
    public y: number;
    public sizeArea: number;
    public scaleArea: number;
    public type: FractalType = FractalType.mandelbrotSet;

    constructor(data: IMandelbrotSet) {
        super(data);
        this.hx = data.Hx;
        this.hy = data.Hy;
        this.x = data.X;
        this.y = data.Y;
        this.sizeArea = data.SizeArea;
        this.scaleArea = data.ScaleArea;
    }

    public draw(context: CanvasRenderingContext2D, width: number, height: number): void {
        for(let x = 0; x < width; x++) {
            for(let y = 0; y < height; y++) {
                var roundedX = Math.floor(x);
                var roundedY = Math.floor(y);
                context.fillStyle = `#${this.canvas[x][y]}` || '#000';
                context.fillRect(roundedX, roundedY, 1, 1);
            }
        }
    }
}

export interface IMandelbrotSet extends IFractalModel {
    Hx: number;
    Hy: number;
    X: number;
    Y: number;
    SizeArea: number;
    ScaleArea: number;
}