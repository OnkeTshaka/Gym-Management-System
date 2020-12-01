using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Project.Models;
using Project.Models.Essentials;

namespace Project.Controllers.Essentials
{
    public class PackageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Package
        public ActionResult Index()
        {
            return View(db.Packages.ToList());
        }
        public ActionResult Offers(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 3;
            var List = db.Packages.OrderByDescending(x => x.PackId).ToPagedList(pageNumber, pageSize);
            return View(List);
        }

        // GET: Package/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // GET: Package/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Package/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackId,PackageType,Description,Monthly_Fee,Joining_Fee,TotalCost,Duration")] Package package)
        {
            if (ModelState.IsValid)
            {
                package.TotalCost = package.CalcTotalCost();
                db.Packages.Add(package);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(package);
        }

        // GET: Package/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // POST: Package/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackId,PackageType,Description,Monthly_Fee,Joining_Fee,TotalCost,Duration")] Package package)
        {
            if (ModelState.IsValid)
            {
                package.TotalCost = package.CalcTotalCost();
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(package);
        }

        // GET: Package/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // POST: Package/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Package package = db.Packages.Find(id);
            db.Packages.Remove(package);
            db.SaveChanges();
            return RedirectToAction("Index");
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
