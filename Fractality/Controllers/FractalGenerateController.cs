using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;

namespace Fractality.Controllers
{
    [ApiController]
    [Route("api/fractal")]
    public class FractalGenerateController : ControllerBase
    {
        [HttpGet("generate")]
        public void Generate()
        {
            const int width = 600;
            const int height = 400;
            using var image = new Image<Rgba32>(width, height);
            var rand = new Random();
            PatternBrush brush = Brushes.Horizontal(Color.Red, Color.Blue);
            PatternPen pen = Pens.DashDot(Color.Green, 5);
            Star star = new(x: 100.0f, y: 100.0f, prongs: 5, innerRadii: 20.0f, outerRadii: 30.0f);
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
            image.Save("test.png");
        }
    }
}
