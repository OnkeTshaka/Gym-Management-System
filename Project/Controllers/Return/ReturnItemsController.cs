using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Project.Models;
using Project.Models.ManageStaff;
using Project.Models.OnlineShopping;
using Project.Models.Return;

namespace Project.Controllers.Return
{
    public class ReturnItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ReturnItems
        public ActionResult Index(int? id, int? productID)
        {
            var viewModel = new ReturnItemIndexData();

            viewModel.ReturnItems = db.ReturnItems.Include(i => i.Products).Include(r => r.Reason);

            if (id != null)
            {
                ViewBag.ReturnItemID = id.Value;
                viewModel.Products = viewModel.ReturnItems.Where(
                    i => i.ReturnItemID == id.Value).Single().Products;
            }

            if (productID != null)
            {
                ViewBag.ProductID = productID.Value;
                var selectedProducts = viewModel.Products.Where(x => x.ProductID == productID).Single();
                db.Entry(selectedProducts);
            }

            return View(viewModel);
        }
        private void PopulateAssignedProductData(ReturnItem returnItem)
        {
            var allProducts = db.Products;
            var returnItemProducts = new HashSet<int>(returnItem.Products.Select(c => c.ProductID));
            var viewModel = new List<AssignedProductData>();
            foreach (var product in allProducts)
            {
                viewModel.Add(new AssignedProductData
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Price=product.Price,
                    Assigned = returnItemProducts.Contains(product.ProductID)
                });
            }
            ViewBag.Products = viewModel;
        }
        // GET: ReturnItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnItem returnItem = db.ReturnItems.Find(id);
            if (returnItem == null)
            {
                return HttpNotFound();
            }
            return View(returnItem);
        }

        // GET: ReturnItems/Create
        public ActionResult Create()
        {
            ViewBag.ReasonID = new SelectList(db.Reasons, "ReasonID", "Name");
            var returnItems = new ReturnItem();
            returnItems.Products = new List<Product>();
            PopulateAssignedProductData(returnItems);
            return View();
        }

        // POST: ReturnItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReturnItemID,PurchaseRef,Quantity,Description,ClientName,ClientEmail,ReturnDate,Tax,Total,Status,From,To,ReasonID,selectAction")] ReturnItem returnItem, string[] selectedProducts)
        {
            
          
            bool isCapthcaValid = ValidateCaptcha(Request["g-recaptcha-response"]);
            string CurrentUserName = User.Identity.GetUserName();
            Member member = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault();
            returnItem.memberID = member.ID;
            returnItem.ClientName = member.Username;
            returnItem.ClientEmail = member.Email;
            returnItem.ReturnDate = DateTime.Now;
            returnItem.To = "Berea Centre Road, Bulwer, Berea, South Africa";
                if (isCapthcaValid)
            {
                if (returnItem.Status == null)
                {
                    returnItem.Status = "Awaiting";
                }
              if (selectedProducts != null)
                {
                returnItem.Products = new List<Product>();
                   foreach (var product in selectedProducts)
                   {
                      var productToAdd = db.Products.Find(int.Parse(product));
                    returnItem.Products.Add(productToAdd);
                   }
                    if (ModelState.IsValid)
                    {
                        db.ReturnItems.Add(returnItem);
                        db.SaveChanges();
                        return RedirectToAction("Details","ReturnItems", new { id = returnItem.ReturnItemID });
                    }
                }

            }
            else
            {
                TempData["message"] = "Verify the returning items.";
                return RedirectToAction("Create");
            }
            ViewBag.ReasonID = new SelectList(db.Reasons, "ReasonID", "Name", returnItem.ReasonID);
            PopulateAssignedProductData(returnItem);
            TempData["sms"] = "Request sent, please check your status after 24hrs.";
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public bool ValidateCaptcha(string response)
        {
            //  Setting _Setting = repositorySetting.GetSetting;

            //secret that was generated in key value pair  
            string secret = ConfigurationManager.AppSettings["GoogleSecretkey"]; //WebConfigurationManager.AppSettings["recaptchaPrivateKey"];

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            return Convert.ToBoolean(captchaResponse.Success);

        }
        // GET: ReturnItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnItem returnItem = db.ReturnItems.Include(i => i.Products).Where(i => i.ReturnItemID == id).Single();
            PopulateAssignedProductData(returnItem);
            if (returnItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReasonID = new SelectList(db.Reasons, "ReasonID", "Name", returnItem.ReasonID);
            //ViewBag.ReasonID = new SelectList(db.Reasons, "ReasonID", "Stage", returnItem.ReasonID);
            return View(returnItem);
        }

        // POST: ReturnItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedProducts)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var returnItemToUpdate = db.ReturnItems
               .Include(i => i.Products)
               .Where(i => i.ReturnItemID == id)
               .Single();

            if (TryUpdateModel(returnItemToUpdate, "",
               new string[] { "PurchaseRef","Quantity","Description","ClientName","ClientEmail","ReturnDate","Tax","Total","Status","ReasonID","selectAction" }))
            {
                try
                {
                    UpdateReturnItemsProducts(selectedProducts, returnItemToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index","ReturnItems", new { id= returnItemToUpdate.ReturnItemID});
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedProductData(returnItemToUpdate);
            ViewBag.ReasonID = new SelectList(db.Reasons, "ReasonID", "Name", returnItemToUpdate.ReasonID);
            return View(returnItemToUpdate);
        }
        private void UpdateReturnItemsProducts(string[] selectedProducts, ReturnItem returnItemToUpdate)
        {
            if (selectedProducts == null)
            {
                returnItemToUpdate.Products = new List<Product>();
                return;
            }

            var selectedProductsHS = new HashSet<string>(selectedProducts);
            var returnItemsProducts = new HashSet<int>
                (returnItemToUpdate.Products.Select(c => c.ProductID));
            foreach (var product in db.Products)
            {
                if (selectedProductsHS.Contains(product.ProductID.ToString()))
                {
                    if (!returnItemsProducts.Contains(product.ProductID))
                    {
                        returnItemToUpdate.Products.Add(product);
                    }
                }
                else
                {
                    if (returnItemsProducts.Contains(product.ProductID))
                    {
                        returnItemToUpdate.Products.Remove(product);
                    }
                }
            }
        }

        // GET: ReturnItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnItem returnItem = db.ReturnItems.Find(id);
            if (returnItem == null)
            {
                return HttpNotFound();
            }
            return View(returnItem);
        }

        // POST: ReturnItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReturnItem returnItem = db.ReturnItems
              .Include(i => i.Products)
              .Where(i => i.ReturnItemID == id)
              .Single();

            returnItem.Products = null;
            db.ReturnItems.Remove(returnItem);
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
