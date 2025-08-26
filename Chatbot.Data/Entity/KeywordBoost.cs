namespace Chatbot.Data.Entity
{
    public class KeywordBoost : BaseEntity
    {
        public int Id { get; set; }
        public required string Keyword { get; set; }
        public double Boost { get; set; }

        public int IntentId { get; set; }

        public virtual Intent Intent { get; set; }
    }
}
