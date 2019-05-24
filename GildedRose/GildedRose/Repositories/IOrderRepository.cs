using System.Threading.Tasks;
using GildedRose.Dtos;
using GildedRose.Models;

namespace GildedRose.Repositories
{
	public interface IOrderRepository
	{
		void Add(Order order);
		Task<OrderDto> GetOrderAsync(int orderId, string customerId);
	}
}