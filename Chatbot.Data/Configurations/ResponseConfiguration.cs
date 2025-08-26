using Chatbot.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class ResponseConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder.ToTable("Responses");

            builder.HasKey(x => x.Id);
        }
    }
}
