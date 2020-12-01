using Project.Models.OnlineShopping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Essentials
{
    public class DeliveryTimes
    {

        [Key, ScaffoldColumn(false), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryTimesID { get; set; }
        [ForeignKey("Order")]
        [Required, Display(Name = "Order")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey("Driver")]
        [Required, Display(Name = "Driver")]
        public int DriverID { get; set; }
        public virtual Driver Driver { get; set; }
        [Required, Display(Name = "DeliveryDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }
        [Required, Display(Name = "From")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public DateTime FromTime { get; set; }
        [Required, Display(Name = "To")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public DateTime ToTime { get; set; }
        public string Status { get; set; }


    }
}