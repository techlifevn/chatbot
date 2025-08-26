namespace Chatbot.Model.KeywordBoost
{
    public class KeywordBoostCreateRequest
    {
        public required string Keyword { get; set; }
        public double Boost { get; set; }
        public string UserId { get; set; }
    }
}
