using Project.Models.ManageStaff;
using Project.Models.OnlineShopping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Return
{
    public enum selectActions
    {
        Repair,
        Replace,
        Refund
    }
    public class ReturnItem
    {
        [Key, ScaffoldColumn(false), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReturnItemID { get; set; }
        [Display(Name = "Customer")]
        public string ClientName { get; set; }
        [Display(Name = "Email")]
        public string ClientEmail { get; set; }
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        [Display(Name ="Action")]
        public selectActions selectAction { get; set; }
        public string Status { get; set; }
        [Display(Name ="Pick Up Address")]
        public string From { get; set; }
        public string To { get; set; }
        [ForeignKey("Member")]
        public int memberID { get; set; }
        public virtual Member Member { get; set; }
        public int ReasonID { get; set; }
        public virtual Reason Reason { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}