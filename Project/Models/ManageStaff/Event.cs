using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.ManageStaff
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventID { get; set; }
        [StringLength(100)]
        [Display(Name = "Class")]
        public string Subject { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        [StringLength(10)]
        [Display(Name = "Theme Color")]
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        public string Trainer { get; set; }
        public decimal Cost { get; set; }
    }
}