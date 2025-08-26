namespace Chatbot.Model.Embedding
{
    public class EmbeddingVm
    {
        public int Id { get; set; }
        public byte[] EmbeddingData { get; set; }

        public int IntentId { get; set; }
        public int PatternId { get; set; }
    }
}
