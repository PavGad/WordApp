using System.Text.Json.Serialization;

namespace WordApp.Shared.Dtos.AuthDtos
{
    public class SignUpResponse
    {
        public string AccessToken { get; set; }
        public Guid Id { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public SignUpResponse(string accessToken, string refreshToken, Guid id)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Id = id;
        }
    }
}
