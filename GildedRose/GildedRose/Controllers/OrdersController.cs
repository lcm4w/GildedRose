using System;
using System.Collections.ObjectModel;
using System.Web.Http;
using AutoMapper.QueryableExtensions;
using GildedRose.ActionFilters;
using GildedRose.AuthFilters;
using GildedRose.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Net;
using GildedRose.Dtos;
using System.Data.Entity;

namespace GildedRose.Controllers
{
	[RoutePrefix("orders")]
	public class OrdersController : ApiController
	{
		private ApplicationDbContext _context { get; set; }

		public OrdersController()
		{
			_context = new ApplicationDbContext();
		}

		[BasicAuthFilter]
		[HttpGet, Route("{id}", Name = "GetOrder")]
		public IHttpActionResult Get(int id)
		{
			var orderDto = _context.Orders
				.Where(o => o.Id == id)
				.Include(o => o.Customer)
				.Include(o => o.OrderItems.Select(oi => oi.Item))
				.ProjectTo<OrderDto>()
				.ToList();

			if (orderDto == null)
			{
				return Content(HttpStatusCode.NotFound, "Order does not exist.");
			}

			return Ok(orderDto);
		}
		
		// POST orders
		[BasicAuthFilter]
		[ValidateOrders(BodyRequired = true)]
		[HttpPost, Route("")]
		public async Task<IHttpActionResult> Post([FromBody]OrderPostDto orders)
		{
			var order = new Order
			{
				CustomerId = User.Identity.GetUserId(),
				OrderDate = DateTime.Now
			};
			_context.Orders.Add(order);

			var orderItems = new Collection<OrderItem>();

			foreach (var orderItemDto in orders.OrderItems)
			{
				var item = _context.Items.SingleOrDefault(i => i.Id == orderItemDto.ItemId);

				var orderItem = new OrderItem
				{
					ItemId = orderItemDto.ItemId,
					Order = order,
					Price = item.Price,
					Quantity = orderItemDto.Quantity,
					LinePrice = orderItemDto.Quantity * item.Price
				};

				orderItems.Add(orderItem);
				_context.OrderItems.Add(orderItem);
			}

			order.OrderItems = orderItems;
			order.TotalPrice = orderItems.Sum(oi => oi.LinePrice);

			await _context.SaveChangesAsync();

			var orderDto = _context.Orders
				.Where(o => o.Id == order.Id)
				.Include(o => o.Customer)
				.Include(o => o.OrderItems.Select(oi => oi.Item))
				.ProjectTo<OrderDto>()
				.ToList();

			return CreatedAtRoute("GetOrder", new { order.Id }, orderDto);
		}
	}
}