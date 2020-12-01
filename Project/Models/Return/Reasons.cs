using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Return
{
    public class Reason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReasonID { get; set; }
        public string Name { get; set; }
    }
    public class Terms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TermsID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Agreed { get; set; }
    }
}