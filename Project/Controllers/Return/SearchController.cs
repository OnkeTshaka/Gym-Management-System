using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.Return
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        [HttpGet]
        public ActionResult Index(String search)
        {
            if (String.IsNullOrWhiteSpace(search))
                return Redirect("~/");

            var s = search.ToLower();
            ViewBag.Search = search;
        
            var order = db.Orders.Where(p => p.Refcode.ToLower().Contains(s)).ToList();
            return View(order);
        }

        public ActionResult MySearch()
        {
            return View();
        }
    }
}