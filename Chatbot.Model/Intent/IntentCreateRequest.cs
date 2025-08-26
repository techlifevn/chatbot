namespace Chatbot.Model.Intent
{
    public class IntentCreateRequest
    {
        public required string Name { get; set; }
        public required string Tag { get; set; }
        public required string DefaultResponse { get; set; }
        public int Priority { get; set; } = 50;
        public string UserId { get; set; }
    }
}
