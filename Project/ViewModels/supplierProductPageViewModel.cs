using Project.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class supplierProductPageViewModel
    {
        public PagedList.IPagedList<supplierProduct> supplierProducts { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}