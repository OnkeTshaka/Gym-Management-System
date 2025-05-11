namespace Project.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using Project.Models;
    using Project.Models.OnlineShopping;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Project.Models.Return;

    internal sealed class Configuration : DbMigrationsConfiguration<Project.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Project.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category {CategoryName = "Supplements"},
                    new Category {CategoryName = "Gym Apparel"},
                    new Category {CategoryName = "Fitness Equipment"},
                    new Category {CategoryName = "Accessories"},
                    new Category {CategoryName = "Personal Care"},
                    new Category {CategoryName = "Memberships & Services"}
                };
                categories.ForEach(d => context.Categories.AddOrUpdate(x => x.CategoryName, d));
                context.SaveChanges();
            }

            if (!context.Reasons.Any())
            {
                var reasons = new List<Reason>
                {
                    new Reason { Name = "Received the wrong item" },
                    new Reason { Name = "Product arrived damaged or defctive" },
                    new Reason { Name = "Item doesn't fit (e.g. apparel or accessories)" },
                    new Reason { Name = "Changed mind/No longer needed" },
                    new Reason { Name = "Item not as described or pictured" },
                    new Reason { Name = "Duplicate order placed by mistake" },
                    new Reason { Name = "Receieved late or aftr expected delivery date" },
                    new Reason { Name = "Allergic reaction or intolerancee (for supplements)" },
                    new Reason { Name = "Missing parts or accessories" },
                    new Reason { Name = "Wrong size or variation (e.g. Flavor weight, color)" },
                };
               reasons.ForEach(d => context.Reasons.AddOrUpdate(x => x.Name, d));
                context.SaveChanges();
            }
            // seed roles
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManger.RoleExists("Admin"))
            {
                roleManger.Create(new IdentityRole("Admin"));
            }
            if (!roleManger.RoleExists("Supplier"))
            {
                roleManger.Create(new IdentityRole("Supplier"));
            }
            if (!roleManger.RoleExists("Driver"))
            {
                roleManger.Create(new IdentityRole("Driver"));
            }

            // seed admin user
            var useManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.UserName == "firewalls@gmail.com"))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "firewalls@gmail.com",
                    Email = "firewalls@gmail.com",
                    EmailConfirmed = true
                };
                var result = useManager.Create(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    useManager.AddToRole(adminUser.Id, "Admin");
                }
            }

            if (!context.Users.Any(u => u.UserName == "Driver"))
            {
                var driverUser = new ApplicationUser
                {
                    UserName = "Driver",
                    Email = "zomsa@duck.com",
                    PhoneNumber = "0129345675",
                    Address = "Berea, Durban",
                    EmailConfirmed = true
                };
                var result = useManager.Create(driverUser, "driver#@");
                if (result.Succeeded)
                {
                    useManager.AddToRole(driverUser.Id, "Driver");
                }
            }


            if (!context.Users.Any(u => u.UserName == "Supplier"))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "Supplier",
                    Email = "zomsa@duck.com",
                    PhoneNumber = "0129345675",
                    Address = "Berea, Durban",
                    EmailConfirmed = true
                };
                var result = useManager.Create(adminUser, "supplier#@");
                if (result.Succeeded)
                {
                    useManager.AddToRole(adminUser.Id, "Supplier");
                }
            }
        }
    }
}
