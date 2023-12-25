import { FractalModel, FractalType, IFractalModel } from "./fractal.model";

export class JuliaSet extends FractalModel {

    public type: FractalType = FractalType.juliaSet;
    
    constructor(data: IFractalModel) {
        super(data);
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