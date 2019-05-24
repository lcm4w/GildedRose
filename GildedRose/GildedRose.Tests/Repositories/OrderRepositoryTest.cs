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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GildedRose.Tests.Repositories
{
	[TestClass]
	public class OrderRepositoryTest
	{
		private Mock<IApplicationDbContext> _mockContext;
		private OrderRepository _repository;
		private Mock<DbSet<Order>> _mockOrders;
		private List<Order> _orders;
		private int _orderId;
		private string _userId;

		[TestInitialize]
		public void TestInitialize()
		{
			_orderId = 1;
			_userId = "1";

			var identity = new GenericIdentity("user1@domain.com");
			identity.AddClaims(new List<Claim> {
				new Claim(ClaimTypes.Name, "user1@domain.com"),
				new Claim(ClaimTypes.NameIdentifier, _userId)
			});

			var principal = new GenericPrincipal(identity, null);

			_orders = new List<Order>();

			_orders.Add(new Order
			{
				Id = _orderId,
				OrderDate = DateTime.Now,
				Customer = new ApplicationUser() { Id = _userId, UserName = "user1@domain.com" },
				CustomerId = _userId,
				OrderItems = new List<OrderItem>
				{
					new OrderItem()
					{
						ItemId = 1,
						OrderId = _orderId,
						Price = 1,
						LinePrice = 2,
						Quantity = 2
					},
					new OrderItem()
					{
						ItemId = 2,
						OrderId = _orderId,
						Price = 3,
						LinePrice = 3,
						Quantity = 1
					}
				},
				TotalPrice = 5
			});

			Mapper.Reset();
			Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());

			_mockOrders = new Mock<DbSet<Order>>();

			_mockContext = new Mock<IApplicationDbContext>();

			_repository = new OrderRepository(_mockContext.Object);

			_mockOrders.SetSource(_orders);

			_mockContext.SetupGet(c => c.Orders).Returns(_mockOrders.Object);
		}

		[TestMethod]
		public void GetOrder_NoOrderWithGivenIdExists_ShouldBeNull()
		{
			var order = _repository.GetOrder(_orderId + 1, _userId);

			order.Should().BeNull();
		}

		[TestMethod]
		public void GetOrder_NoUserWithGivenIdExists_ShouldBeNull()
		{
			var order = _repository.GetOrder(_orderId, _userId + "-");

			order.Should().BeNull();
		}
		
		[TestMethod]
		public void GetOrder_ValidRequest_ShouldBeEquivalent()
		{
			var order = _repository.GetOrder(_orderId, _userId);

			order.Should().BeEquivalentTo(_orders
											.Take(1)
											.AsQueryable()
											.ProjectTo<OrderDto>()
											.SingleOrDefault()
											);
		}
	}
}