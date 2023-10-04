namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class WordSetUpdateRequest
    {
        public WordSetDto WordSet { get; set; }
        public List<WordRequest> NewWords { get; set; }
    }
}
