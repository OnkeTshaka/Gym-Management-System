using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Project.ViewModels;

namespace Project.Models.OnlineShopping
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string search, string category, int? page)
        {

            var categories = db.Products.Select(x => new CategoryViewModel { Name = x.Category.CategoryName }).Distinct().ToList();

            ViewBag.selectedCategory = category;

            var products = (dynamic)null;
            if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(search))
            {
                products = db.Products.ToList().ToPagedList(page ?? 1, 8);

            }
            else if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(search))
            {
                products = db.Products
                    .Where(p => p.Category.CategoryName.Equals(category) && p.Name.Contains(search))
                    .ToList().ToPagedList(page ?? 1, 8);
            }
            else
            {
                products = db.Products
                    .Where(p => p.Category.CategoryName.Equals(category) || p.Name.Contains(search))
                    .ToList().ToPagedList(page ?? 1, 8);
            }

            ProductPageViewModel homePageViewModel = new ProductPageViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(homePageViewModel);
        }
        public List<String> GetAutoComplete(string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var query = from product in context.Products
                            where product.Name.ToLower().Contains(name.ToLower())
                            select product.Name;

                List<String> names = query.ToList();
                return names;
            }
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            Product A1 = new Product();
            return View(A1);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,Name,Picture,Price,Time,Quantity,Discount")] Product product, HttpPostedFileBase image1)
        {
            if (image1 != null)
            {
                product.Picture = new byte[image1.ContentLength];
                image1.InputStream.Read(product.Picture, 0, image1.ContentLength);
            }
            if (ModelState.IsValid)
            {
                product.Time = DateTime.Now.ToLongDateString();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,Name,Picture,Price,Time,Quantity,Discount")] Product product, HttpPostedFileBase image1)
        {
            if (ModelState.IsValid)
            {
                Product productInDB = db.Products.Single(c => c.ProductID == product.ProductID);
                if (image1 != null)
                {
                    product.Picture = new byte[image1.ContentLength];
                    image1.InputStream.Read(product.Picture, 0, image1.ContentLength);
                    productInDB.Picture = product.Picture;
                }
                productInDB.CategoryID = product.CategoryID;
                productInDB.Name = product.Name;
                productInDB.Price = product.Price;
                productInDB.Quantity = product.Quantity;
                productInDB.Discount = product.Discount;
                productInDB.Time = DateTime.Now.ToString();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
