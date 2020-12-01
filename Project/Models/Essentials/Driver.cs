using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Essentials
{
    public class Driver
    {

        [Key, ScaffoldColumn(false), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverID { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Driver Name")]
        [StringLength(100, MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "This Field Is Required, Enter Your Name in Text Pattern Only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Identity Number")]
        [StringLength(14, MinimumLength = 13)]
        [DataType(DataType.Text, ErrorMessage = "This Field Is Required, Enter Your Identity Number in Text Pattern Only")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Driver Licence")]
        [DataType(DataType.Text, ErrorMessage = "This Field Is Required, Enter Your Driving Licence Number in Text Pattern Only")]
        public string DriverLicence { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Phone Number")]
        [StringLength(11, ErrorMessage = "Enter Valid Phone Number", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "This Field Is Required, Enter Your Phone Number in Phone Pattern Only")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [EmailAddress(ErrorMessage = "Enter a Valid Email Address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This Field Is Required, Enter Your Email Address.")]
        public string EmailAddress { get; set; }

        //[Display(Name = "Profile Picture")]
        //public string ProfilePicture { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        public Driver()
        {
            IsAvailable = true;
        }

    }
}