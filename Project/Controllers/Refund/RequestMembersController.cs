using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;
using Project.Models.ManageStaff;
using Project.Models.Refund;

namespace Project.Controllers.Refund
{
    public class RequestMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RequestMembers
        public ActionResult Unfiltered()
        {
            return View(db.RequestMembers.OrderByDescending(e=>e.RequestMemberID).Where(e=>e.Apply == true).ToList());
        }
        public ActionResult Index()
        {
            string CurrentUserName = User.Identity.GetUserName();
            return View(db.RequestMembers.OrderByDescending(e => e.RequestMemberID).Where(m=>m.Username == CurrentUserName).ToList());
        }

        // GET: RequestMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestMember requestMember = db.RequestMembers.Find(id);
            if (requestMember == null)
            {
                return HttpNotFound();
            }
            return View(requestMember);
        }

        // GET: RequestMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestMemberID,JoinDate,Username,Email,Description,TotalCost,Apply,Period,Responses")] RequestMember requestMember)
        {

            if (ModelState.IsValid)
            {
                db.RequestMembers.Add(requestMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestMember);
        }

        // GET: RequestMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestMember requestMember = db.RequestMembers.Find(id);
            if (requestMember == null)
            {
                return HttpNotFound();
            }
            return View(requestMember);
        }

        // POST: RequestMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestMemberID,JoinDate,Username,Email,Description,TotalCost,Apply,ApplicationDate,Period,Responses")] RequestMember requestMember)
        {
            if (ModelState.IsValid)
            {
                requestMember.ApplicationDate = DateTime.Now.ToLongDateString();
                db.Entry(requestMember).State = EntityState.Modified;
                db.SaveChanges();
                MailMessage nn = new MailMessage();
                nn.To.Add(requestMember.Email);
                nn.From = new MailAddress("fire31386@gmail.com", "Return Of Firewalls Team");
                nn.Subject = "Refund for membership";
                nn.Body = " <b>Membership for :</b>  " + "<b>" + requestMember.Username + "<br/>" + "<b>Status :" + requestMember.Responses + "</b>" + "<br/>" 
                  + "<b>Amount Due To You :R</b>" + requestMember.TotalCost + "<br/>" +
                   "Kind Regards" + "<br/>" +
                   "<b> Return Of Firewalls Team</b>";
                nn.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("fire31386@gmail.com", "xrdgzhacjsvarnfr");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(nn);
                if (User.IsInRole("Member"))
                {
                    return RedirectToAction("Index");
                }
                else if (requestMember.Responses == adminResponse.Approved)
                {
                    return RedirectToAction("Details", "RequestMembers", new { id = requestMember.RequestMemberID });
                }
                else {

                    return RedirectToAction("Unfiltered");
                }
            }
            return View(requestMember);
        }

        // GET: RequestMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestMember requestMember = db.RequestMembers.Find(id);
            if (requestMember == null)
            {
                return HttpNotFound();
            }
            return View(requestMember);
        }

        // POST: RequestMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestMember requestMember = db.RequestMembers.Find(id);
            db.RequestMembers.Remove(requestMember);
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
