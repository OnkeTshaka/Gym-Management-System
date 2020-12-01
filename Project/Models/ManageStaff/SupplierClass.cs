using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.ManageStaff
{
    public class SupplierClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }
        [Display(Name = "Invoice Total")]
        public double InvoicePrice { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }



    }
}