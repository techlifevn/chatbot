namespace Chatbot.Data.Entity
{
    public class Synonym : BaseEntity
    {
        public int Id { get; set; }
        public required string MainTerm { get; set; }
        public required string SynonymText { get; set; }
    }
}
