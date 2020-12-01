using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.OnlineShopping
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public CartItem(Product product, int quanity)
        {
            Product = product;
            Quantity = quanity;
        }
       
    }
}