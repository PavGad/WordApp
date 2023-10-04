using WordApp.Shared.Enums;

namespace WordApp.Shared.Dtos.AuthDtos
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
    }
}
