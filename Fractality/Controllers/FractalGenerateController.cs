using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;

namespace Fractality.Controllers
{
    [ApiController]
    [Route("api/fractal")]
    public class FractalGenerateController : ControllerBase
    {
        [HttpGet("generate/mandelbrot-set")]
        public async Task<IActionResult> GenerateMandelbrotSet()
        {
            var rand = new Random();

            int width = rand.Next(600, 1000);
            int height = rand.Next(400, 800);

            using var image = new Image<Rgba32>(width, height);
            PatternBrush brush = Brushes.Horizontal(
                GetRandomColor(), 
                GetRandomColor());
            PatternPen pen = Pens.DashDot(GetRandomColor(), rand.Next(0, 10));
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

        [HttpGet("generate/julia-set")]
        public async Task<IActionResult> GenerateJuliaSet()
        {
            var rand = new Random();

            int width = rand.Next(600, 1000);
            int height = rand.Next(400, 800);

            using var image = new Image<Rgba32>(width, height);
            PatternBrush brush = Brushes.Horizontal(
                GetRandomColor(),
                GetRandomColor());
            PatternPen pen = Pens.DashDot(GetRandomColor(), rand.Next(0, 10));
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

        [HttpGet("generate/douady-rabbit")]
        public async Task<IActionResult> GenerateDouadyRabbit()
        {
            var rand = new Random();

            int width = rand.Next(600, 1000);
            int height = rand.Next(400, 800);

            using var image = new Image<Rgba32>(width, height);
            PatternBrush brush = Brushes.Horizontal(
                GetRandomColor(),
                GetRandomColor());
            PatternPen pen = Pens.DashDot(GetRandomColor(), rand.Next(0, 10));
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
            var imageName = "douady-rabbit.png";
            var path = $"./{baseUrl}/{imageName}";
            await image.SaveAsPngAsync(path);
            return Ok(imageName);
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
