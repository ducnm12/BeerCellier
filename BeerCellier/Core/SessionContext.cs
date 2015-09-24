using BeerCellier.Entities;
using BeerCellier.Queries;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace BeerCellier.Core
{
    public interface ISessionContext
    {        
        bool SignIn(string username, string password);
        void signOut();
        User GetCurrentLoggedUser();
    }

    public class SessionContext : ISessionContext
    {
        private readonly IPersistenceContext _persistenceContext;

        public SessionContext(IPersistenceContext persistenceContext)
        {
            _persistenceContext = persistenceContext;
        }

        public User GetCurrentLoggedUser()
        {
            var user = HttpContext.Current.User;
            return _persistenceContext.Query<User>().FindByUsername(getUsername(user));
        }       

        public bool SignIn(string username, string password)
        {
            var request = HttpContext.Current.Request;

            if (!IsValidCredentials(username, password))
            {
                return false;
            }

            var loginClaim = new Claim(ClaimTypes.NameIdentifier, username);
            var claimsIdentity = new ClaimsIdentity(new[] { loginClaim }, DefaultAuthenticationTypes.ApplicationCookie);
            
            var authenticationManager = request.GetOwinContext().Authentication;
            authenticationManager.SignIn(claimsIdentity);

            return true;
        }

        public void signOut()
        {
            var request = HttpContext.Current.Request;
            var authenticationManager = request.GetOwinContext().Authentication;

            authenticationManager.SignOut();
        }

        private bool IsValidCredentials(string username, string password)
        {
            var user = _persistenceContext.Query<User>()
                .Where(u => u.Username == username)
                .SingleOrDefault();

            if (user == null)
            {
                return false;
            }

            if (!user.IsPasswordValid(password))
            {
                return false;
            }

            return true;
        }

        private static string getUsername(IPrincipal user)
        {
            var claimIdentity = user.Identity as ClaimsIdentity;
            return claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}