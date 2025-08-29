using System.ComponentModel.DataAnnotations;

namespace Chatbot.Model.User
{
    public class UserSignRequest
    {
        [Display(Name = "UserName", Prompt = "UserName")]
        public string UserName { get; set; }
        [Display(Name = "Password", Prompt = "Password")]
        public string Password { get; set; }
    }
}
