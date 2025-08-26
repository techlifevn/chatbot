using Chatbot.Model.Pattern;
using Chatbot.Model.Response;

namespace Chatbot.Model.Intent
{
    public class IntentVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string DefaultResponse { get; set; }
        public int Priority { get; set; } = 50;

        public List<PatternVm> Patterns { get; set; } = new List<PatternVm>();
        public List<ResponseVm> Responses { get; set; } = new List<ResponseVm>();
    }
}
