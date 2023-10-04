namespace WordApp.Persistence.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
        public User User { get; set; }
    }
}
