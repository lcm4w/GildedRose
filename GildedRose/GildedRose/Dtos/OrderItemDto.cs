using GildedRose.Dtos;

namespace GildedRose.Models
{
	public class OrderItemDto
	{
		public int ItemId { get; set; }

		public ItemFromOrderDto Item { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }

		public decimal LinePrice { get; set; }
	}
}