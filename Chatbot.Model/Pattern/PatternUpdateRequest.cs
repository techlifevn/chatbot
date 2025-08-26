using System.ComponentModel.DataAnnotations;

namespace Chatbot.Model.Pattern
{
    public class PatternUpdateRequest
    {
        public required string Id { get; set; }

        [Display(Name = "Câu hỏi")]
        [Required(ErrorMessage = "Vui lòng nhập câu hỏi")]
        public required string PatternText { get; set; }

        [Display(Name = "Nhãn")]
        [Required(ErrorMessage = "Vui lòng chọn nhãn")]
        public required string IntentId { get; set; }

        public string UserId { get; set; }

    }
}
