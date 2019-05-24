using GildedRose.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GildedRose.Dtos
{
	public class OrderDto
	{
		public int Id { get; set; }

		public DateTime OrderDate { get; set; }

		public CustomerDto Customer { get; set; }

		public decimal TotalPrice { get; set; }

		public ICollection<OrderItemDto> OrderItems { get; set; }

		public OrderDto()
		{
			OrderItems = new Collection<OrderItemDto>();
		}
	}
}