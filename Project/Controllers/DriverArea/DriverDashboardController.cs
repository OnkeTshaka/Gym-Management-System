using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.DriverArea
{
    public class DriverDashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DriverDashboard
        //[Authorize(Roles = "Driver")]
        public ActionResult Index()
        {
            DriverDashboardViewModel DBVM = new DriverDashboardViewModel
            {
                DeliveryOrders = db.DeliveryTime.Count(),
                DeliveryReturns = db.DeliveryReturns.Count(),
                CompletedReturns = db.DeliveryReturns.Where(x => x.Status == "Complete").Count()
        };
            return View(DBVM);
        }
    }
}