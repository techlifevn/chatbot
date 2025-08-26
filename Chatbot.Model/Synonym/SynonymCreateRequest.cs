namespace Chatbot.Model.Synonym
{
    public class SynonymCreateRequest
    {
        public required string MainTerm { get; set; }
        public required string SynonymText { get; set; }

        public string UserId { get; set; }
    }
}
