using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GildedRose.AuthFilters;

namespace GildedRose.Controllers
{
	[RoutePrefix("items")]
	public class ItemsController : ApiController
	{
		// GET items
		[HttpGet, Route("")]
		public IEnumerable<string> Get()
		{
			return new string[] { User.Identity.Name, User.Identity.AuthenticationType };
		}
	}
}