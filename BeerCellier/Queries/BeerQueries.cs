using BeerCellier.Entities;
using System.Data.Entity;
using System.Linq;

namespace BeerCellier.Queries
{
    public static class BeerQueries
    {
        public static Beer FindById(this IQueryable<Beer> query, int id)
        {
            return query.Include(b => b.Owner).SingleOrDefault(b => b.ID == id);
        }

        public static IQueryable<Beer> ForUser(this IQueryable<Beer> query, User user)
        {            
            return query.Where(b => b.Owner.ID == user.ID);
        }

        public static IQueryable<Beer> Search(this IQueryable<Beer> query, User user, string term)
        {
            return query.ForUser(user)
                .Where(b => b.Name.StartsWith(term));
        }
    }
}