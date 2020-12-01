using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models.Supplier
{
    public class myCart
    {
        [Key, ScaffoldColumn(false), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartID { get; set; }
        public int? ProductID { get; set; }
        public int MemberID { get; set; }
        public int? CartStatusID { get; set; }

        public virtual supplierProduct supplierProduct { get; set; }
    }
}