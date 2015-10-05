using BeerCellier.Core;
using BeerCellier.Entities;
using BeerCellier.Models;
using BeerCellier.Queries;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BeerCellier.Controllers
{
    [Authorize]
    public class CellarController : Controller
    { 
        private readonly IPersistenceContext _persistenceContext;
        private readonly ISessionContext _sessionContext;
        public int PageSize { get; }       

        public CellarController(IPersistenceContext persistenceContext, ISessionContext sessionContext, int pageSize)
        {
            _persistenceContext = persistenceContext;
            _sessionContext = sessionContext;
            this.PageSize = pageSize;
        }

        // GET: Cellar
        public ActionResult Index(string searchTerm, int? page)
        {
            var query = _persistenceContext.Query<Beer>().ForUser(_sessionContext.GetCurrentLoggedUser());            

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {               
                query = query.Where(b => b.Name.Contains(searchTerm));
            }
           
            query = query.OrderBy(b => b.Name);

            var pageNumber = page ?? 1;
            var model = query.ToViewModels().ToPagedList(pageNumber, PageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_BeerList", model);
            }

            return View(model);
        }

        // GET: Cellar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var beer = _persistenceContext.Query<Beer>().FindById(id.Value);
            
            if (beer == null)
            {
                return HttpNotFound();
            }

            var model = new BeerViewModel(beer);

            return View(model);
        }

        // GET: Cellar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cellar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Quantity")] CreateBeerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var beer = new Beer
                {
                    Quantity = model.Quantity,
                    Name = model.Name,
                    Owner = _sessionContext.GetCurrentLoggedUser()
                };

                _persistenceContext.Add(beer);
                _persistenceContext.SaveChanges();

                TempData["messageInfo"] = "Your beer has been added.";

                return RedirectToAction("Index");          
            }

            return View(model);
        }

        // GET: Cellar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var beer = _persistenceContext.Query<Beer>().FindById(id.Value);

            if (beer == null)
            {
                return HttpNotFound();
            }

            var model = new EditBeerViewModel(beer);

            return View(model);
        }

        // POST: Cellar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Quantity")] EditBeerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var beer = _persistenceContext.Query<Beer>().FindById(model.ID);

                if (beer == null)
                {
                    return HttpNotFound();
                }

                beer.Name = model.Name;
                beer.Quantity = model.Quantity;
                
                _persistenceContext.SaveChanges();

                TempData["messageInfo"] = "Your modifications has been saved.";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Cellar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var beer = _persistenceContext.Query<Beer>().FindById(id.Value);

            if (beer == null)
            {
                return HttpNotFound();
            }

            var model = new BeerViewModel(beer);            

            return View(model);
        }

        // POST: Cellar/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "ID")] DeleteBeerViewModel model)
        {
            Beer beer = _persistenceContext.Query<Beer>().FindById(model.ID);

            if (beer == null)
            {
                return HttpNotFound();
            }

            _persistenceContext.Remove(beer);
            _persistenceContext.SaveChanges();

            TempData["messageInfo"] = "Your beer has been deleted.";

            return RedirectToAction("Index");
        }

        // GET: Cellar/Drink/5
        public ActionResult Drink(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var beer = _persistenceContext.Query<Beer>().FindById(id.Value);

            if (beer == null)
            {
                return HttpNotFound();
            }

            var model = new BeerViewModel(beer);

            return View(model);
        }

        // POST: Cellar/Drink/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Drink([Bind(Include = "ID")] DrinkBeerViewModel model)
        {
            Beer beer = _persistenceContext.Query<Beer>().FindById(model.ID);

            if (beer == null)
            {
                return HttpNotFound();
            }

            beer.Quantity -= 1;

            if (beer.Quantity < 0)
            {
                beer.Quantity = 0;
            }
            
            _persistenceContext.SaveChanges();

            TempData["messageInfo"] = "Your modifications has been drinked.";

            return RedirectToAction("Index");
        }

        // GET: /Cellar/QuickSearch?term=
        public ActionResult QuickSearch(string term)
        {
            var model = _persistenceContext.Query<Beer>()
                .Search(_sessionContext.GetCurrentLoggedUser(), term)
                .Take(10)
                .Select(b => new {
                    label = b.Name
                });

            return Json(model, JsonRequestBehavior.AllowGet);
        }   
    }    
}
