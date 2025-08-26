namespace Chatbot.Model.KeywordBoost
{
    public class KeywordBoostUpdateRequest
    {
        public string Id { get; set; }
        public required string Keyword { get; set; }
        public double Boost { get; set; }
        public string UserId { get; set; }
    }
}
