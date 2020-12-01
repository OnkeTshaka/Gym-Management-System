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
using PagedList;
using Project.Models;
using Project.Models.ManageStaff;
using Project.Models.Return;

namespace Project.Models.OnlineShopping
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var orderList = db.Orders.OrderByDescending(x => x.OrderID).ToPagedList(pageNumber, pageSize);

            return View(orderList);
        }
       

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        public ActionResult myDriver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        public ActionResult OrderComplete()
        {
            return View();
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress,From,OrderDate,PaymentType,Status")] Order order)
        {
            
            if (ModelState.IsValid)
            {
                List<CartItem> cartItems = (List<CartItem>)Session["cart"];
                Random randm = new Random();
                string upr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string downr = "abcdefghijklmnopqrstuvwxyz";
                string digir = "1234567890";
                char[] tno = new char[8];
                int r1 = randm.Next(0, 25);
                int r2 = randm.Next(0, 25);
                int r3 = randm.Next(0, 9);
                tno[0] = upr[r1];
                tno[1] = downr[r2];
                tno[2] = digir[r3];
                r1 = randm.Next(0, 25);
                r2 = randm.Next(0, 25);
                r3 = randm.Next(0, 9);
                tno[3] = upr[r2];
                tno[4] = downr[r1];
                tno[5] = digir[r3];
                string t_no = new string(tno);
                order.Refcode = t_no;
                order.OrderDate = DateTime.Now;
                string CurrentUserName = User.Identity.GetUserName();
                order.CustomerName = CurrentUserName;
                order.From = "Berea Centre Road, Bulwer, Berea, South Africa";
                if (order.Status == null)
                {
                    order.Status = "Pending";
                }


                db.Orders.Add(order);
                db.SaveChanges();
                foreach (CartItem cart in cartItems)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderID = order.OrderID,
                        ProductID = cart.Product.ProductID,
                        Quantity = cart.Quantity,
                        Price = cart.Product.Price
                    };
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();
                }
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("fire31386@gmail.com", "xrdgzhacjsvarnfr");
                    StringBuilder body = new StringBuilder()
                        .AppendLine("<h2>" + "A new order has been submitted!" + "</h2>")
                         .AppendLine("<h4>" + "Order placed by: " + order.CustomerName + "</h4>" + "</br>")
                          .AppendLine("<h4>" + "Reference code: " + order.Refcode + "</h4>" + "</br>")
                        .AppendLine("--------------------------------------------")
                        .AppendLine()
                    .AppendLine("<h4>" + "Purchased Items: " + "</h4>" + "</br>");
                    body.AppendLine("--------------------------------------------");

                    foreach (CartItem cart in cartItems)
                    {
                        var subtotal = cart.Product.GetDiscountedPrice() * cart.Quantity;
                        body.AppendFormat("{0} x {1} R{2:C}", "<h5>" + cart.Quantity, cart.Product.Name, subtotal + "</h5>");
                    }
                    body.AppendFormat("</br>");


                   body.AppendLine("--------------------------------------------" + "</br>")
                        .AppendFormat("Total : R" + cartItems.Sum(e => e.Product.GetDiscountedPrice() * e.Quantity) + "</br>")
                        .AppendLine("--------------------------------------------");

                    MailMessage mailMessage = new MailMessage("fire31386@gmail.com", order.CustomerEmail, "New Order Placed!", body.ToString());

                    mailMessage.BodyEncoding = Encoding.ASCII;
                    mailMessage.IsBodyHtml = true;

                    smtpClient.Send(mailMessage);
                }


                TempData["message"] = "Email sent";

                Session.Remove("cart");
                Session.Remove("count");
                return RedirectToAction("Details","Orders", new { id=order.OrderID} );
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress,OrderDate,PaymentType,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
