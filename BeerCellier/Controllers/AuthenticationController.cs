using BeerCellier.Models;
using System.Web.Mvc;
using BeerCellier.Entities;
using BeerCellier.Core;

namespace BeerCellier.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IPersistenceContext _persistenceContext;
        private readonly ISessionContext _sessionContext;

        public AuthenticationController(IPersistenceContext persistenceContext, ISessionContext sessionContext)
        {
            _persistenceContext = persistenceContext;
            _sessionContext = sessionContext;
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

            if (!_sessionContext.SignIn(model.Username, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Username or password is invalid.");
                return View(model);
            }

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

            _persistenceContext.Add(user);
            _persistenceContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _sessionContext.signOut();            
            return RedirectToAction("Index", "Home");
        }
    }
}