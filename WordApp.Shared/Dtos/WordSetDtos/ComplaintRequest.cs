namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class ComplaintRequest
    {
        public int Reason { get; set; }
        public string Message { get; set; }
        public Guid WordSetId { get; set; }
    }
}
