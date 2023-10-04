namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class WordRequest
    {
        public string TargetWord { get; set; }
        public string OriginalWord { get; set; }
        public string TargetContext { get; set; }
        public string OriginalContext { get; set; }
        public string Definition { get; set; }
    }
}
