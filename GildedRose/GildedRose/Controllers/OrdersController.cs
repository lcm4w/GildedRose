using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GildedRose.AuthFilters;

namespace GildedRose.Controllers
{
	[RoutePrefix("orders")]
	public class OrdersController : ApiController
	{
		// POST orders
		[BasicAuthFilter]
		[HttpPost, Route("")]
		public void Post([FromBody]string value)
		{
		}
	}
}