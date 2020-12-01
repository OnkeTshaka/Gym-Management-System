using Project.Models.Essentials;
using Project.Models.OnlineShopping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models.ManageStaff
{
    public class Member
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Memberer Email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Member Username is required to differentiate.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Member Address is required to differentiate.")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "ID Number")]
        public string IDNum { get; set; }

        [Required(ErrorMessage = "Mobile number is required to contact the member")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Joining Date of Member is required to keep record.")]
        [Display(Name = "Joining Date")]
        public string JoinDate { get; set; }
        public string Profile { get; set; }
        public virtual ICollection<BookingTrainer> BookingTrainers { get; set; }
        public virtual ICollection<MembershipPlan> MembershipPlans { get; set; }

    }
}