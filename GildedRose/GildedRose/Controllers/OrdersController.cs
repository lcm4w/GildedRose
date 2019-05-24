using System;
using System.Collections.ObjectModel;
using System.Web.Http;
using GildedRose.ActionFilters;
using GildedRose.AuthFilters;
using GildedRose.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using GildedRose.Persistence;

namespace GildedRose.Controllers
{
	[BasicAuthFilter]
	[RoutePrefix("orders")]
	public class OrdersController : ApiController
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrdersController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET orders/1
		[HttpGet, Route("{id}", Name = "GetOrder")]
		public async Task<IHttpActionResult> Get(int id)
		{
			var orderDto = await _unitOfWork.Orders.GetOrderAsync(id, User.Identity.GetUserId());

			if (orderDto == null)
				return Content(HttpStatusCode.NotFound, "Order does not exist.");

			return Ok(orderDto);
		}

		// POST orders
		[ValidateOrders(BodyRequired = true)]
		[HttpPost, Route("")]
		public async Task<IHttpActionResult> Post([FromBody]OrderPostDto orders)
		{
			var order = new Order
			{
				CustomerId = User.Identity.GetUserId(),
				OrderDate = DateTime.Now
			};

			var orderItems = new Collection<OrderItem>();

			foreach (var orderItemDto in orders.OrderItems)
			{
				var item = _unitOfWork.Items.GetItemById(orderItemDto.ItemId);
				if (item == null)
					return Content(HttpStatusCode.NotFound, "Item does not exist.");

				var orderItem = new OrderItem
				{
					ItemId = orderItemDto.ItemId,
					Order = order,
					Price = item.Price,
					Quantity = orderItemDto.Quantity,
					LinePrice = orderItemDto.Quantity * item.Price
				};

				orderItems.Add(orderItem);
			}

			order.OrderItems = orderItems;
			order.TotalPrice = orderItems.Sum(oi => oi.LinePrice);

			_unitOfWork.Orders.Add(order);

			await _unitOfWork.CompleteAsync();

			return CreatedAtRoute("GetOrder",
				new { order.Id },
				await _unitOfWork.Orders.GetOrderAsync(order.Id, User.Identity.GetUserId())
				);
		}
	}
}