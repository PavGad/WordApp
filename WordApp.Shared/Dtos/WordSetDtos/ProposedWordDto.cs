namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class ProposedWordDto
    {
        public Guid Id { get; set; }
        public string TargetWord { get; set; }
        public string OriginalWord { get; set; }
        public string TargetContext { get; set; }
        public string OriginalContext { get; set; }
        public string Definition { get; set; }
        public Guid UserId { get; set; }
        public Guid WordSetId { get; set; }
    }
}
