using Microsoft.AspNet.Identity;
using Project.Models;
using Project.Models.ManageStaff;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.Admin
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            DashboardViewModel DBVM = new DashboardViewModel
            {
                Trainers = db.Trainer.Count(),
                Drivers = db.Drivers.Count(),
                Reasons = db.Reasons.Count(),
                Reviews = db.CommentsRatings.Count(),
                Users = db.Members.Count(),
                Products = db.Products.Count(),
                Categories = db.Categories.Count(),
                Orders = db.Orders.Count(),
                ReturnItems = db.ReturnItems.Count(),
                Packages = db.Packages.Count(),
                Classes = db.Sessions.Count(),
                ContactUsForms = db.ContactUsForms.ToList(),
                MembersWithPlan = db.Membership.Count(),
                Members = db.Members.ToList()
            };
            return View(DBVM);
        }
        public ActionResult AdminProfile()
        {
            var AdminId = User.Identity.GetUserId();
            if (AdminId != null)
            {
                ApplicationUser admin = db.Users.SingleOrDefault(c => c.Id == AdminId);
                return View(admin);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        //REVIEWS POSTED BY MEMBERS
        public ActionResult Reviews()
        {
            var reviews = db.CommentsRatings.OrderByDescending(r => r.ThisDateTime).Take(50).ToList();
            return View(reviews);
        }

        public ActionResult DeleteReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentsRating commentsRating = db.CommentsRatings.Find(id);
            if (commentsRating == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.CommentsRatings.Remove(commentsRating);
                db.SaveChanges();
                return RedirectToAction("Reviews");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


        }

        // GET: ContactUs
        public ActionResult ContactForms()
        {
            return View(db.ContactUsForms.OrderByDescending(c => c.Id).ToList());
        }
        public ActionResult ViewContactForm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUsForm contactUsForm = db.ContactUsForms.SingleOrDefault(c => c.Id == id);
            if (contactUsForm == null)
            {
                return HttpNotFound();
            }
            return View(contactUsForm);
        }
        // GET: ContactUsForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUsForm contactUsForm = db.ContactUsForms.SingleOrDefault(c => c.Id == id);
            if (contactUsForm == null)
            {
                return HttpNotFound();
            }
            return View(contactUsForm);
        }

        // POST: ContactUsForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactUsForm contactUsForm = db.ContactUsForms.Find(id);
            db.ContactUsForms.Remove(contactUsForm);
            db.SaveChanges();
            return RedirectToAction("ContactForms");
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