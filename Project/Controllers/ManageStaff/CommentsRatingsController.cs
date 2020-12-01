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
using Microsoft.AspNet.Identity;

namespace Project.Controllers.ManageStaff
{
    public class CommentsRatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentsRatings
        public ActionResult Index()
        {
            return View(db.CommentsRatings.ToList());
        }

        // GET: CommentsRatings/Details/5
        public ActionResult Details(int? id)
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
            return View(commentsRating);
        }

        // GET: CommentsRatings/Create
        public ActionResult Create()
        {
            return View();
        }
        ////Rating For class
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Classes(FormCollection form)
        //{
        //    var comment = form["Comment"].ToString();
        //    var articleId = int.Parse(form["ArticleId"]);
        //    var rating = int.Parse(form["Rating"]);
        //    string CurrentUserName = User.Identity.GetUserName();
        //    CommentsRating commentsRating = new CommentsRating()
        //    {
        //        ArticleId = articleId,
        //        Comments = comment,
        //        Rating = rating,
        //        ThisDateTime = DateTime.Now,


        //    };
        //    Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
        //    commentsRating.ID = member.ID;
        //    commentsRating.Member = member;

        //    db.CommentsRatings.Add(commentsRating);
        //    db.SaveChanges();

        //    return RedirectToAction("Details", "Sessions", new { id = articleId });
        //}
        //Rating For the trainer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FormCollection form)
        {
            var comment = form["Comment"].ToString();
            var articleId = int.Parse(form["ArticleId"]);
            var rating = int.Parse(form["Rating"]);
            string CurrentUserName = User.Identity.GetUserName();
            CommentsRating commentsRating = new CommentsRating()
            {
                ArticleId = articleId,
                Comments = comment,
                Rating = rating,
                ThisDateTime = DateTime.Now,

            
        };
        Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
            commentsRating.ID = member.ID;
            commentsRating.Member = member;

            db.CommentsRatings.Add(commentsRating);
            db.SaveChanges();

            return RedirectToAction("Details", "Trainers", new { id = articleId });
        }
        //Rating for the class booked by user
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Book(FormCollection form)
        //{
        //    var comment = form["Comment"].ToString();
        //    var articleId = int.Parse(form["ArticleId"]);
        //    var rating = int.Parse(form["Rating"]);
        //    string CurrentUserName = User.Identity.GetUserName();
        //    CommentsRating commentsRating = new CommentsRating()
        //    {
        //        ArticleId = articleId,
        //        Comments = comment,
        //        Rating = rating,
        //        ThisDateTime = DateTime.Now,


        //    };
        //    Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
        //    commentsRating.ID = member.ID;
        //    commentsRating.Member = member;

        //    db.CommentsRatings.Add(commentsRating);
        //    db.SaveChanges();

        //    return RedirectToAction("Details", "BookingTrainers", new { id = articleId });
        //}
        // POST: CommentsRatings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,Comments,ThisDateTime,ArticleId,Rating")] CommentsRating commentsRating)
        {
            if (ModelState.IsValid)
            {
                db.CommentsRatings.Add(commentsRating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(commentsRating);
        }

        // GET: CommentsRatings/Edit/5
        public ActionResult Edit(int? id)
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
            return View(commentsRating);
        }

        // POST: CommentsRatings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,Comments,ThisDateTime,ArticleId,Rating")] CommentsRating commentsRating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentsRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commentsRating);
        }

        // GET: CommentsRatings/Delete/5
        public ActionResult Delete(int? id)
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
            return View(commentsRating);
        }

        // POST: CommentsRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommentsRating commentsRating = db.CommentsRatings.Find(id);
            db.CommentsRatings.Remove(commentsRating);
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
