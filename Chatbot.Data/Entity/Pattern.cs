namespace Chatbot.Data.Entity
{
    /// <summary>
    /// Mẫu câu hỏi
    /// </summary>
    public class Pattern : BaseEntity
    {
        public int Id { get; set; }
        public required string PatternText { get; set; }

        public int IntentId { get; set; }

        public virtual Intent Intent { get; set; }
        public ICollection<Embedding> Embeddings { get; set; }
    }
}
