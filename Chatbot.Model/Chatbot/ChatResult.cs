using Chatbot.Model.Intent;

namespace Chatbot.Model.Chatbot
{
    public class ChatResult
    {
        public string Response { get; set; } = string.Empty;
        public IntentVm? MatchedIntent { get; set; }
        public double Confidence { get; set; }
        public List<(IntentVm intent, double score)> TopCandidates { get; set; } = new();
    }
}
