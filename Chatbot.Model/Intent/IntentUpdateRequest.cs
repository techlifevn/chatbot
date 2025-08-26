namespace Chatbot.Model.Intent
{
    public class IntentUpdateRequest
    {
        public string Id { get; set; }
        public required string Name { get; set; }
        public required string Tag { get; set; }
        public required string DefaultResponse { get; set; }
        public int Priority { get; set; } = 50;
    }
}
