using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.OnlineShopping
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        public string Name { get; set; }
        public byte[] Picture { get; set; }

        public double Price { get; set; }
        public string Time { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public double GetDiscountedPrice()
        {
            return Price * (100 - Discount) / 100;
        }
    }
}