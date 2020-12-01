using Project.Models.ManageStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class RatingTrainerViewModel
    {
        public IEnumerable<Trainer> Trainers { get; set; }
    }
}