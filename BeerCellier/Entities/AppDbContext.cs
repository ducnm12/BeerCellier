using System.Data.Entity;

namespace BeerCellier.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}