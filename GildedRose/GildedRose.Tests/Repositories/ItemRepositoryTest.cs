using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentAssertions;
using GildedRose.Dtos;
using GildedRose.Models;
using GildedRose.Persistence;
using GildedRose.Repositories;
using GildedRose.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GildedRose.Tests.Repositories
{
	[TestClass]
	public class ItemRepositoryTest
	{
		private Mock<IApplicationDbContext> _mockContext;
		private ItemRepository _repository;
		private Mock<DbSet<Item>> _mockItems;
		private List<Item> _items;

		[TestInitialize]
		public void TestInitialize()
		{
			_items = new List<Item>();

			_items.Add(new Item
			{
				Id = 1,
				Sku = "SKU 1",
				Name = "Item 1",
				Description = "Description 1",
				Price = 5,
				Quantity = 10
			});
			_items.Add(new Item
			{
				Id = 2,
				Sku = "SKU 2",
				Name = "Item 2",
				Description = "Description 2",
				Price = 8,
				Quantity = 0
			});

			Mapper.Reset();
			Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());

			_mockItems = new Mock<DbSet<Item>>();

			_mockContext = new Mock<IApplicationDbContext>();

			_repository = new ItemRepository(_mockContext.Object);

			_mockItems.SetSource(_items);

			_mockContext.SetupGet(c => c.Items).Returns(_mockItems.Object);
		}

		[TestMethod]
		public void GetItemsInStock_WhenCalled_ShouldBeEquivalent()
		{
			var items = _repository.GetItemsInStock();

			items.Should().BeEquivalentTo(_items
									.Take(1)
									.AsQueryable()
									.ProjectTo<ItemDto>()
									.ToList()
									);
		}

		[TestMethod]
		public void GetItemById_NoItemWithGivenIdExists_ShouldBeNull()
		{
			var item = _repository.GetItemById(10);

			item.Should().BeNull();
		}

		[TestMethod]
		public void GetItemById_ValidRequest_ShouldBeEquivalent()
		{
			var item = _repository.GetItemById(1);

			item.Should().BeEquivalentTo(_items.First());
		}
	}
}