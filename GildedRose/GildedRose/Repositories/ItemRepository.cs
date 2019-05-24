using AutoMapper.QueryableExtensions;
using GildedRose.Dtos;
using GildedRose.Models;
using GildedRose.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Repositories
{
	public class ItemRepository : IItemRepository
	{
		private readonly ApplicationDbContext _context;

		public ItemRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<ItemDto> GetItemsInStock()
		{
			return _context.Items
							.Where(i => i.Quantity > 0)
							.ProjectTo<ItemDto>()
							.ToList();
		}

		public Item GetItemById(int id)
		{
			return _context.Items.SingleOrDefault(i => i.Id == id);
		}
	}
}