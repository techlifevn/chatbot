namespace Chatbot.Model.Pattern
{
    public class PatternVm
    {
        public int Id { get; set; }
        public required string PatternText { get; set; }
        public int IntentId { get; set; }
    }
}
