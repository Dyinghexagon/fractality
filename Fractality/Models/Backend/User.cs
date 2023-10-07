namespace Fractality.Models.Backend
{
    public class User : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
