using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.Return
{
    public class AssignedProductData
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Assigned { get; set; }
    }
}