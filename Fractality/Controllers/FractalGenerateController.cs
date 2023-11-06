using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;
using System.Numerics;
using System.Text.Json;

namespace Fractality.Controllers
{
    [ApiController]
    [Route("api/fractal")]
    public class FractalGenerateController : ControllerBase
    {
        private readonly ILogger<FractalGenerateController> _logger;
        private double hx, hy, x_, y_, n;
        private double sizeArea, scaleArea;

        public FractalGenerateController(ILogger<FractalGenerateController> logger) {
            _logger = logger;
            SetDefaultValues();
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

        [HttpGet("mandelbrot-set")]
        public IActionResult GenerateMandelbrotSet(
            [FromQuery] string clickType,
            [FromQuery] int width = 400,
            [FromQuery] int height = 600,
            [FromQuery] int limitIteration = 100,
            [FromQuery] int mouseX = 0,
            [FromQuery] int mouseY = 0
            )
        {
            try
            {
                switch (clickType)
                {
                    case "zoomIn":
                        hx = (hx - sizeArea / 2) + mouseX * (sizeArea / width);
                        hy = (hy - sizeArea / 2) + mouseY * (sizeArea / height);
                        sizeArea /= scaleArea;
                        break;
                    case "Middle":
                        sizeArea = 3;
                        scaleArea = 3;
                        break;
                    case "zoomOut":
                        x_ = (hx - sizeArea / 2) + mouseX * (sizeArea / width);
                        y_ = (hy - sizeArea / 2) + mouseY * (sizeArea / height);
                        sizeArea *= scaleArea;
                        break;
                    default:
                        {
                            SetDefaultValues();
                            break;
                        }
                }
                var canvas = new List<List<string>>();
                for (var x = 0; x < width; x++)
                {
                    x_ = (hx - sizeArea / 2) + x * (sizeArea / width);
                    canvas.Add(new List<string>());
                    for (var y = 0; y < height; y++)
                    {
                        y_ = (hy - sizeArea / 2) + y * (sizeArea / height);
                        var z = new Complex();

                        var countIteration = 0;
                        do
                        {
                            countIteration++;
                            z = z * z;
                            z += new Complex(x_, y_);

                            if (z.Magnitude > 2.0) break;
                        } while (countIteration < limitIteration);

                        var color = countIteration < limitIteration
                            ? Color.FromRgb(
                                (byte)(countIteration % 2 * 128),
                                (byte)(countIteration % 4 * 3),
                                (byte)(countIteration % 2 * 66))
                            : Color.FromRgb(74, 124, 194);
                        canvas[x].Add(color.ToString());
                    }
                }
                var baseUrl = "ClientApp/src/assets/images/genetare-images";
                var imageName = $"mandelbrot-set-{Guid.NewGuid()}.png";
                var path = $"./{baseUrl}/{imageName}";
                string jsonString = JsonSerializer.Serialize(canvas);

                return Ok(canvas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }

        private void SetDefaultValues()
        {
            hx = 0;
            hy = 0;
            n = 0;
            sizeArea = 3;
            scaleArea = 3;
        }

        private static Complex GetNormalizeComplex(int x, int y, int width, int height)
        {
            var a = (double)(x - (width / 2)) / (double)(width / 4);
            var b = (double)(y - (height / 2)) / (double)(height / 4);
            return new Complex(a, b);
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
