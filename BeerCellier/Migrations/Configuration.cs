namespace BeerCellier.Migrations
{
    using Core;
    using Entities;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<PersistenceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PersistenceContext context)
        {
            var adminUser = new User("admin", "p@ssw0rd");

            context.Users.AddOrUpdate(u => u.Username, adminUser);

            context.Beers.AddOrUpdate(b => b.Name,
                new Beer { Name = "Orval", Quantity = 6, Owner = adminUser },
                new Beer { Name = "La Chouffe", Quantity = 3, Owner = adminUser },
                new Beer { Name = "The Trooper", Quantity = 6, Owner = adminUser },
                new Beer { Name = "London Porter", Quantity = 6, Owner = adminUser },
                new Beer { Name = "Pabst Blue Ribbon", Quantity = 6, Owner = adminUser },
                new Beer { Name = "La Saison du Tracteur", Quantity = 3, Owner = adminUser },
                new Beer { Name = "La Fin du Monde", Quantity = 2, Owner = adminUser },
                new Beer { Name = "Les Trois Mousquetaires S.S. Pale Ale Americaine", Quantity = 4, Owner = adminUser }
            );
        }
    }
}
