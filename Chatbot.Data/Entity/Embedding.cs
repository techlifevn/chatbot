namespace Chatbot.Data.Entity
{
    public class Embedding
    {
        public int Id { get; set; }
        public byte[] EmbeddingData { get; set; }

        public int IntentId { get; set; }
        public int PatternId { get; set; }

        public virtual Intent Intent { get; set; }
        public virtual Pattern Pattern { get; set; }
    }
}
