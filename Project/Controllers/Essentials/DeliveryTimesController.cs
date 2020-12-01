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
using Project.Models.Essentials;
using Project.Models.OnlineShopping;
using System.Net.Mail;
using PagedList;

namespace Project.Controllers.Essentials
{
    public class DeliveryTimesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DeliveryTimes
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 3;
            var deliveryTime = db.DeliveryTime.OrderByDescending(x => x.DeliveryTimesID).Include(d => d.Driver).Include(d => d.Order).ToPagedList(pageNumber, pageSize); ;
            return View(deliveryTime);
        }

        // GET: DeliveryTimes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTimes deliveryTimes = await db.DeliveryTime.FindAsync(id);
            if (deliveryTimes == null)
            {
                return HttpNotFound();
            }
            //Session["OrderID"] = id;
            return View(deliveryTimes);
        }

        // GET: DeliveryTimes/Create
        public ActionResult Create(int id)
        {
            Session["OrderID"] = id;
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name");
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerName");
            return View();
        }

        // POST: DeliveryTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DeliveryTimesID,OrderID,DriverID,DeliveryDate,FromTime,ToTime,Status")] DeliveryTimes deliveryTimes)
        {
            if (ModelState.IsValid)
            {
                deliveryTimes.OrderID = int.Parse(Session["OrderID"].ToString());
                db.DeliveryTime.Add(deliveryTimes);
                await db.SaveChangesAsync();
                Order order = db.Orders.Single(c => c.OrderID == deliveryTimes.OrderID);
                Driver driver = db.Drivers.Single(c => c.DriverID == deliveryTimes.DriverID);
                deliveryTimes.OrderID = deliveryTimes.OrderID;
                deliveryTimes.DriverID = driver.DriverID;
                MailMessage nn = new MailMessage();
                nn.To.Add(order.CustomerEmail);
                nn.To.Add(driver.EmailAddress);
                nn.From = new MailAddress("fire31386@gmail.com", "Return Of Firewalls Team");
                nn.Subject = "Delivery For Purchased Items";
                nn.Body = " <b>Request made by:</b>  " + "<b>" + order.CustomerName + "<br/>"
                  + "<b>Delivery Date :</b> " + deliveryTimes.DeliveryDate.ToLongDateString() + "<br/>" +
                   " <b>Time:</b>  " + "Between " + deliveryTimes.FromTime.ToString("hh:mm tt") + " AND " + deliveryTimes.ToTime.ToString("hh:mm tt") + "<br/>" +
                   "<b>Order ID: <b>" + order.Refcode +
                   "<b>Your Driver: <b>" + driver.Name +
                  "<br/>" + "<b>Driver Contact Details : </b> " + driver.PhoneNumber + "<br/>" +
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
                return RedirectToAction("Index");
            }

            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name", deliveryTimes.DriverID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerName", deliveryTimes.OrderID);
            return View(deliveryTimes);
        }

        // GET: DeliveryTimes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTimes deliveryTimes = await db.DeliveryTime.FindAsync(id);
            if (deliveryTimes == null)
            {
                return HttpNotFound();
            }
            //Session["OrderID"] = id;
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name", deliveryTimes.DriverID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerName", deliveryTimes.OrderID);
            return View(deliveryTimes);
        }

        // POST: DeliveryTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DeliveryTimesID,OrderID,DriverID,DeliveryDate,FromTime,ToTime,Status")] DeliveryTimes deliveryTimes)
        {
            if (ModelState.IsValid)
            {
                //deliveryTimes.OrderID = int.Parse(Session["OrderID"].ToString());
                db.Entry(deliveryTimes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name", deliveryTimes.DriverID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerName", deliveryTimes.OrderID);
            return View(deliveryTimes);
        }

        // GET: DeliveryTimes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTimes deliveryTimes = await db.DeliveryTime.FindAsync(id);
            if (deliveryTimes == null)
            {
                return HttpNotFound();
            }
            return View(deliveryTimes);
        }

        // POST: DeliveryTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DeliveryTimes deliveryTimes = await db.DeliveryTime.FindAsync(id);
            db.DeliveryTime.Remove(deliveryTimes);
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
