namespace BeerCellier.Migrations
{
    using Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDbContext context)
        {
            context.Beers.AddOrUpdate(b => b.Name,
                new Beer { Name = "Orval", Quantity = 6 },
                new Beer { Name = "La Chouffe", Quantity = 6 },
                new Beer { Name = "The Trooper", Quantity = 6 },
                new Beer { Name = "London Porter", Quantity = 6 },
                new Beer { Name = "Pabst Blue Ribbon", Quantity = 6 }
            );
        }
    }
}
