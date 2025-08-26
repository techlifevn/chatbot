using Chatbot.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class EmbeddingConfiguration : IEntityTypeConfiguration<Embedding>
    {
        public void Configure(EntityTypeBuilder<Embedding> builder)
        {
            builder.ToTable("Embeddings");
            builder.HasKey(x => x.Id);
        }
    }
}
