using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Project.Models.ManageStaff;
using PagedList;

namespace Project.Controllers.ManageStaff
{
    public class SessionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sessions
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceParm = sortOrder == "Price" ? "price" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var sessions = from s in db.Sessions
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sessions = sessions.Where(s => s.SessionType.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sessions = sessions.OrderByDescending(s => s.SessionType);
                    break;
                case "Price":
                    sessions = sessions.OrderBy(s => s.Price);
                    break;
                case "price":
                    sessions = sessions.OrderByDescending(s => s.Price);
                    break;
                default:  // Name ascending 
                    sessions = sessions.OrderBy(s => s.SessionType);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(sessions.ToPagedList(pageNumber, pageSize));
        }

            // GET: Sessions/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = id.Value;
            var comments = db.RateClasses.Where(d => d.ArticleId.Equals(id.Value)).ToList();
            ViewBag.Comments = comments;

            var ratings = db.RateClasses.Where(d => d.ArticleId.Equals(id.Value)).ToList();
            if (ratings.Count() > 0)
            {
                var ratingSum = ratings.Sum(d => d.Rating.Value);
                ViewBag.RatingSum = ratingSum;
                var ratingCount = ratings.Count();
                ViewBag.RatingCount = ratingCount;
            }
            else
            {
                ViewBag.RatingSum = 0;
                ViewBag.RatingCount = 0;
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            Session b1 = new Session();
            return View(b1);
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionID,SessionType,Icon,Description,Price")] Session session, HttpPostedFileBase image1)
        {
            var db = new ApplicationDbContext();
            if (image1 != null)
            {
                session.Icon = new byte[image1.ContentLength];
                image1.InputStream.Read(session.Icon, 0, image1.ContentLength);
            }
            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SessionID,SessionType,Icon,Description,Price")] Session session,HttpPostedFileBase image1)
        {
            byte[] data = null;
            data = new byte[image1.ContentLength];
            image1.InputStream.Read(data, 0, image1.ContentLength);
            session.Icon = data;
            session.Icon = image1 != null ? data : session.Icon;
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
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
