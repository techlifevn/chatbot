using System.ComponentModel.DataAnnotations;

namespace Chatbot.Model.Response
{
    public class ResponseUpdateRequest
    {
        public string Id { get; set; }
        [Display(Name = "Câu trả lời")]
        [Required(ErrorMessage = "Vui lòng nhập câu trả lời")]
        public required string ResponseText { get; set; }

        [Display(Name = "Nhãn")]
        [Required(ErrorMessage = "Vui lòng chọn nhãn")]
        public required string IntentId { get; set; }

        public string UserId { get; set; }
    }
}
