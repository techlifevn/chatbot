namespace Chatbot.Model.Response
{
    public class ResponseVm
    {
        public int Id { get; set; }
        public required string ResponseText { get; set; }
        public int UsageCount { get; set; }
        public int IntentId { get; set; }
    }
}
