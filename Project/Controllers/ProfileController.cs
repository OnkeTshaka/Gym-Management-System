using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Microsoft.AspNet.Identity;
using Project.Models.ManageStaff;
using Project.Models.Return;
using Project.Core;

namespace Project.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult UniqueIndex(int? page)
        {
            string CurrentUserName = User.Identity.GetUserName();
            return View(db.Orders.OrderByDescending(m=>m.OrderID).Where(m => m.CustomerName == CurrentUserName).ToList());
        }
        public ActionResult BookClassIndex(int? page)
        {
            string CurrentUserName = User.Identity.GetUserName();
            return View(db.BookingTrainer.OrderByDescending(m => m.BookingID).Where(m => m.Username == CurrentUserName).ToList());
        }
        public ActionResult ReturnIndex(int? id, int? productID)
        {
            string CurrentUserName = User.Identity.GetUserName();
            var viewModel = new ReturnItemIndexData();

            viewModel.ReturnItems = db.ReturnItems.OrderByDescending(m => m.ReturnItemID).Include(i => i.Products).Include(r => r.Reason).Where(m => m.ClientName == CurrentUserName).ToList();
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
        public ActionResult RefundIndex(int? page)
        {
            string CurrentUserName = User.Identity.GetUserName();
            return View(db.Membership.OrderByDescending(m => m.PlanID).Where(m => m.Username == CurrentUserName).ToList());
        }
        public PartialViewResult Progress1()
        {
            var opt = new MultipleStepProgressTabOption()
            {
                Steps = new List<string>()
                {
                    "<h5 style:'color:#fff;'>Pending</h5>",
                    "<h5 style:'color:white;'>Approved</h5>",
                    "<h5 style:'color:#white;'>Reviewed</h5>",
                    "<h5 style:'color:#fff;'>Processing</h5>",
                    "<h5 style:'color:#fff;'>Complete</h5>"


                },
                CurrentStepIndex = 1
            };
            return PartialView(opt);
        }
        public PartialViewResult Progress2()
        {
            var opt = new MultipleStepProgressTabOption()
            {
                Steps = new List<string>()
                {
                   "<h5 style:'color:#fff;'>Pending</h5>",
                    "<h5 style:'color:white;'>Approved</h5>",
                    "<h5 style:'color:#white;'>Reviewed</h5>",
                    "<h5 style:'color:#fff;'>Processing</h5>",
                    "<h5 style:'color:#fff;'>Complete</h5>"
                },
                CurrentStepIndex = 2
            };
            return PartialView(opt);
        }
        public PartialViewResult Progress3()
        {
            var opt = new MultipleStepProgressTabOption()
            {
                Steps = new List<string>()
                {
                  "<h5 style:'color:#fff;'>Pending</h5>",
                    "<h5 style:'color:white;'>Approved</h5>",
                    "<h5 style:'color:#white;'>Reviewed</h5>",
                    "<h5 style:'color:#fff;'>Processing</h5>",
                    "<h5 style:'color:#fff;'>Complete</h5>"
                },
                CurrentStepIndex = 3
            };
            return PartialView(opt);
        }
        public PartialViewResult Progress4()
        {
            var opt = new MultipleStepProgressTabOption()
            {
                Steps = new List<string>()
                {
                    "<h5 style:'color:#fff;'>Pending</h5>",
                    "<h5 style:'color:white;'>Approved</h5>",
                    "<h5 style:'color:#white;'>Reviewed</h5>",
                    "<h5 style:'color:#fff;'>Processing</h5>",
                    "<h5 style:'color:#fff;'>Complete</h5>"
                },
                CurrentStepIndex = 4
            };
            return PartialView(opt);
        }
        public PartialViewResult Progress5()
        {
            var opt = new MultipleStepProgressTabOption()
            {
                Steps = new List<string>()
                {
                    "<h5 style:'color:#fff;'>Pending</h5>",
                    "<h5 style:'color:white;'>Approved</h5>",
                    "<h5 style:'color:#white;'>Reviewed</h5>",
                    "<h5 style:'color:#fff;'>Processing</h5>",
                    "<h5 style:'color:#fff;'>Complete</h5>"
                },
                CurrentStepIndex = 5
            };
            return PartialView(opt);
        }
        // GET: Profile/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //instead of showing him bad request, why not show user, his own profile :D
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                string CurrentUserName = User.Identity.GetUserName();
                id = db.Members.Where(s => s.Username == CurrentUserName).FirstOrDefault().ID;

                if (id==null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }

            return View(member);
        }

        // GET: Profile/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email,Username,Address,IDNum,MobileNumber,JoinDate,Profile")] Member member, HttpPostedFileBase ProfilePictureFile)
        {
            if (ModelState.IsValid)
            {
                Member memberInDB = db.Members.Single(c => c.ID == member.ID);
                if (ProfilePictureFile != null)
                {
                    String path = Path.Combine(Server.MapPath("~/Images"), (DateTime.Now.ToString("yyyyMMddHHmmss") + "_User_" + ProfilePictureFile.FileName));
                    ProfilePictureFile.SaveAs(path);
                    member.Profile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "_User_" + ProfilePictureFile.FileName);
                    memberInDB.Profile = member.Profile;
                }
                memberInDB.Email = member.Email;
                memberInDB.Username = member.Username;
                memberInDB.MobileNumber = member.MobileNumber;
                memberInDB.Address = member.Address;
                memberInDB.IDNum = member.IDNum;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }


        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
