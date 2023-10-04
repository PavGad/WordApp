namespace WordApp.Shared.Dtos.AuthDtos
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}
