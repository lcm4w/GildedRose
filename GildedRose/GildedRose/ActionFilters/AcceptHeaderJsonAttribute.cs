using System.Net.Http.Headers;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace GildedRose.ActionFilters
{
	public class AcceptHeaderJsonAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			actionContext.Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}