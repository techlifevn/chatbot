using Chatbot.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class KeywordBoostConfiguration : IEntityTypeConfiguration<KeywordBoost>
    {
        public void Configure(EntityTypeBuilder<KeywordBoost> builder)
        {
            builder.ToTable("KeywordBoosts");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Intent).WithMany(x => x.KeywordBoosts).HasForeignKey(x => x.IntentId);
        }
    }
}
