using BeerCellier.Entities;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace BeerCellier.Queries
{
    public static class BeerQueries
    {
        public static Beer FindById(this IDbSet<Beer> query, int id)
        {
            return query.Include(b => b.Owner).SingleOrDefault(b => b.ID == id);
        }

        public static IQueryable<Beer> ForUser(this IQueryable<Beer> query, IPrincipal user)
        {
            var username = getUsername(user);
            return query.Where(b => b.Owner.Username == username);
        }

        public static IQueryable<Beer> Search(this IQueryable<Beer> query, IPrincipal user, string term)
        {
            return query.ForUser(user)
                .Where(b => b.Name.StartsWith(term));
        }

        private static string getUsername(IPrincipal user)
        {
            var claimIdentity = user.Identity as ClaimsIdentity;
            return claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}