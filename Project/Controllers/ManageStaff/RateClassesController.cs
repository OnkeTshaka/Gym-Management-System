using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Project.Models.ManageStaff;
using Microsoft.AspNet.Identity;

namespace Project.Controllers.ManageStaff
{
    public class RateClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RateClasses
        public async Task<ActionResult> Index()
        {
            var rateClasses = db.RateClasses.Include(r => r.Member);
            return View(await rateClasses.ToListAsync());
        }

        // GET: RateClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateClass rateClass = await db.RateClasses.FindAsync(id);
            if (rateClass == null)
            {
                return HttpNotFound();
            }
            return View(rateClass);
        }

        // GET: RateClasses/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Members, "ID", "Email");
            return View();
        }
        //Rating For class
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Classes(FormCollection form)
        {
            var comment = form["Comment"].ToString();
            var articleId = int.Parse(form["ArticleId"]);
            var rating = int.Parse(form["Rating"]);
            string CurrentUserName = User.Identity.GetUserName();
            RateClass rateClass = new RateClass()
            {
                ArticleId = articleId,
                Comments = comment,
                Rating = rating,
                ThisDateTime = DateTime.Now,


            };
            Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
            rateClass.ID = member.ID;
            rateClass.Member = member;

            db.RateClasses.Add(rateClass);
            db.SaveChanges();

            return RedirectToAction("Details", "Sessions", new { id = articleId });
        }

        // POST: RateClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ClassId,Comments,ThisDateTime,ArticleId,Rating,ID")] RateClass rateClass)
        {
            if (ModelState.IsValid)
            {
                db.RateClasses.Add(rateClass);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.Members, "ID", "Email", rateClass.ID);
            return View(rateClass);
        }

        // GET: RateClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateClass rateClass = await db.RateClasses.FindAsync(id);
            if (rateClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Members, "ID", "Email", rateClass.ID);
            return View(rateClass);
        }

        // POST: RateClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClassId,Comments,ThisDateTime,ArticleId,Rating,ID")] RateClass rateClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rateClass).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Members, "ID", "Email", rateClass.ID);
            return View(rateClass);
        }

        // GET: RateClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateClass rateClass = await db.RateClasses.FindAsync(id);
            if (rateClass == null)
            {
                return HttpNotFound();
            }
            return View(rateClass);
        }

        // POST: RateClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RateClass rateClass = await db.RateClasses.FindAsync(id);
            db.RateClasses.Remove(rateClass);
            await db.SaveChangesAsync();
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
