using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCASM2.Models
{
	public class OrderDetail
	{
		[Key]
		[Column(Order = 1)]
		public int Order_Id { get; set; }
		[Key]
		[Column(Order = 2)]
		public int Quantity { get; set; }
		public int Pro_Id { get; set; }
		[ForeignKey("Order_Id")]
		public virtual Order? Order { get; set; }
		[ForeignKey("Pro_Id")]
		public virtual Product? Product { get; set;}

	}
}
