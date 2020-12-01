using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class DriverDashboardViewModel
    {
        public int DeliveryOrders { get; set; }
        public int DeliveryReturns { get; set; }
        public int CompletedReturns { get; set; }
    }
}