using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Models.Essentials;
using Project.Models.ManageStaff;
using Project.Models.OnlineShopping;
using Project.Models.Refund;
using Project.Models.Return;

namespace Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string IDNum { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<BookingTrainer> BookingTrainer { get; set; }
        public virtual DbSet<Trainer> Trainer { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<CommentsRating> CommentsRatings { get; set; }
        public virtual DbSet<RateBooking> RateBookings{ get; set; }
        public virtual DbSet<RateClass> RateClasses { get; set; }
        public virtual DbSet<ContactUsForm> ContactUsForms { get; set; }
        //Essentials
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<SupplierClass> supplierClass { get; set; }
        public virtual DbSet<MembershipPlan> Membership { get; set; }


        //ONLINE SHOPPING
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        //Return Item
        public virtual DbSet<ReturnItem> ReturnItems { get; set; }
        public virtual DbSet<Reason> Reasons { get; set; }
        public virtual DbSet<Terms> Terms { get; set; }

        //Refund
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<RequestMember> RequestMembers { get; set; }

        //Delivery
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DeliveryTimes> DeliveryTime { get; set; }
        public virtual DbSet<DeliveryReturn> DeliveryReturns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Trainer>()
            //    .HasMany(c => c.Clubs).WithMany(i => i.Trainers)
            //    .Map(t => t.MapLeftKey("TrainerID")
            //        .MapRightKey("ClubID")
            //        .ToTable("TrainerClub"));
            base.OnModelCreating(modelBuilder);
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}