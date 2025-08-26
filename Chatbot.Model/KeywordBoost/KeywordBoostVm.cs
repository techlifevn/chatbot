namespace Chatbot.Model.KeywordBoost
{
    public class KeywordBoostVm
    {
        public int Id { get; set; }
        public required string Keyword { get; set; }
        public double Boost { get; set; }
        public bool IsStatus { get; set; }
        public int IntentId { get; set; }
    }
}
