using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;
using Project.Models.ManageStaff;
using Project.Models.Return;

namespace Project.Controllers.Return
{
    public class TermsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Terms
        public ActionResult Index()
        {
            return View(db.Terms.ToList());
        }

        // GET: Terms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terms terms = db.Terms.Find(id);
            if (terms == null)
            {
                return HttpNotFound();
            }
            return View(terms);
        }
        [Authorize]
        // GET: Terms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Terms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TermsID,Name,Email,Agreed")] Terms terms)
        {
            string CurrentUserName = User.Identity.GetUserName();

            Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
            terms.Name = member.Username;
            terms.Email = member.Email;
            if (terms.Agreed == true)
            {
                if (ModelState.IsValid)
                {
                    db.Terms.Add(terms);
                    db.SaveChanges();
                    return RedirectToAction("Create", "ReturnItems");
                }
            }
            else
            {
                TempData["message"] = "Click the checkbox to accept T&Cs.";
                return RedirectToAction("Create");
            }


            return View(terms);
        }

        // GET: Terms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terms terms = db.Terms.Find(id);
            if (terms == null)
            {
                return HttpNotFound();
            }
            return View(terms);
        }

        // POST: Terms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TermsID,Name,Email,Agreed")] Terms terms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(terms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(terms);
        }

        // GET: Terms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terms terms = db.Terms.Find(id);
            if (terms == null)
            {
                return HttpNotFound();
            }
            return View(terms);
        }

        // POST: Terms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Terms terms = db.Terms.Find(id);
            db.Terms.Remove(terms);
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
