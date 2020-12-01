
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.ManageStaff
{
    public class Trainer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainerID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name="Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Contact_No { get; set; }
        public int Experience { get; set; }
        [Required, StringLength(10000), Display(Name = "Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        [Display(Name = "ID Number")]
        [RegularExpression(@"^\(?([0-9]{6})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid ID number, Enter 13 numbers!")]
        public string ID_No { get; set; }
        [Display(Name = "Class Name")]
        public string SessionName { get; set; }
        [Display(Name = "Class Description")]
        public string ClassDescription { get; set; }
        public int DOB { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string century { get; set; }
        [ForeignKey("Session")]
        public int SessionID { get; set; }
        public virtual Session Session { get; set; }
        public virtual string getClass()
        {
            var db = new ApplicationDbContext();
            var name = (from s in db.Sessions
                          where s.SessionID == SessionID
                          select s.SessionType
                          ).FirstOrDefault();

            return name;
        }
        public string classDescript()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var desc = (from d in db.Sessions
                        where d.SessionID == SessionID
                        select d.Description).FirstOrDefault();
            return desc;
        }
        public int calcyear()
        {
            int year = '0';
            string yearfromid = ID_No.Substring(0, 2);
            if (century == "20th")
            {
                year = int.Parse("19" + yearfromid);
            }
            else if (century == "21st")
            {
                year = int.Parse(20 + yearfromid);
            }
            return (year);
        }
        public int calcage()
        {
            int age = '0';
            age = DateTime.Now.Year - calcyear();
            return (age);
        }

        public string getgender()
        {
            string gender = "";
            int gendervalue = int.Parse(ID_No.Substring(6, 4));

            if (gendervalue < 5000)
            {
                gender = "female";
            }
            else
            {
                gender = "male";
            }
            return (gender);
        }

    }
}