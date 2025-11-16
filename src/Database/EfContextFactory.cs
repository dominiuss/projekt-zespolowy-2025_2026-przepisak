using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PrzepisakApi.src.Database;

namespace miejsce.api.src.Data
{
    public class EfContextFactory : IDesignTimeDbContextFactory<EfContext>
    {
        public EfContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<EfContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new EfContext(optionsBuilder.Options);
        }
    }
}
