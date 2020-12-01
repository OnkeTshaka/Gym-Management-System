using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Essentials
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int PackId { get; set; }
        [Display(Name = "Package Type ")]
        public string PackageType { get; set; }
        [StringLength(10000), Display(Name = "Description"), DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }
        public int Duration { get; set; }
        [Display(Name = "Monthly Fee")]
        [Required]
        public double Monthly_Fee { get; set; }

        [Display(Name = "Joining Fee")]
        public double Joining_Fee { get; set; }
        [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }

        public double CalcTotalCost()
        {
            TotalCost = Monthly_Fee + Joining_Fee;
            return TotalCost;

        }

    }
}