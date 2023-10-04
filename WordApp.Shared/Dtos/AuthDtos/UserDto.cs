using WordApp.Shared.Enums;

namespace WordApp.Shared.Dtos.AuthDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
