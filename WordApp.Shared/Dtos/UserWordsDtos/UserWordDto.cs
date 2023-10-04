using WordApp.Shared.Enums;

namespace WordApp.Shared.Dtos.UserWordsDtos
{
    public class UserWordDto
    {
        public Guid Id { get; set; }
        public string TargetWord { get; set; }
        public string OriginalWord { get; set; }
        public string TargetContext { get; set; }
        public string OriginalContext { get; set; }
        public string Definition { get; set; }
        public DateTimeOffset RepeatOn { get; set; }
        public string Theme { get; set; }
        public int Stage { get; set; }
        public Language OriginalLanguage { get; set; }
        public Language TargetLanguage { get; set; }
    }
}
