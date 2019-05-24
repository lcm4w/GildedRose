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
		private readonly IApplicationDbContext _context;

		public OrderRepository(IApplicationDbContext context)
		{
			_context = context;
		}

		public OrderDto GetOrder(int orderId, string customerId)
		{
			return _context.Orders
							.Where(o => o.Id == orderId && o.CustomerId == customerId)
							.Include(o => o.Customer)
							.Include(o => o.OrderItems.Select(oi => oi.Item))
							.ProjectTo<OrderDto>()
							.SingleOrDefault();
		}

		public void Add(Order order)
		{
			_context.Orders.Add(order);
		}
	}
}