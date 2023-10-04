using WordApp.Shared.Enums;

namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class WordSetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public Level Level { get; set; }
        public Language OriginalLanguage { get; set; }
        public Language TargetLanguage { get; set; }
        public Guid CreatedById { get; set; }
        public Guid ConfirmedById { get; set; }
        public List<WordDto> Words { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
