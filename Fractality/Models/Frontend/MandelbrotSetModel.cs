namespace Fractality.Models.Frontend
{
    public class MandelbrotSetModel : FractalModel
    {
        public double Hx { get; set; } = 0;

        public double Hy { get; set; } = 0;

        public double X { get; set; }

        public double Y { get; set; }

        public double SizeArea { get; set; } = 3;

        public double ScaleArea { get; set; } = 3;

    }
}
