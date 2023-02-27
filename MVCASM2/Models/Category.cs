using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCASM2.Models
{
	public class Category
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Cat_Id { get; set; }
		public string Cat_Name { get; set;}
		public string Cat_Des { get; set; }
		public virtual ICollection<Product>? Products { get; set;}
	}
}
