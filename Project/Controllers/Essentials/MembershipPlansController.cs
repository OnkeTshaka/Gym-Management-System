using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Project.Models.Essentials;
using Microsoft.AspNet.Identity;
using Project.Models.ManageStaff;
using System.Text;
using System.Net.Mail;
//using PayPal;
using PayPal.Api;
using Project.Models.Refund;
using CaptchaMvc.HtmlHelpers;

namespace Project.Controllers.Essentials
{
    public class MembershipPlansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MembershipPlans
        public ActionResult Index()
        {
            var membership = db.Membership.Include(m => m.Member).Include(m => m.Package);
            return View(membership.ToList());
        }
        public ActionResult endTrial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPlan membershipPlan = db.Membership.Find(id);
            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            return View(membershipPlan);
        }
        public ActionResult viewTrial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPlan membershipPlan = db.Membership.Find(id);
            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            return View(membershipPlan);
        }
        // GET: MembershipPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPlan membershipPlan = db.Membership.Find(id);
            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            return View(membershipPlan);
        }

        // GET: MembershipPlans/Create
        public ActionResult Create()
        {
            ViewBag.PackId = new SelectList(db.Packages, "PackId", "PackageType");
            return View();
        }

        // POST: MembershipPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlanID,Shift,JoinDate,Username,Email,IDNum,Address,CancelMember,Description,MobileNumber,Monthly_Fee,Joining_Fee,TotalCost,trialPeriod,PackId,memberID")] MembershipPlan membershipPlan)
        {
            if (ModelState.IsValid)
            {
                string CurrentUserName = User.Identity.GetUserName();
                membershipPlan.JoinDate = DateTime.Now.ToLongDateString();
                Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
                Package package = db.Packages.Single(c => c.PackId == membershipPlan.PackId);
                membershipPlan.memberID = member.ID;
                membershipPlan.Username = member.Username;
                membershipPlan.MobileNumber = member.MobileNumber;
                membershipPlan.IDNum = member.IDNum;
                membershipPlan.Email = member.Email;
                membershipPlan.trialPeriod = membershipPlan.CalcEnd();
                membershipPlan.Address = member.Address;
                membershipPlan.Description = package.Description;
                membershipPlan.Monthly_Fee = package.Monthly_Fee;
                membershipPlan.Joining_Fee = package.Joining_Fee;
                membershipPlan.TotalCost = package.CalcTotalCost();
             

                MailMessage nn = new MailMessage();
                nn.To.Add(membershipPlan.Email);
                nn.From = new MailAddress("fire31386@gmail.com", "Return Of Firewalls Team");
                nn.Subject = "Membership Requested";
                nn.Body = " <b>Membership for :</b>  " + "<b>" + member.Username + "<br/>" + "<b>ID number :" + member.IDNum + "</b>" + "<br/>" + "<b>Package type:</b>  " + package.PackageType + "<br/> "
                  + "<b>Package Price :R</b> " + package.Monthly_Fee + "<br/>" +
                   " <b>Duration:</b>  " + package.Duration +  "<br/>" + "<b>Months<b>"+ "</b>"+
                   "<b>Joining Fee R<b>" +package.Joining_Fee +
                  "<br/>" + "<b>Total Cost to Pay : R</b> " + package.TotalCost + "<br/>" +
                   " Thank you," + "<b>" + " For requesting this membership plan" + "<br/>"
                   + "<br/>" + "<br/>" +
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
                ViewBag.Message = "Mail has been sent";
                db.Membership.Add(membershipPlan);
                db.SaveChanges();
                RequestMember trial = new RequestMember();
                trial.Email = membershipPlan.Email;
                trial.Responses =adminResponse.Pending;
                trial.Username = membershipPlan.Username;
                trial.TotalCost = package.CalcTotalCost();
                trial.Description = package.Description;
                trial.Period = membershipPlan.CalcEnd();
                trial.JoinDate = membershipPlan.JoinDate;
                trial.ApplicationDate = "N/A";
                db.RequestMembers.Add(trial);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = membershipPlan.PlanID });
            }

            ViewBag.PackId = new SelectList(db.Packages, "PackId", "PackageType", membershipPlan.PackId);
            return View(membershipPlan);
        }

        // GET: MembershipPlans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPlan membershipPlan = db.Membership.Find(id);
            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.memberID = new SelectList(db.Members, "ID", "Email", membershipPlan.memberID);
            ViewBag.PackId = new SelectList(db.Packages, "PackId", "PackageType", membershipPlan.PackId);
            return View(membershipPlan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlanID,Shift,JoinDate,Username,Email,IDNum,Address,CancelMember,Description,MobileNumber,Monthly_Fee,Joining_Fee,TotalCost,trialPeriod,PackId,memberID")] MembershipPlan membershipPlan)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(membershipPlan).State = EntityState.Modified;
                    db.SaveChanges();
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index");
                    }
                    if (User.IsInRole("Member"))
                    {
                        return RedirectToAction("endTrial", "MembershipPlans", new { id = membershipPlan.PlanID });
                    }

                }
            }
            ViewBag.memberID = new SelectList(db.Members, "ID", "Email", membershipPlan.memberID);
            ViewBag.PackId = new SelectList(db.Packages, "PackId", "PackageType", membershipPlan.PackId);
            return View(membershipPlan);
        }


        // GET: MembershipPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPlan membershipPlan = db.Membership.Find(id);
            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            return View(membershipPlan);
        }

        // POST: MembershipPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MembershipPlan membershipPlan = db.Membership.Find(id);
            db.Membership.Remove(membershipPlan);
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
            MembershipPlan cause = db.Membership.Find(cid);
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
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/MembershipPlans/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    // This function exectues after receving all parameters for the payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Home/Calendar");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }

            //on successful payment, show success page to user.
            return View("Home/Calendar");
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            //Adding Item Details like name, currency, price etc
            itemList.items.Add(new Item()
            {
                name = "Payments",
                currency = "ZAR",
                price = "1",
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "ZAR",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your invoice number", //Generate an Invoice No
                amount = amount,
                item_list = itemList
            });


            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

    }
}
