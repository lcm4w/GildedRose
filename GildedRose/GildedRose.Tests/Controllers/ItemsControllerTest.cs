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
using System.Collections.Generic;

namespace GildedRose.Tests.Controllers
{
	[TestClass]
	public class ItemsControllerTest
	{
		private ItemsController _controller;
		private ICollection<ItemDto> _items;

		[TestInitialize]
		public void TestInitialize()
		{
			_items = new List<ItemDto>();

			_items.Add(new ItemDto
			{
				Id = 1,
				Name = "Item 1",
				Description = "Description 1",
				Price = 5
			});
			_items.Add(new ItemDto
			{
				Id = 2,
				Name = "Item 2",
				Description = "Description 2",
				Price = 8
			});

			var mockItemRepository = new Mock<IItemRepository>();

			var mockUoW = new Mock<IUnitOfWork>();
			mockUoW.SetupGet(u => u.Items).Returns(mockItemRepository.Object);
			mockUoW.Setup(u => u.Items.GetItemsInStock()).Returns(_items);

			_controller = new ItemsController(mockUoW.Object);
			_controller.MockCurrentUser("1", "user1@domain.com");
		}

		[TestMethod]
		public void Get_WhenCalled_ShouldReturnOk()
		{
			var result = _controller.Get();

			result.Should().BeOfType<OkNegotiatedContentResult<ICollection<ItemDto>>>();
		}
	}
}
