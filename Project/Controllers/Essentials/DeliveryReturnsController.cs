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
using Project.Models.Return;
using System.Net.Mail;
using Project.Models.Essentials;
using PagedList;

namespace Project.Controllers.Essentials
{
    public class DeliveryReturnsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DeliveryReturns
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
           var deliveryReturns = db.DeliveryReturns.OrderByDescending(x => x.DeliveryReturnID).Include(d => d.Driver).Include(d => d.ReturnItem).ToPagedList(pageNumber, pageSize);
            return View(deliveryReturns);
        }
        // GET: DeliveryReturns/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryReturn deliveryReturn = await db.DeliveryReturns.FindAsync(id);
            if (deliveryReturn == null)
            {
                return HttpNotFound();
            }
            return View(deliveryReturn);
        }

        // GET: DeliveryReturns/Create
        public ActionResult Create(int id)
        {

            Session["ReturnItemID"] = id;
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name");
            ViewBag.ReturnItemID = new SelectList(db.ReturnItems, "ReturnItemID", "ClientName");
            return View();
        }

        // POST: DeliveryReturns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DeliveryReturnID,ReturnItemID,DriverID,DeliveryDate,FromTime,ToTime,Status")] DeliveryReturn deliveryReturn)
        {
            if (ModelState.IsValid)
            {
                deliveryReturn.ReturnItemID = int.Parse(Session["ReturnItemID"].ToString());
                db.DeliveryReturns.Add(deliveryReturn);
                await db.SaveChangesAsync();
                ReturnItem returnItem = db.ReturnItems.Single(c => c.ReturnItemID == deliveryReturn.ReturnItemID);
                Driver driver = db.Drivers.Single(c => c.DriverID == deliveryReturn.DriverID);
                deliveryReturn.ReturnItemID = returnItem.ReturnItemID;
                deliveryReturn.DriverID = driver.DriverID;
                MailMessage nn = new MailMessage();
                nn.To.Add(returnItem.ClientEmail);
                nn.To.Add(driver.EmailAddress);
                nn.From = new MailAddress("fire31386@gmail.com", "Return Of Firewalls Team");
                nn.Subject = "Pick up for Returning Products";
                nn.Body = " <b>Request made by:</b>  " + "<b>" + returnItem.ClientName + "<br/>"
                  + "<b>Pick Up Date :</b> " + deliveryReturn.DeliveryDate.ToLongDateString() + "<br/>" +
                   " <b>Time:</b>  " +"Between"+ deliveryReturn.FromTime.ToString("hh:mm tt") + " AND " + deliveryReturn.ToTime.ToString("hh:mm tt") + "<br/>" +
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

            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name", deliveryReturn.DriverID);
            ViewBag.ReturnItemID = new SelectList(db.ReturnItems, "ReturnItemID", "ClientName", deliveryReturn.ReturnItemID);
            return View(deliveryReturn);
        }

        // GET: DeliveryReturns/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryReturn deliveryReturn = await db.DeliveryReturns.FindAsync(id);
            if (deliveryReturn == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name", deliveryReturn.DriverID);
            ViewBag.ReturnItemID = new SelectList(db.ReturnItems, "ReturnItemID", "ClientName", deliveryReturn.ReturnItemID);
            return View(deliveryReturn);
        }

        // POST: DeliveryReturns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DeliveryReturnID,ReturnItemID,DriverID,DeliveryDate,FromTime,ToTime,Status")] DeliveryReturn deliveryReturn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryReturn).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "Name", deliveryReturn.DriverID);
            ViewBag.ReturnItemID = new SelectList(db.ReturnItems, "ReturnItemID", "ClientName", deliveryReturn.ReturnItemID);
            return View(deliveryReturn);
        }

        // GET: DeliveryReturns/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryReturn deliveryReturn = await db.DeliveryReturns.FindAsync(id);
            if (deliveryReturn == null)
            {
                return HttpNotFound();
            }
            return View(deliveryReturn);
        }

        // POST: DeliveryReturns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DeliveryReturn deliveryReturn = await db.DeliveryReturns.FindAsync(id);
            db.DeliveryReturns.Remove(deliveryReturn);
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
