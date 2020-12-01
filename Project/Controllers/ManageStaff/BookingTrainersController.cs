using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project.Models;
using Project.Models.ManageStaff;

namespace Project.Controllers.ManageStaff
{
    public class BookingTrainersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookingTrainers
        public ActionResult Index()
        {
            var bookingTrainer = db.BookingTrainer.Include(b => b.Event).Include(b => b.Member);
            return View(bookingTrainer.ToList());
        }

        // GET: BookingTrainers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTrainer bookingTrainer = db.BookingTrainer.Find(id);
            if (bookingTrainer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = id.Value;
            var comments = db.RateBookings.Where(d => d.ArticleId.Equals(id.Value)).ToList();
            ViewBag.Comments = comments;

            var ratings = db.RateBookings.Where(d => d.ArticleId.Equals(id.Value)).ToList();
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
            return View(bookingTrainer);
        }

        // GET: BookingTrainers/Create
        public ActionResult Create()
        {
            ViewBag.EventID = new SelectList(db.Events, "EventID", "Subject");
            ViewBag.memberID = new SelectList(db.Members, "ID", "Email");
            return View();
        }

        // POST: BookingTrainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,BookingDate,Username,Email,IDNum,Address,Description,MobileNumber,EventID,memberID")] BookingTrainer bookingTrainer)
        {
            if (ModelState.IsValid)
            {
                string CurrentUserName = User.Identity.GetUserName();
                bookingTrainer.BookingDate = DateTime.Now;
                Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
                Event events = db.Events.Single(e=>e.EventID == bookingTrainer.EventID);
                bookingTrainer.EventID = events.EventID;
                bookingTrainer.memberID = member.ID;
                bookingTrainer.Username = member.Username;
                bookingTrainer.MobileNumber = member.MobileNumber;
                bookingTrainer.IDNum = member.IDNum;
                bookingTrainer.Email = member.Email;
                bookingTrainer.Address = member.Address;

                MailMessage nn = new MailMessage();
                nn.To.Add(bookingTrainer.Email);
                nn.From = new MailAddress("fire31386@gmail.com", "Return Of Firewalls Team");
                nn.Subject = "Personal Trainer and Class Booked ";
                nn.Body = " <b>Booking for :</b>  " + "<b>" + member.Username + "<br/>" 
                    + "<b>" +"Class:"+"</b> " + events.Subject + "<br/>" 
                       + "<b>"+"ID number :" + member.IDNum + "</b>" + "<br/>" + "<b>Your trainer:</b>  " + events.Trainer + "<br/> "
                  + "<b>Booking Date:</b> " + bookingTrainer.BookingDate + "<br/>"+
                  "<b>Link:</b> " + events.Description+ "<br/>"+

                   "<b>Starting Time:</b> " + events.Start + "<br/>"
                   + "<b>Ending Time:</b> " + events.End + "<br/>" +
                   

                   "Please make sure that you are on time for your class"+
                   " Thank you," + "<b>" + " for booking";
                nn.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("fire31386@gmail.com", "xrdgzhacjsvarnfr");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(nn);
                ViewBag.Message = "Mail has been sent";
                db.BookingTrainer.Add(bookingTrainer);
                db.SaveChanges();
                return RedirectToAction("Details","Profile");
            }

            ViewBag.EventID = new SelectList(db.Events, "EventID", "Subject", bookingTrainer.EventID);
            ViewBag.memberID = new SelectList(db.Members, "ID", "Email", bookingTrainer.memberID);
            return View(bookingTrainer);
        }

