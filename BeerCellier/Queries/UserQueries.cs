using BeerCellier.Entities;
using System.Linq;

namespace BeerCellier.Queries
{
    public static class UserQueries
    {
        public static User FindByUsername(this IQueryable<User> query, string username)
        {
            return query.SingleOrDefault(u => u.Username == username);
        }        
    }
}