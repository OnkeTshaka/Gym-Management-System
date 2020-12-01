using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Refund
{
    public enum adminResponse
    {
        Pending,
        NotPayed,
        Approved,
        Declined,
        Payed
       
        
    }
    public class RequestMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestMemberID { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string JoinDate { get; set; }
        [Display(Name = "Application Date")]
        public string ApplicationDate { get; set; }
        //Member properties
        [Display(Name = "Name")]
        public string Username { get; set; }

        public string Email { get; set; }
        public adminResponse Responses { get; set; }
        public string Description { get; set; }

        [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }
        public bool Apply { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Period { get; set; }

    }
}