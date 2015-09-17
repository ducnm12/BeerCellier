using BeerFridge.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BeerCellier.Controllers
{
    public class CellarController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Cellar
        public ActionResult Index()
        {
            return View(db.Beers.ToList());
        }

        // GET: Cellar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
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
        public ActionResult Create([Bind(Include = "ID,Name,Quantity")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                db.Beers.Add(beer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beer);
        }

        // GET: Cellar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Cellar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Quantity")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beer);
        }

        // GET: Cellar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
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
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Cellar/Drink/5
        [HttpPost, ActionName("Drink")]
        [ValidateAntiForgeryToken]
        public ActionResult DrinkConfirmed(int id)
        {
            Beer beer = db.Beers.Find(id);
            beer.Quantity -= 1;

            if (beer.Quantity < 0)
            {
                beer.Quantity = 0;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        // GET: Cellar/Search?searchTerm=
        public ActionResult Search(string searchTerm)
        {
            var query = db.Beers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Name.Contains(searchTerm));
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_BeerList", query.AsEnumerable());
            }

            return View(query.AsEnumerable());
        }

        // GET: /Cellar/QuickSearch?term=
        public ActionResult QuickSearch(string term)
        {
            var model = db.Beers
                .Where(b => b.Name.StartsWith(term))
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
