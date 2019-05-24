using GildedRose.Controllers;
using GildedRose.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GildedRose.Tests.Extensions;
using GildedRose.Repositories;
using FluentAssertions;
using System.Web.Http.Results;
using GildedRose.Models;
using System.Collections.ObjectModel;
using GildedRose.Dtos;
using System.Threading.Tasks;

namespace GildedRose.Tests.Controllers
{
	[TestClass]
	public class OrdersControllerTest
	{
		private OrdersController _controller;
		private Item _item;
		private int _itemId;
		private int _quantity;

		[TestInitialize]
		public void TestInitialize()
		{
			_itemId = 1;
			_quantity = 10;

			_item = new Item
			{
				Id = _itemId,
				Sku = "SKU 1",
				Name = "Item 1",
				Description = "Description 1",
				Price = 5,
				Quantity = _quantity
			};

			var mockItemRepository = new Mock<IItemRepository>();
			var mockOrderRepository = new Mock<IOrderRepository>();

			var mockUoW = new Mock<IUnitOfWork>();
			mockUoW.SetupGet(u => u.Orders).Returns(mockOrderRepository.Object);
			mockUoW.SetupGet(u => u.Items).Returns(mockItemRepository.Object);
			mockUoW.Setup(u => u.Items.GetItemById(_itemId)).Returns(_item);

			_controller = new OrdersController(mockUoW.Object);
			_controller.MockCurrentUser("1", "user1@domain.com");
		}

		[TestMethod]
		public async Task Post_MultipleSameItem_ShouldReturnBadRequest()
		{
			var dto = new OrderPostDto
			{
				OrderItems = new Collection<OrderItemPostDto>
				{
					new OrderItemPostDto()
					{
						ItemId = _itemId,
						Quantity = 1
					},
					new OrderItemPostDto()
					{
						ItemId = _itemId,
						Quantity = 2
					}
				}
			};

			var result = await _controller.Post(dto);

			result.Should().BeOfType<BadRequestErrorMessageResult>();
		}

		[TestMethod]
		public async Task Post_NoItemWithGivenIdExists_ShouldReturnNotFound()
		{
			var dto = new OrderPostDto
			{
				OrderItems = new Collection<OrderItemPostDto>
				{
					new OrderItemPostDto()
					{
						ItemId = _itemId + 1,
						Quantity = _quantity - 1
					}
				}
			};

			var result = await _controller.Post(dto);

			result.Should().BeOfType<NotFoundResult>();
		}

		[TestMethod]
		public async Task Post_NotEnoughStock_ShouldReturnBadRequest()
		{
			var dto = new OrderPostDto
			{
				OrderItems = new Collection<OrderItemPostDto>
				{
					new OrderItemPostDto()
					{
						ItemId = _itemId,
						Quantity = _quantity + 1
					}
				}
			};

			var result = await _controller.Post(dto);

			result.Should().BeOfType<BadRequestErrorMessageResult>();
		}

		[TestMethod]
		public async Task Post_ValidRequest_ShouldReturnCreated()
		{
			var dto = new OrderPostDto
			{
				OrderItems = new Collection<OrderItemPostDto>
				{
					new OrderItemPostDto()
					{
						ItemId = _itemId,
						Quantity = _quantity - 1
					}
				}
			};

			var result = await _controller.Post(dto);

			result.Should().BeOfType<CreatedAtRouteNegotiatedContentResult<OrderDto>>();
		}
	}
}