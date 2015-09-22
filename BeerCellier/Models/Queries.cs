using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace BeerCellier.Models
{
    public static class Queries
    {
        public static IQueryable<Beer> ForUser(this IQueryable<Beer> query, IPrincipal user)
        {
            var username = getUsername(user);
            return query.Where(b => b.Owner.Username == username);
        }

        public static User FindUser(this IQueryable<User> query, IPrincipal user)
        {
            var username = getUsername(user);
            return query.SingleOrDefault(u => u.Username == username);
        }

        private static string getUsername(IPrincipal user)
        {
            var claimIdentity = user.Identity as ClaimsIdentity;
            return claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}