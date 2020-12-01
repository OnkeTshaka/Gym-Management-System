using PagedList;
using Project.Models;
using Project.Models.Supplier;
using Project.Models.ManageStaff;
using Project.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult Index()
        {
            HomeViewModel LBDV = new HomeViewModel
            {
               Package = db.Packages.OrderByDescending(c => c.PackId).Take(3)
            };
            return View(LBDV);
        }
      
        public ActionResult About()
        {
            
            return View();
        }
        public ActionResult GetDirections()
        {

            return View();
        }
        public ActionResult Calendar()
        {
            return View();
        }

        //GET EVENTS
        public JsonResult GetEvents()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var events = db.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = db.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.Link = e.Link;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                        //Add some code
                        v.Trainer = e.Trainer;
                        v.Cost = e.Cost;
                    }
                }
                else
                {
                    db.Events.Add(e);
                }

                db.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var v = db.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    db.Events.Remove(v);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactUsForm ContactUsForm)
        {
            if (ModelState.IsValid)
            {
                db.ContactUsForms.Add(ContactUsForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index", ContactUsForm);
        }
        public ActionResult Class(string searching,int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var trainerList = db.Sessions.OrderByDescending(x => x.SessionID).Where(m => m.SessionType.Contains(searching)|| searching == null).ToPagedList(pageNumber, pageSize);
            return View(trainerList);
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