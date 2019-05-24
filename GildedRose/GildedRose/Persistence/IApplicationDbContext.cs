using System.Data.Entity;
using GildedRose.Models;

namespace GildedRose.Persistence
{
	public interface IApplicationDbContext
	{
		DbSet<Item> Items { get; set; }
		DbSet<OrderItem> OrderItems { get; set; }
		DbSet<Order> Orders { get; set; }
	}
}