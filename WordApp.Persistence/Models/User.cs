using WordApp.Shared.Enums;

namespace WordApp.Persistence.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<WordSet> CreatedWordSets { get; set; }
        public ICollection<WordSet> ConfirmedWordSets { get; set; }
        public ICollection<Word> Words { get; set; }
        public ICollection<UserWord> UserWords { get; set; }
    }
}
