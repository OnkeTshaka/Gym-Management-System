using Project.Models.ManageStaff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.ManageStaff
{
    public class BookingTrainer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingID { get; set; }
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }
        [Display(Name = "Name")]
        public string Username { get; set; }
        public string Email { get; set; }
        [Display(Name = "ID Number")]
        public string IDNum { get; set; }
        public string Address { get; set; }
        [Display(Name = "Phone")]
        public string MobileNumber { get; set; }
        [ForeignKey("Event")]
        public int EventID { get; set; }
        public virtual Event Event { get; set; }
        [ForeignKey("Member")]
        public int memberID { get; set; }
        public virtual Member Member { get; set; }
    }
}