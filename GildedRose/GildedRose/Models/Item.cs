using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace GildedRose.Models
{
	public class Item
	{
		public int Id { get; set; }

		[Required]
		[StringLength(20)]
		public string Sku { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[StringLength(255)]
		public string Description { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }
	}
}