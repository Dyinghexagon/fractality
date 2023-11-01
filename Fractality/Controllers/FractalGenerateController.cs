using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;
using System.Text;

namespace Fractality.Controllers
{
    [ApiController]
    [Route("api/fractal/generate")]
    public class FractalGenerateController : ControllerBase
    {
        private readonly ILogger<FractalGenerateController> _logger;

        public FractalGenerateController(ILogger<FractalGenerateController> logger) {
            _logger = logger;
        }

        [HttpGet("mandelbrot-set")]
        public async Task<IActionResult> GenerateMandelbrotSet()
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
                var imageName = "mandelbrot-set.png";
                var path = $"./{baseUrl}/{imageName}";
                await image.SaveAsPngAsync(path);
                return Ok(imageName);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
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
                var imageName = "mandelbrot-set.png";
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

        [HttpGet("test")]
        public IActionResult Generate()
        {
            try
            {
                double realCoord, imagCoord;
                double realTemp, imagTemp, realTemp2, arg;
                int iterations;
                var result = new StringBuilder();

                for(imagCoord = 1.2; imagCoord >= -1.2; imagCoord -= 0.5)
                {
                    for(realCoord = -0.6; imagCoord <= 1.77; imagCoord += 0.03)
                    {
                        iterations = 0;
                        realTemp = realCoord;
                        imagTemp = imagCoord;
                        arg = (realCoord * realCoord) + (imagCoord * imagCoord);
                        while((arg < 4) && (iterations < 10))
                        {
                            realTemp2 = (realTemp * realTemp) - (imagTemp * imagTemp) + realCoord;
                            imagTemp = (2 * realTemp * imagTemp) + imagCoord;
                            realTemp = realTemp2;
                            arg = (realTemp * realTemp) + (imagTemp * imagTemp);
                            iterations++;
                        }
                        switch(iterations % 4)
                        {
                            case 0:
                                result.Append(".");
                                break;
                            case 1:
                                result.Append("o");
                                break;
                            case 2:
                                result.Append("0");
                                break;
                            case 3:
                                result.Append("@");
                                break;
                        }
                    }
                    result.Append("\n");
                }

                return Ok(result.ToString());
            } catch(Exception ex)
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
