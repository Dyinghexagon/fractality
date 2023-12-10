using System.Threading;

namespace Fractality.Models.Backend
{
    public class Fractal : Entity
    {
        public List<List<string>> Canvas { get; set; } = new List<List<string>>();
    }
}
