using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.Refund
{
    public class StatsController : Controller
    {
        // GET: Stats
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            int expensive = context.Feedbacks.Where(x => x.Reasons == "Expensive").Count();
            int busy = context.Feedbacks.Where(x => x.Reasons == "Busy").Count();
            int other = context.Feedbacks.Where(x => x.Reasons == "Other").Count();
            int environment = context.Feedbacks.Where(x => x.Reasons == "Environment").Count();
            int afford = context.Feedbacks.Where(x => x.Reasons == "Afford").Count();
            Ratio obj = new Ratio();
            obj.Expensive = expensive;
            obj.Busy = busy;
            obj.Other = other;
            obj.Environment = environment;
            obj.Afford = afford;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public class Ratio
        {
            public int Expensive { get; set; }
            public int Busy { get; set; }
            public int Other { get; set; }
            public int Environment { get; set; }
            public int Afford { get; set; }
        }
    }
}