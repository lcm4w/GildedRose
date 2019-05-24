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
		private readonly IApplicationDbContext _context;

		public ItemRepository(IApplicationDbContext context)
		{
			_context = context;
		}

		public ICollection<ItemDto> GetItemsInStock()
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