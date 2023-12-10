using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Fractality.Models.Backend;
using Fractality.Models.Frontend;
using AutoMapper;

namespace Fractality.Controllers
{
    [ApiController]
    [Route("api/fractal")]
    public class FractalGenerateController : ControllerBase
    {
        private readonly ILogger<FractalGenerateController> _logger;
        private readonly IMapper _mapper;
        public FractalGenerateController(
            ILogger<FractalGenerateController> logger,
            IMapper mapper
        ) {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("julia-set")]
        public IActionResult GenerateJuliaSet(
            [FromBody] JuliaSetModel fractalModel,
            [FromQuery] ClickType clickType,
            [FromQuery] int width = 400,
            [FromQuery] int height = 600,
            [FromQuery] int limitIteration = 100,
            [FromQuery] double rel = 0.74543,
            [FromQuery] double im = 0.11301,
            [FromQuery] double xMin = Double.NaN, 
            [FromQuery] double yMin = Double.NaN, 
            [FromQuery] double xMax = Double.NaN,
            [FromQuery] double yMax = Double.NaN
        )
        {
            try
            {
                var juliaSet = _mapper.Map<JuliaSet>(fractalModel);

                if (juliaSet is null)
                {
                    return BadRequest("Fractal not Found");
                }

                var c = new Complex(rel, im);
                
                //var r = CalculateR(c);
                var r = 1.50197192317588;

                if (double.IsNaN(xMin) || Double.IsNaN(xMax) || Double.IsNaN(yMin) || Double.IsNaN(yMax))
                {
                    xMin = -r;
                    yMin = -r;
                    xMax = r;
                    yMax = r;
                }

                var xStep = Math.Abs(xMax - xMin) / width;
                var yStep = Math.Abs(yMax - yMin) / height;
                juliaSet.Canvas.Clear();
                juliaSet.Canvas = new List<List<string>>(width);
                var xyIdx = new Dictionary<int, IDictionary<int, int>>();
                int maxIdx = 0;

                for (var i = 0; i < width; i++)
                {
                    xyIdx.Add(i, new Dictionary<int, int>());
                    for (var j = 0; j < height; j++) 
                    {
                        double x = xMin + i * xStep;
                        double y = yMin + j * yStep;

                        var z = new Complex(x, y);
                        var zIter = SqPolyIteration(z, c, limitIteration, r);
                        var idx = zIter.Count - 1;

                        if (maxIdx < idx)
                        {
                            maxIdx = idx;
                        }

                        xyIdx[i].Add(j, idx);
                    }
                }

                for(var i = 0; i < width; i++)
                {
                    juliaSet.Canvas.Add(new List<string>(height));
                    for (var j = 0; j < height; j++)
                    {
                        var idx = xyIdx[i][j];
                        var x = xMin + i * xStep;
                        var y = yMin + j * yStep;
                        var z = new Complex(x, y);
                        var iter = width - i - 1;
                        juliaSet.Canvas[i].Add(ComplexHeatMap(idx, 0, maxIdx, z, r).ToString());
                    }
                }

                return Ok(_mapper.Map<FractalModel>(juliaSet));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }

        private static double CalculateR(Complex c)
        {
            return (1 + Math.Sqrt(1 + 4 * c.Magnitude)) / 2;
        }

        private static IList<Complex> SqPolyIteration(Complex z0, Complex c, int n, double r = 0)
        {
            var res = new List<Complex>();
            res.Add(z0);
            for (int i = 0; i < n; i++)
            {
                if (r > 0)
                {
                    if (res.Last().Magnitude > r)
                    {
                        break;
                    }
                }
                res.Add(res.Last() * res.Last() + c);
            }
            return res;
        }

        [HttpPost("mandelbrot-set")]
        public IActionResult GenerateMandelbrotSet(
            [FromBody] MandelbrotSetModel fractalModel,
            [FromQuery] ClickType clickType,
            [FromQuery] int width = 400,
            [FromQuery] int height = 600,
            [FromQuery] int limitIteration = 100,
            [FromQuery] double mouseX = 0,
            [FromQuery] double mouseY = 0
            )
        {
            try
            {
                var mandelbrotSet = _mapper.Map<MandelbrotSet>(fractalModel);

                if (mandelbrotSet is null)
                {
                    return BadRequest("Fractal not Found");
                }

                var rand = new Random();
                var min = 0;
                var max = 1000;

                switch (clickType)
                {
                    case ClickType.ZoomIn:
                        mandelbrotSet.Hx = mandelbrotSet.RecalculateHx(mouseX, width);
                        mandelbrotSet.Hy = mandelbrotSet.RecalculateHy(mouseY, height);
                        mandelbrotSet.SizeArea /= mandelbrotSet.ScaleArea;
                        break;
                    case ClickType.Middle:
                        mandelbrotSet.SizeArea = 3;
                        mandelbrotSet.ScaleArea = 3;
                        break;
                    case ClickType.ZoomOut:
                        mandelbrotSet.X = mandelbrotSet.RecalculateHx(mouseX, width);
                        mandelbrotSet.Y = mandelbrotSet.RecalculateHy(mouseY, height);
                        mandelbrotSet.SizeArea *= mandelbrotSet.ScaleArea;
                        break;
                    default:
                        {
                            break;
                        }
                }

                mandelbrotSet.Canvas.Clear();
                mandelbrotSet.Canvas = new List<List<string>>(width);
                for (var i = 0; i < width; i++)
                {
                    mandelbrotSet.X = mandelbrotSet.RecalculateHx(i, width);
                    mandelbrotSet.Canvas.Add(new List<string>(height));
                    for (var j = 0; j < height; j++)
                    {
                        mandelbrotSet.Y = mandelbrotSet.RecalculateHy(j, height);
                        var z = new Complex(0, 0);

                        var countIteration = 0;
                        do
                        {
                            countIteration++;
                            z *= z;
                            z += new Complex(mandelbrotSet.X, mandelbrotSet.Y);
                            if (z.Magnitude > 2.0) break;
                        } while (countIteration < limitIteration);

                        var color = Color.FromRgba(
                            (byte)(countIteration % 8 * 16),
                            (byte)(countIteration % 4 * 32),
                            (byte)(countIteration % 2 * 64), 255);

                        var color1 = ComplexHeatMap(rand.Next(min, max), min, max, z, rand.NextDouble());
                        mandelbrotSet.Canvas[i].Add(color.ToString());
                    }
                }
                
                return Ok(_mapper.Map<FractalModel>(mandelbrotSet));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }

        public static Color ComplexHeatMap(decimal value, decimal min, decimal max, Complex z, double r)
        {
            var val = (value - min) / (max - min);
            return Color.FromRgba(
                255,
                Convert.ToByte(255 * val),
                Convert.ToByte(255 * (1 - val)),
                Convert.ToByte(255 * (z.Magnitude / r > 1 ? 1 : z.Magnitude / r))
            );
        }
    }
}

public enum ClickType
{
    ZoomIn,
    Middle,
    ZoomOut,
    Nonne
}
