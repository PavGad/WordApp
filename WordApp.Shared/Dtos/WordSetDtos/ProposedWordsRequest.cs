namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class ProposedWordsRequest
    {
        public Guid WordSetId { get; set; }
        public WordRequest[] Words { get; set; }
    }
}
