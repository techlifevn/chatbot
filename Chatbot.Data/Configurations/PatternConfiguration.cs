using Chatbot.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class PatternConfiguration : IEntityTypeConfiguration<Pattern>
    {
        public void Configure(EntityTypeBuilder<Pattern> builder)
        {
            builder.ToTable("Patterns");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Embeddings).WithOne(x => x.Pattern).HasForeignKey(x => x.PatternId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
