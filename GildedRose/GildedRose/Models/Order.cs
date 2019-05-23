using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GildedRose.Models
{
	public class Order
	{
		public int Id { get; set; }

		[Required]
		public DateTime OrderDate { get; set; }

		public ApplicationUser Customer { get; set; }

		[Required]
		public string CustomerId { get; set; }

		public decimal TotalPrice { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }

		public Order()
		{
			OrderItems = new Collection<OrderItem>();
		}
	}
}