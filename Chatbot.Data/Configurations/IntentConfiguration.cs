using Chatbot.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class IntentConfiguration : IEntityTypeConfiguration<Intent>
    {
        public void Configure(EntityTypeBuilder<Intent> builder)
        {
            builder.ToTable("Intents");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Patterns).WithOne(x => x.Intent).HasForeignKey(x => x.IntentId);
            builder.HasMany(x => x.Responses).WithOne(x => x.Intent).HasForeignKey(x => x.IntentId);
            builder.HasMany(x => x.Embeddings).WithOne(x => x.Intent).HasForeignKey(x => x.IntentId);
            builder.HasMany(x => x.ConversationLogs).WithOne(x => x.Intent).HasForeignKey(x => x.IntentId);
        }
    }
}
