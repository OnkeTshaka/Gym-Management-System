using Project.Models.OnlineShopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.Return
{
    public class ReturnItemIndexData
    {
        public IEnumerable<ReturnItem> ReturnItems { get; set; }

        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Order> Orderd { get; set; }
    }
}