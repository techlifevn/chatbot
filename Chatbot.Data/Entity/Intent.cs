namespace Chatbot.Data.Entity
{
    /// <summary>
    /// Ý định
    /// </summary>
    public class Intent : BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Tag { get; set; }
        public required string DefaultResponse { get; set; }
        public int Priority { get; set; } = 50;

        public ICollection<Pattern> Patterns { get; set; }
        public ICollection<Response> Responses { get; set; }
        public ICollection<Embedding> Embeddings { get; set; }
        public ICollection<ConversationLog> ConversationLogs { get; set; }
        public ICollection<KeywordBoost> KeywordBoosts { get; set; }
    }
}
