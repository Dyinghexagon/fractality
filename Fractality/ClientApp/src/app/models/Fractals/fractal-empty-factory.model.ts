import { JuliaSet } from "./julia-set.model";
import { MandelbrotSet } from "./mandelbrot-set.mode;";

export class FractalEmptyFactory {
    public static createEmptyMandelbrotSet(): MandelbrotSet {
        return new MandelbrotSet({
            Hx: 0,
            Hy: 0,
            X: 0,
            Y: 0,
            SizeArea: 3,
            ScaleArea: 3,
            Canvas: [],
        });
    }

    public static createEmptyJuliaSet(): JuliaSet {
        return new JuliaSet({
            Canvas: []
        });
    }
}