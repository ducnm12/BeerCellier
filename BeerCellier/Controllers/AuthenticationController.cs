using BeerCellier.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using BeerCellier.Entities;

namespace BeerCellier.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AppDbContext db;

        public AuthenticationController(AppDbContext context)
        {
            db = context;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]       
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password")] LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Users
                .Where(u => u.Username == model.Username)
                .SingleOrDefault();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or password is invalid.");
                return View(model);
            }

            if (!user.IsPasswordValid(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Username or password is invalid.");
                return View(model);
            }
            
            var loginClaim = new Claim(ClaimTypes.NameIdentifier, model.Username);
            var claimsIdentity = new ClaimsIdentity(new[] { loginClaim }, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;

            authenticationManager.SignIn(claimsIdentity);

            if (Url.IsLocalUrl(ViewBag.ReturnUrl))
            {
                return Redirect(ViewBag.ReturnUrl);
            }            

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password")] RegisterViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User(model.Username, model.Password);

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;

            authenticationManager.SignOut();
            
            return RedirectToAction("Index", "Home");
        }
    }
}