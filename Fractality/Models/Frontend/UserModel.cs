namespace Fractality.Models.Frontend
{
    public class UserModel : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
