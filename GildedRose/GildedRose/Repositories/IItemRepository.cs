using System.Collections.Generic;
using GildedRose.Dtos;
using GildedRose.Models;

namespace GildedRose.Repositories
{
	public interface IItemRepository
	{
		Item GetItemById(int id);
		ICollection<ItemDto> GetItemsInStock();
	}
}