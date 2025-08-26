using Chatbot.Service;

namespace Chatbot.Web
{
    public class Chatbot
    {
        private readonly IChatbotService _chatbotService;

        public Chatbot(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }
    }
}
