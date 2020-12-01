using Project.Models.ManageStaff;
using Project.Models.Refund;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Essentials
{
    public class MembershipPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanID { get; set; }

        public string Shift { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string JoinDate { get; set; }

        //Member properties
        [Display(Name = "Name")]
        public string Username { get; set; }

        public string Email { get; set; }
        [Display(Name = "ID Number")]
        public string IDNum { get; set; }

        public string Address { get; set; }

        [Display(Name = "Cancel my membership")]
        public bool CancelMember { get; set; }

        public string Description { get; set; }

        [Display(Name = "Phone")]
        public string MobileNumber { get; set; }

        [Display(Name = "Monthly Fee")]
        [Required]
        public double Monthly_Fee { get; set; }


        [Display(Name = "Joining Fee")]
        public double Joining_Fee { get; set; }

        [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime trialPeriod { get; set; }

        [ForeignKey("Package")]
        public int PackId { get; set; }
        public virtual Package Package { get; set; }

        [ForeignKey("Member")]
        public int memberID { get; set; }
        public virtual Member Member { get; set; }
        public virtual DateTime CalcEnd()
        {
            var y = DateTime.Now.AddDays(10);
            return (y);
        }
        public virtual int Days()
        {
            var days = (trialPeriod - DateTime.Now).Days;
            return (days);
        }

    }
}