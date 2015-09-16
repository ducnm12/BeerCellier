using System.Data.Entity;

namespace BeerFridge.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
    }
}