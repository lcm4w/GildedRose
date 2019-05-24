using System.Web.Http;
using GildedRose.ActionFilters;
using GildedRose.Persistence;

namespace GildedRose.Controllers
{
	[RoutePrefix("items")]
	public class ItemsController : ApiController
	{
		private readonly IUnitOfWork _unitOfWork;

		public ItemsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET items
		[AcceptHeaderJson]
		[HttpGet, Route("")]
		public IHttpActionResult Get()
		{
			return Ok(_unitOfWork.Items.GetItemsInStock());
		}
	}
}