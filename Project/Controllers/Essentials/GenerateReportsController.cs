using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.Essentials
{
    public class GenerateReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: GenerateReports
        public ActionResult Index()
        {
            SalesViewModel DBVM = new SalesViewModel
            {
                Total = db.OrderDetails.Sum(e => e.Price),
                Products = db.OrderDetails.Count(),
                Size = db.OrderDetails.Sum(e=>e.Quantity)
            };
            return View(DBVM);
        }
        public ActionResult GetData()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var query = context.OrderDetails.Include("Product")
                   .GroupBy(p => p.Product.Name)
                   .Select(g => new { name = g.Key, count = g.Sum(w => w.Quantity) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}