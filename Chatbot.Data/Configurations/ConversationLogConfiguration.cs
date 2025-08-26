using Chatbot.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class ConversationLogConfiguration : IEntityTypeConfiguration<ConversationLog>
    {
        public void Configure(EntityTypeBuilder<ConversationLog> builder)
        {
            builder.ToTable("ConversationLogs");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.SessionId);
        }
    }
}
