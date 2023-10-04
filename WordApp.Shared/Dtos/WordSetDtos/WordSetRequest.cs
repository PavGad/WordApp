using WordApp.Shared.Enums;

namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class WordSetRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverImageBase64 { get; set; }
        public Level Level { get; set; }
        public Language OriginalLanguage { get; set; }
        public Language TargetLanguage { get; set; }
        public List<WordRequest> Words { get; set; }
    }
}
