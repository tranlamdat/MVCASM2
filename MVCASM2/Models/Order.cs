using System.ComponentModel.DataAnnotations;

namespace MVCASM2.Models
{
	public class Order
	{
		[Key]
		public int Order_Id { get; set; }
		public string Cus_Name { get; set; }
		public string DeliveryLocal { get; set; }
		public string Cus_Phone { get; set; }
		[DataType(DataType.Date)]
		public DateTime OrderDate { get; set; }
		[DataType(DataType.Date)]
		public DateTime DeliveryDate { get; set; }
		public virtual ICollection<Customer>? Customers { get; set; }
	
	}
}
