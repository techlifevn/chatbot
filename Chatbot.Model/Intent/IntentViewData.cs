namespace Chatbot.Model.Intent
{
    public class IntentViewData
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string DefaultResponse { get; set; }
        public int Priority { get; set; }
        public int? PatternId { get; set; }
        public string PatternText { get; set; }
        public int? ResponseId { get; set; }
        public string ResponseText { get; set; }
        public int? EmbeddingId { get; set; }
        public byte[] EmbeddingData { get; set; }
    }
}
