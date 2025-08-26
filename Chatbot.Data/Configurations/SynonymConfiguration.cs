using Chatbot.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class SynonymConfiguration : IEntityTypeConfiguration<Synonym>
    {
        public void Configure(EntityTypeBuilder<Synonym> builder)
        {
            builder.ToTable("Synonyms");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.SynonymText);
        }
    }
}
