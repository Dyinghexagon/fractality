using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;
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

        [HttpGet("julia-set")]
        public async Task<IActionResult> GenerateJuliaSet()
        {
            try
            {
                var rand = new Random();

                int width = rand.Next(600, 1000);
                int height = rand.Next(400, 800);

                using var image = new Image<Rgba32>(width, height);
                PatternBrush brush = Brushes.Horizontal(
                    GetRandomColor(),
                    GetRandomColor());
                PatternPen pen = Pens.DashDot(GetRandomColor(), rand.Next(1, 10));
                Star star = new(
                    x: GetRandomFloatValueForRange(100, width),
                    y: GetRandomFloatValueForRange(100, height),
                    prongs: 5,
                    innerRadii: GetRandomFloatValueForRange(10, 30),
                    outerRadii: GetRandomFloatValueForRange(10, 30)
                );
                DrawingOptions options = new()
                {
                    GraphicsOptions = new()
                    {
                        ColorBlendingMode = PixelColorBlendingMode.Multiply
                    }
                };
                // Draws a star with horizontal red and blue hatching with a dash-dot pattern outline.
                image.Mutate(x => x.Fill(options, brush, star)
                                   .Draw(options, pen, star));
                var baseUrl = "ClientApp/src/assets/images/genetare-images";
                var imageName = "julia-set.png";
                var path = $"./{baseUrl}/{imageName}";
                await image.SaveAsPngAsync(path);
                return Ok(imageName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }

        [HttpPost("mandelbrot-set")]
        public IActionResult GenerateMandelbrotSet(
            [FromBody] FractalModel fractalModel,
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
                var fractal = _mapper.Map<Fractal>(fractalModel);
                switch (clickType)
                {
                    case ClickType.ZoomIn:
                        fractal.Hx = fractal.RecalculateHx(mouseX, width);
                        fractal.Hy = fractal.RecalculateHy(mouseY, height);
                        fractal.SizeArea /= fractal.ScaleArea;
                        break;
                    case ClickType.Middle:
                        fractal.SizeArea = 3;
                        fractal.ScaleArea = 3;
                        break;
                    case ClickType.ZoomOut:
                        fractal.X = fractal.RecalculateHx(mouseX, width);
                        fractal.Y = fractal.RecalculateHy(mouseY, height);
                        fractal.SizeArea *= fractal.ScaleArea;
                        break;
                    default:
                        {
                            break;
                        }
                }

                fractal.Canvas.Clear();
                for (var i = 0; i < width; i++)
                {
                    fractal.X = fractal.RecalculateHx(i, width);
                    fractal.Canvas.Add(new List<string>());
                    for (var j = 0; j < height; j++)
                    {
                        fractal.Y = fractal.RecalculateHy(j, height);
                        var z = new Complex(0, 0);

                        var countIteration = 0;
                        do
                        {
                            countIteration++;
                            z *= z;
                            z += new Complex(fractal.X, fractal.Y);
                            if (z.Magnitude > 2.0) break;
                        } while (countIteration < limitIteration);

                        var color = Color.FromRgba(
                            (byte)(countIteration % 8 * 16),
                            (byte)(countIteration % 4 * 32),
                            (byte)(countIteration % 2 * 64), 255);
                        fractal.Canvas[i].Add(color.ToString());
                    }
                }
                
                return Ok(_mapper.Map<FractalModel>(fractal));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }

        private static Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        }

        private static float GetRandomFloatValueForRange(int minValue, int maxValue)
        {
            var random = new Random();
            return random.NextSingle() * random.Next(minValue, maxValue);
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
