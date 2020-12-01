using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.ManageStaff
{
    public class RateBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        public string Comments { get; set; }
        public DateTime? ThisDateTime { get; set; }
        public int ArticleId { get; set; }
        public int? Rating { get; set; }
        [ForeignKey("Member")]
        public int ID { get; set; }
        public virtual Member Member { get; set; }
    }
}