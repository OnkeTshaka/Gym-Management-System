using Project.Models.Essentials;
using Project.Models.ManageStaff;
using Project.Models.Return;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.OnlineShopping
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Phone")]
        public string CustomerPhone { get; set; }
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }
        [Display(Name = "Address")]
        public string CustomerAddress { get; set; }
        [Display(Name = "Reference code")]
        public  string Refcode { get; set; }
        public string From { get; set; }
        [Display(Name = "OrderDate")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<DeliveryTimes> DeliveryTime { get; set; }
    }
}