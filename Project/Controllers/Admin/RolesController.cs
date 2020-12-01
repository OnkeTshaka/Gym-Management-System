using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers.Admin
{
    public class RolesController : Controller
    {
        // GET: Dashboard/Roles
        ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Admin")]

        // GET: Dashboard/Roles
        public ActionResult Index()
        {
            UsersRolesViewModel UsersWithRoles = new UsersRolesViewModel();

            var allUsers = db.Users.ToList();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            List<ApplicationUser> Adminstrators = new List<ApplicationUser>();
            List<ApplicationUser> Managers = new List<ApplicationUser>();
            List<ApplicationUser> Members = new List<ApplicationUser>();

            foreach (var user in allUsers)
            {
                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    Adminstrators.Add(user);
                }
                else if (userManager.IsInRole(user.Id, "Manager"))
                {
                    Managers.Add(user);
                }
                else if (userManager.IsInRole(user.Id, "Member"))
                {
                    Members.Add(user);
                }
            }

            UsersWithRoles.Adminstrators = Adminstrators;
            UsersWithRoles.Managers = Managers;
            UsersWithRoles.Members = Members;

            return View(UsersWithRoles);
        }

        // GET: Dashboard/Roles/Details/5
        [Authorize(Roles = "Admin")]

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Where(i => i.Id == id).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            List<string> userRoles = userManager.GetRoles(user.Id).ToList();

            UserRolesViewModel userRoleViewModel = new UserRolesViewModel();

            userRoleViewModel.User = user;
            userRoleViewModel.UserRoles = userRoles;

            var roles = db.Roles.ToList();

            List<string> AllRoles = new List<string>();

            foreach (var role in roles)
            {
                AllRoles.Add(role.Name);
            }

            userRoleViewModel.AllRoles = AllRoles;
            return View(userRoleViewModel);
        }

        [Authorize(Roles = "Admin")]

        // GET: Dashboard/Roles/Create
        public ActionResult AddNewRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewRole(string RoleName)
        {
            if (!string.IsNullOrEmpty(RoleName) && !string.IsNullOrWhiteSpace(RoleName) && RoleName.Length > 5)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                // check if this doesnt already exists    
                if (!roleManager.RoleExists(RoleName))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = RoleName;
                    roleManager.Create(role);
                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Dashboard/Roles/Create
        public ActionResult DeleteRole()
        {
            var Roles = db.Roles.ToList();

            var AllRoleNames = (from r in Roles
                                select new SelectListItem { Text = r.Name, Value = r.Name });

            ViewBag.AllRoles = AllRoleNames;

            return View();
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public ActionResult DeleteRole(string RoleName)
        {
            if (RoleName.Equals("Admin") || RoleName.Equals("Manager") || RoleName.Equals("Member"))
            {
                //These are must roles & no should delete it
                return RedirectToAction("Index");
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            // check if this exists    
            if (roleManager.RoleExists(RoleName))
            {
                var Role = db.Roles.Where(r => r.Name == RoleName).FirstOrDefault();

                db.Roles.Remove(Role);
                db.SaveChanges();

                //I should also delete Roles from Users too here.
                var allUsers = db.Users.ToList();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                foreach (var user in allUsers)
                {
                    if (userManager.IsInRole(user.Id, RoleName))
                    {
                        userManager.RemoveFromRole(user.Id, RoleName);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult AddRoleToUser(string id, string RoleName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Where(i => i.Id == id).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            List<string> AllowedRoles = new List<string>();

            var RolesFromDB = db.Roles.ToList();

            foreach (var Role in RolesFromDB)
            {
                AllowedRoles.Add(Role.Name);
            }

            if (!AllowedRoles.Contains(RoleName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userManager.AddToRole(user.Id, RoleName);

            //Now return the user back to the details of this ad
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleFromUser(string id, string RoleName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Where(i => i.Id == id).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            List<string> userRoles = userManager.GetRoles(user.Id).ToList();

            if (!userRoles.Contains(RoleName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            userManager.RemoveFromRole(user.Id, RoleName);

            return RedirectToAction("Index");
        }
    }
}