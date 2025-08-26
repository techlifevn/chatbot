namespace Chatbot.Data.Entity
{
    public class BaseEntity
    {
        public int Order { get; set; }
        public bool IsDelete { get; set; }
        public bool IsStatus { get; set; }
        public string? CreateByUserId { get; set; }
        public DateTime? CreateOnDate { get; set; }
        public string? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
    }
}
