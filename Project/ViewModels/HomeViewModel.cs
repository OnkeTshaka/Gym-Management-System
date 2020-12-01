using Project.Models.Essentials;
using Project.Models.ManageStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class HomeViewModel
    {
        public ContactUsForm ContactUsForm { get; set; }
        public IEnumerable<Package> Package { get; set; }
    }
}