        // GET: BookingTrainers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTrainer bookingTrainer = db.BookingTrainer.Find(id);
            if (bookingTrainer == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "Subject", bookingTrainer.EventID);
            ViewBag.memberID = new SelectList(db.Members, "ID", "Email", bookingTrainer.memberID);
            return View(bookingTrainer);
        }

        // POST: BookingTrainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingID,BookingDate,Username,Email,IDNum,Address,Description,MobileNumber,EventID,memberID")] BookingTrainer bookingTrainer)
        {
            if (ModelState.IsValid)
            {
                if (bookingTrainer.BookingID != 0)
                {
                    BookingTrainer sesInDB = db.BookingTrainer.Single(c => c.BookingID == bookingTrainer.BookingID);
                    sesInDB.EventID = bookingTrainer.EventID;
                    
                    string CurrentUserName = User.Identity.GetUserName();
                    Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
                    Event events = db.Events.FirstOrDefault();
                    bookingTrainer.memberID = member.ID;
                    bookingTrainer.Username = member.Username;
                    bookingTrainer.BookingDate = DateTime.Now;
                    bookingTrainer.MobileNumber = member.MobileNumber;
                    bookingTrainer.IDNum = member.IDNum;
                    bookingTrainer.Email = member.Email;
                    bookingTrainer.Address = member.Address;

                    MailMessage nn = new MailMessage();
                    nn.To.Add(bookingTrainer.Email);
                    nn.From = new MailAddress("thefirewalls8@gmail.com", "Return Of Firewalls Team");
                    nn.Subject = "Updated booking for Class Booked ";
                    nn.Body = " <b>Booking for :</b>  " + "<b>" + member.Username + "<br/>"
                        + "< b > Class :</ b > " + " < b > " + events.Subject + " < br /> "
                           + "<b>ID number :" + member.IDNum + "</b>" + "<br/>" + "<b>Your trainer:</b>  " + events.Trainer + "<br/> "
                      + "<b>Booking Date:</b> " + bookingTrainer.BookingDate + "<br/>" +
                       "<b>Starting Time:</b> " + events.Start + "<br/>"+
                        "<b>Link:</b> " + events.Link + "<br/>"
                       + "<b>Ending Time:</b> " + events.End + "<br/>" +
                       "<b>Cost:</b> " + "R" + events.Cost + "<br/>" +
                       " Thank you," + "<b>" + " for booking";
                    nn.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    NetworkCredential nc = new NetworkCredential("thefirewalls8@gmail.com", "Dut@1234");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Send(nn);
                    ViewBag.Message = "Mail has been sent";
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = bookingTrainer.BookingID });
                }
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "Subject", bookingTrainer.EventID);
            ViewBag.memberID = new SelectList(db.Members, "ID", "Email", bookingTrainer.memberID);
            return View(bookingTrainer);
        }

        // GET: BookingTrainers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTrainer bookingTrainer = db.BookingTrainer.Find(id);
            if (bookingTrainer == null)
            {
                return HttpNotFound();
            }
            return View(bookingTrainer);
        }

        // POST: BookingTrainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingTrainer bookingTrainer = db.BookingTrainer.Find(id);
            db.BookingTrainer.Remove(bookingTrainer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PayFast(string id, string price, decimal P_amount, int cid)
        {
            // Create the order in your DB and get the ID
            string amount = price;
            string orderID = id;
            string name = "Payment reference #" + orderID;
            string description = "Product payment";

            string site = "";
            string merchant_id = "";
            string merchant_key = "";

            // Check if we are using the test or live system
            string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchant_id = "10000100";
                merchant_key = "46f0cd694581a";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchant_id = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantID"];
                merchant_key = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
            }
            // Build the query string for payment site

            StringBuilder str = new StringBuilder();
            str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
            str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
            str.Append("&return_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_ReturnURL"]));
            str.Append("&cancel_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_CancelURL"]));
            //str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

            str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderID));
            str.Append("&amount=" + HttpUtility.UrlEncode(amount));
            str.Append("&item_name=" + HttpUtility.UrlEncode(name));
            str.Append("&item_description=" + HttpUtility.UrlEncode(description));
            decimal AmountPayed = P_amount;
            BookingTrainer cause = db.BookingTrainer.Find(cid);
            //cause.price += P_amount;
            db.SaveChanges();

            // Redirect to PayFast
            Response.Redirect(site + str.ToString());
            //return (site + str.ToString());

            return View();
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
