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
        private AppDbContext db = new AppDbContext();

        // GET: Cellar
        public ActionResult Index(string searchTerm, int? page)
        {
            var query = db.Beers.ForUser(User);            

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {               
                query = query.Where(b => b.Name.Contains(searchTerm));
            }
           
            query = query.OrderBy(b => b.Name);

            var pageNumber = page ?? 1;
            var model = query.ToViewModels().ToPagedList(pageNumber, 5);

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

            var beer = db.Beers.FindById(id.Value);
            
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
                    Owner = db.Users.FindUser(User)
                };

                db.Beers.Add(beer);
                db.SaveChanges();

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

            var beer = db.Beers.FindById(id.Value);

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
                var beer = db.Beers.FindById(model.ID);

                if (beer == null)
                {
                    return HttpNotFound();
                }

                beer.Name = model.Name;
                beer.Quantity = model.Quantity;

                db.Entry(beer).State = EntityState.Modified;
                db.SaveChanges();

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

            var beer = db.Beers.Find(id);

            if (beer == null)
            {
                return HttpNotFound();
            }

            var model = new BeerViewModel(beer);

            return View(model);
        }

        // POST: Cellar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Beer beer = db.Beers.Find(id);
            db.Beers.Remove(beer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Cellar/Drink/5
        public ActionResult Drink(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var beer = db.Beers.Find(id);

            if (beer == null)
            {
                return HttpNotFound();
            }

            var model = new BeerViewModel(beer);

            return View(model);
        }

        // POST: Cellar/Drink/5
        [HttpPost, ActionName("Drink")]
        [ValidateAntiForgeryToken]
        public ActionResult DrinkConfirmed(int id)
        {
            Beer beer = db.Beers.FindById(id);

            if (beer == null)
            {
                return HttpNotFound();
            }

            beer.Quantity -= 1;

            if (beer.Quantity < 0)
            {
                beer.Quantity = 0;
            }

            db.Entry(beer).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Cellar/QuickSearch?term=
        public ActionResult QuickSearch(string term)
        {
            var model = db.Beers
                .Search(User, term)
                .Take(10)
                .Select(b => new {
                    label = b.Name
                });

            return Json(model, JsonRequestBehavior.AllowGet);
        }        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }        
    }    
}
