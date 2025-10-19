using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Features.Recipe.Domain;

namespace PrzepisakApi.src.Database
{
    public class PrzepisakDbContext : DbContext, IPrzepisakDbContext
    {
        public PrzepisakDbContext(DbContextOptions<PrzepisakDbContext> options) : base(options)
        {
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrzepisakDbContext).Assembly);
        }
    }
}
