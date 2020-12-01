using Project.Models.Essentials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Return
{
    public class DeliveryReturn
    {
        [Key, ScaffoldColumn(false), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryReturnID { get; set; }
        [ForeignKey("ReturnItem")]
        [Required, Display(Name = "Returning Item(s)")]
        public int ReturnItemID { get; set; }
        public virtual ReturnItem ReturnItem { get; set; }
        [ForeignKey("Driver")]
        [Required, Display(Name = "Driver")]
        public int DriverID { get; set; }
        public virtual Driver Driver { get; set; }
        [Required, Display(Name = "Collection Date")]
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