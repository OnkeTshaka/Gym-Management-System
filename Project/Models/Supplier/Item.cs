using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Supplier
{
    public class Item
    {
        //[Key, ScaffoldColumn(false), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ItemID { get; set; }
        public virtual supplierProduct supplierProduct { get; set; }
        public int Quantity { get; set; }
    }
}