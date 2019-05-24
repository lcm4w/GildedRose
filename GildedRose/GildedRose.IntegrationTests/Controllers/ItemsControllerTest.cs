using FluentAssertions;
using GildedRose.Controllers;
using GildedRose.Dtos;
using GildedRose.Models;
using GildedRose.Persistence;
using GildedRose.IntegrationTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace GildedRose.IntegrationTests.Controllers
{
	[TestClass]
	public class ItemsControllerTest : IntegrationTestsBase
	{
		private ItemsController _controller;
		private ApplicationDbContext _context;

		[TestInitialize]
		public void Setup()
		{
			_context = new ApplicationDbContext();
			_controller = new ItemsController(new UnitOfWork(_context));
		}

		[TestCleanup]
		public void TearDown()
		{
			_context.Dispose();
		}

		[TestMethod]
		public void Get_WhenCalled_ShouldReturnAllItems()
		{
			// Arrange
			var user = _context.Users.First();
			_controller.MockCurrentUser(user.Id, user.UserName);

			var item = new Item
			{
				Id = 5,
				Sku = "SKU 5",
				Name = "Product 5",
				Description = "Desciption for Product 5",
				Price = 100,
				Quantity = 497
			};
			_context.Items.Add(item);
			_context.SaveChanges();

			// Act
			var result = _controller.Get();

			// Assert
			(result as OkNegotiatedContentResult<ICollection<ItemDto>>).Content.Should().HaveCount(5);
		}
	}
}