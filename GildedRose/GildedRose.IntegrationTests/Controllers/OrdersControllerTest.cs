using FluentAssertions;
using GildedRose.Controllers;
using GildedRose.Dtos;
using GildedRose.Models;
using GildedRose.Persistence;
using GildedRose.IntegrationTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http.Results;

namespace GildedRose.IntegrationTests.Controllers
{
	[TestClass]
	public class OrdersControllerTest : IntegrationTestsBase
	{
		private OrdersController _controller;
		private ApplicationDbContext _context;

		[TestInitialize]
		public void Setup()
		{
			_context = new ApplicationDbContext();
			_controller = new OrdersController(new UnitOfWork(_context));
		}

		[TestCleanup]
		public void TearDown()
		{
			_context.Dispose();
		}

		[TestMethod]
		public void Post_ValidRequest_ShouldCreateGivenOrder()
		{
			// Arrange
			var user = _context.Users.First();
			_controller.MockCurrentUser(user.Id, user.UserName);

			// Items table pre-state:
			// Id 2 / Quantity 200
			// Id 3 / Quantity 50
			var dto = new OrderPostDto
			{
				OrderItems = new Collection<OrderItemPostDto>
				{
					new OrderItemPostDto()
					{
						ItemId = 2,
						Quantity = 15
					},
					new OrderItemPostDto()
					{
						ItemId = 3,
						Quantity = 27
					}
				}
			};

			// Act
			var result = _controller.Post(dto);

			// Assert
			var contentResult = (result as CreatedAtRouteNegotiatedContentResult<OrderDto>).Content;

			contentResult.Should().NotBeNull();
			_context.Orders.Should().HaveCount(1);
			_context.Orders.First().OrderItems.Should().HaveCount(2);
			_context.Items.Where(i => i.Id == 2).SingleOrDefault().Quantity = 200 - 15;
			_context.Items.Where(i => i.Id == 3).SingleOrDefault().Quantity = 50 - 27;
		}
	}
}