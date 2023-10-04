namespace WordApp.Persistence.Models
{
    public class Complaint
    {
        public Guid Id { get; set; }
        public ComplaintReason? Reason { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public WordSet WordSet { get; set; }
        public User User { get; set; }
    }
}
