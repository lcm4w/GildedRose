using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Models
{
	public class OrderItem
	{
		public Order Order { get; set; }

		public Item Item { get; set; }

		[Key]
		[Column(Order = 1)]
		public int OrderId { get; set; }

		[Key]
		[Column(Order = 2)]
		public int ItemId { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }

		public decimal LinePrice { get; set; }

	}
}