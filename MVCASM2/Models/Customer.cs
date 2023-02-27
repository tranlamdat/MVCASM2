using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCASM2.Models
{
    public class Customer
    {
        [Key] //Create PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto increment
        public int Cus_Id { get; set; }
        [Required(ErrorMessage = "Id khong the bo trong")]
        [StringLength(50)]
        public string Cus_Name { get; set; }
		[Required(ErrorMessage = "Ten khong the bo trong")]
		public DateTime Cus_Birthday { get; set; }
		[Required(ErrorMessage = "Ngay sinh khong the bo trong")]
		public string Cus_Gender { get; set; }
		[Required(ErrorMessage = "xin hay chon gioi tinh")]
		public string Cus_Address { get; set; }
        public virtual Order? Order { get; set; }
    }
}
