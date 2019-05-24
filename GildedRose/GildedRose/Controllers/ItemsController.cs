using System.Linq;
using System.Web.Http;
using AutoMapper.QueryableExtensions;
using GildedRose.ActionFilters;
using GildedRose.Dtos;
using GildedRose.Models;

namespace GildedRose.Controllers
{
	[RoutePrefix("items")]
	public class ItemsController : ApiController
	{
		private ApplicationDbContext _context { get; set; }

		public ItemsController()
		{
			_context = new ApplicationDbContext();
		}

		// GET items
		[AcceptHeaderJson]
		[HttpGet, Route("")]
		public IHttpActionResult Get()
		{
			var items = _context.Items
				.Where(i => i.Quantity > 0)
				.ProjectTo<ItemDto>()
				.ToList();

			return Ok(items);
		}
	}
}