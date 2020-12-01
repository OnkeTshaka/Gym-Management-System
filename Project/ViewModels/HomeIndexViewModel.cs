using Project.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class HomeIndexViewModel
    {
        //public PagedList.IPagedList<Product> Products { get; set; }
        public IEnumerable<supplierProduct> supplierProducts { get; set; }
    }
}