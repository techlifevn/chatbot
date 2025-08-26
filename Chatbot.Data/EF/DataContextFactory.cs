using Chatbot.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Chatbot.Data.EF
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString(SystemConstants.ConnectionSqlServer);

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
