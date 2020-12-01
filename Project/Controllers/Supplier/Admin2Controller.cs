using Project.Models;
using Project.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.Supplier
{
    public class Admin2Controller : Controller
    {
        // GET: Admin
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Categories()
        {
            return View(db.supplierCategories.ToList());
        }

        // GET: Categories/Create
        public ActionResult CategoryAdd()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryAdd([Bind(Include = "CategoryID,CategoryName,IsActive,IsDelete")] supplierCategory category)
        {
            if (ModelState.IsValid)
            {
                db.supplierCategories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierCategory category = db.supplierCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryEdit([Bind(Include = "CategoryID,CategoryName,IsActive,IsDelete")] supplierCategory category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(category);
        }
        public ActionResult CategoryDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierCategory category = db.supplierCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("CategoryDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplierCategory category = db.supplierCategories.Find(id);
            db.supplierCategories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Categories");
        }
        //Index for product
        public ActionResult Product(string searching)
        {
            var products = db.supplierProducts.OrderByDescending(x => x.ProductID).Where(m => m.ProductName.Contains(searching) || m.supplierCategory.CategoryName.Contains(searching) || searching == null).Include(p => p.supplierCategory);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierProduct product = db.supplierProducts.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult ProductAdd()
        {
            ViewBag.CategoryID = new SelectList(db.supplierCategories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductAdd([Bind(Include = "ProductID,ProductName,CategoryID,IsActive,IsDelete,CreatedDate,ModifiedDate,Description,ProductImage,IsFeatured,Quantity,Price")] supplierProduct product, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    String path = Path.Combine(Server.MapPath("~/ProdImg"), file.FileName);
                    file.SaveAs(path);
                    product.ProductImage = file.FileName;
                }
                product.CreatedDate = DateTime.Now;
                //var products = db.supplierProducts.Include(b => b.item).Include(b => b.Member);
                db.supplierProducts.Add(product);
                db.SaveChanges();
                return RedirectToAction("Product");
            }

            ViewBag.CategoryID = new SelectList(db.supplierCategories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        //Edit for product
        public ActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierProduct product = db.supplierProducts.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.supplierCategories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductEdit([Bind(Include = "ProductID,ProductName,CategoryID,IsActive,IsDelete,CreatedDate,ModifiedDate,Description,ProductImage,IsFeatured,Quantity,Price")] supplierProduct product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    String path = Path.Combine(Server.MapPath("~/ProdImg"), file.FileName);
                    file.SaveAs(path);
                    product.ProductImage = file.FileName;
                }
                product.ModifiedDate = DateTime.Now;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Product");
            }
            ViewBag.CategoryID = new SelectList(db.supplierCategories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }
        public ActionResult ProductDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierProduct product = db.supplierProducts.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("ProductDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            supplierProduct product = db.supplierProducts.Find(id);
            db.supplierProducts.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Product");
        }

    }
}