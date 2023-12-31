﻿using System.Threading;

namespace Fractality.Models.Backend
{
    public class Fractal : Entity
    {
        public double Hx { get; set; } = 0;

        public double Hy { get; set; } = 0;

        public double X { get; set; }

        public double Y { get; set; }

        public double SizeArea { get; set; } = 3;

        public double ScaleArea { get; set; } = 3;

        public List<List<string>> Canvas { get; set; } = new List<List<string>>();

        public double RecalculateHx(double x, double width)
        {
            return (Hx - SizeArea / 2) + x * (SizeArea / width);
        }

        public double RecalculateHy(double y, double height)
        {
            return (Hy - SizeArea / 2) + y * (SizeArea / height);
        }
    }
}
