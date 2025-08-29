using Chatbot.Data.Configurations;
using Chatbot.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Chatbot.Data.EF
{
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName!.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.ApplyConfiguration(new IntentConfiguration());
            builder.ApplyConfiguration(new PatternConfiguration());
            builder.ApplyConfiguration(new ResponseConfiguration());
            builder.ApplyConfiguration(new EmbeddingConfiguration());
            builder.ApplyConfiguration(new ConversationLogConfiguration());
            builder.ApplyConfiguration(new SynonymConfiguration());
            builder.ApplyConfiguration(new KeywordBoostConfiguration());
        }


        public DbSet<Intent> Intents { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Embedding> Embeddings { get; set; }
        public DbSet<ConversationLog> ConversationHistories { get; set; }
        public DbSet<Synonym> Synonyms { get; set; }
        public DbSet<KeywordBoost> KeywordBoosts { get; set; }
    }
}
