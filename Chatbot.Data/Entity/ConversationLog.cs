namespace Chatbot.Data.Entity
{
    public class ConversationLog : BaseEntity
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string UserInput { get; set; }
        public string BotResponse { get; set; }
        public float Confidence { get; set; }

        public int IntentId { get; set; }

        public virtual Intent Intent { get; set; }
    }
}
