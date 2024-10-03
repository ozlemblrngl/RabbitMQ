using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatermarkApp_RabbitMQ.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[StringLength(100)]
		public string Name { get; set; }

		[Column(TypeName = "decimal(18,2")] // 16 karakteri virgülden önce 2 karakteri virgülden sonra
		public decimal Price { get; set; }

		[Range(1, 100)]
		public int Stock { get; set; }

		[StringLength(100)]
		public string ImageName { get; set; }
	}
}
