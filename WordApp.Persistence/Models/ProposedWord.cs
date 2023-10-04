namespace WordApp.Persistence.Models
{
    public class ProposedWord
    {
        public Guid Id { get; set; }
        public string TargetWord { get; set; }
        public string OriginalWord { get; set; }
        public string TargetContext { get; set; }
        public string OriginalContext { get; set; }
        public string Definition { get; set; }
        public User User { get; set; }
        public WordSet WordSet { get; set; }
    }
}
