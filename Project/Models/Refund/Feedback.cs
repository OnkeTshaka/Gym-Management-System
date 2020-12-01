using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Refund
{
    public class Feedback
    {
        [Key, ScaffoldColumn(false), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }
        public string Reasons { get; set; }
        [StringLength(1000), Display(Name = "Other Reasons"), DataType(DataType.MultilineText)]
        public string Other { get; set; }

    }
}