namespace Chatbot.Data.Entity
{
    public class Response : BaseEntity
    {
        public int Id { get; set; }
        public required string ResponseText { get; set; }
        public int UsageCount { get; set; }

        public int IntentId { get; set; }

        public virtual Intent Intent { get; set; }
    }
}
