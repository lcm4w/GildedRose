using System.Threading.Tasks;
using GildedRose.Dtos;
using GildedRose.Models;

namespace GildedRose.Repositories
{
	public interface IOrderRepository
	{
		void Add(Order order);
		OrderDto GetOrder(int orderId, string customerId);
	}
}