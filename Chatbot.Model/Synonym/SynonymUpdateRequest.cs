namespace Chatbot.Model.Synonym
{
    public class SynonymUpdateRequest
    {
        public string Id { get; set; }
        public required string MainTerm { get; set; }
        public required string SynonymText { get; set; }

        public string UserId { get; set; }
    }
}
