using AutoMapper;
using GildedRose.Dtos;

namespace GildedRose.Models
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<ApplicationUser, CustomerDto>();
			CreateMap<OrderItem, OrderItemDto>();
			CreateMap<Order, OrderDto>();
			CreateMap<Item, ItemDto>();
			CreateMap<Item, ItemFromOrderDto>();
		}
	}
}