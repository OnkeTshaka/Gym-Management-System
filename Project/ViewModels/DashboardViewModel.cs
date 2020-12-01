using Project.Models.ManageStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class DashboardViewModel
    {
        public int Trainers { get; set; }
        public int Drivers { get; set; }
        public int Reasons { get; set; }
        public int Products { get; set; }
        public int Categories { get; set; }
        public int Orders { get; set; }
        public int ReturnItems { get; set; }
        public int Users { get; set; }
        public int Reviews { get; set; }
        public int Classes { get; set; }
        public int Packages { get; set; }
        public int MembersWithPlan { get; set; }
        public IEnumerable<Member> Members { get; set; }
        public IEnumerable<ContactUsForm> ContactUsForms { get; set; }
    }
}