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
    public class RateBookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RateBookings
        public async Task<ActionResult> Index()
        {
            var rateBookings = db.RateBookings.Include(r => r.Member);
            return View(await rateBookings.ToListAsync());
        }

        // GET: RateBookings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateBooking rateBooking = await db.RateBookings.FindAsync(id);
            if (rateBooking == null)
            {
                return HttpNotFound();
            }
            return View(rateBooking);
        }

        // GET: RateBookings/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Members, "ID", "Email");
            return View();
        }
        //Rating for the class booked by user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(FormCollection form)
        {
            var comment = form["Comment"].ToString();
            var articleId = int.Parse(form["ArticleId"]);
            var rating = int.Parse(form["Rating"]);
            string CurrentUserName = User.Identity.GetUserName();
            RateBooking rateBooking = new RateBooking()
            {
                ArticleId = articleId,
                Comments = comment,
                Rating = rating,
                ThisDateTime = DateTime.Now,


            };
            Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
            rateBooking.ID = member.ID;
            rateBooking.Member = member;

            db.RateBookings.Add(rateBooking);
            db.SaveChanges();

            return RedirectToAction("Details", "BookingTrainers", new { id = articleId });
        }
        // POST: RateBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookingId,Comments,ThisDateTime,ArticleId,Rating,ID")] RateBooking rateBooking)
        {
            if (ModelState.IsValid)
            {
                db.RateBookings.Add(rateBooking);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.Members, "ID", "Email", rateBooking.ID);
            return View(rateBooking);
        }

        // GET: RateBookings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateBooking rateBooking = await db.RateBookings.FindAsync(id);
            if (rateBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Members, "ID", "Email", rateBooking.ID);
            return View(rateBooking);
        }

        // POST: RateBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookingId,Comments,ThisDateTime,ArticleId,Rating,ID")] RateBooking rateBooking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rateBooking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Members, "ID", "Email", rateBooking.ID);
            return View(rateBooking);
        }

        // GET: RateBookings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateBooking rateBooking = await db.RateBookings.FindAsync(id);
            if (rateBooking == null)
            {
                return HttpNotFound();
            }
            return View(rateBooking);
        }

        // POST: RateBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RateBooking rateBooking = await db.RateBookings.FindAsync(id);
            db.RateBookings.Remove(rateBooking);
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
