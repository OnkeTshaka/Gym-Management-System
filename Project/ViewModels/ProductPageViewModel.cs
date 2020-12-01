using Project.Models.OnlineShopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class ProductPageViewModel
    {
        public PagedList.IPagedList<Product> Products { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}