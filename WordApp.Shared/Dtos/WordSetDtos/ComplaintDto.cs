namespace WordApp.Shared.Dtos.WordSetDtos
{
    public class ComplaintDto
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public WordSetDto WordSet { get; set; }
        public Guid UserId { get; set; }
    }
}
