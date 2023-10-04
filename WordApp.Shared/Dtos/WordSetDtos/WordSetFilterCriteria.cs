using WordApp.Shared.Enums;

namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class WordSetFilterCriteria
    {
        public int PageNumber { get; set; }
        public string SearchText { get; set; }
        public List<Level> Levels { get; set; }
    }
}
