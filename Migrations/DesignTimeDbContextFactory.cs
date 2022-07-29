using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Migrations
{
    public class DesignTimeDbContextFactory
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../EWallet/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            string connectionString = configuration.GetConnectionString("DbConnectionString");
            builder.UseSqlServer(connectionString);
            return new AppDbContext(builder.Options);
        }
    }
}
