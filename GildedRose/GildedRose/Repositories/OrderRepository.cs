using AutoMapper.QueryableExtensions;
using GildedRose.Dtos;
using GildedRose.Models;
using GildedRose.Persistence;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<OrderDto> GetOrderAsync(int orderId, string customerId)
		{
			return await _context.Orders
							.Where(o => o.Id == orderId && o.CustomerId == customerId)
							.Include(o => o.Customer)
							.Include(o => o.OrderItems.Select(oi => oi.Item))
							.ProjectTo<OrderDto>()
							.SingleOrDefaultAsync();
		}

		public void Add(Order order)
		{
			_context.Orders.Add(order);
		}
	}
}