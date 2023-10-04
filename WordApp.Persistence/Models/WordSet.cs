using WordApp.Shared.Enums;

namespace WordApp.Persistence.Models
{
    public class WordSet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool Confirmed { get; set; }
        public DateTimeOffset? ConfirmedOn { get; set; }
        public string CoverImageUrl { get; set; }
        public Level Level { get; set; }
        public Guid? ConfirmedById { get; set; }
        public User? ConfirmedBy { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public Language OriginalLanguage { get; set; }
        public Language TargetLanguage { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<ProposedWord> ProposedWords { get; set; }
        public ICollection<Word> Words { get; set; }
    }
}
