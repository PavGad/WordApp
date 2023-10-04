using System.Text.Json.Serialization;

namespace WordApp.Shared.Dtos.AuthDtos
{
    public class LogInResponse
    {
        public string AccessToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public LogInResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
