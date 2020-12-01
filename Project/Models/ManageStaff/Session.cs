using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.ManageStaff
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionID { get; set; }
        [Display(Name = "Session Type")]
        public string SessionType { get; set; }
        public byte[] Icon { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}