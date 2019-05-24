using GildedRose.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GildedRose.ActionFilters
{
	public class ValidateOrdersAttribute : ActionFilterAttribute
	{
		// True if the bound FromBody parameter is required (disallow nulls)
		public bool BodyRequired { get; set; }

		public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
		{
			if (!actionContext.ModelState.IsValid)
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(
					HttpStatusCode.BadRequest, actionContext.ModelState);
			}
			// If the FromBody parameter is required, find it in the action arguments and check for null
			else if (BodyRequired)
			{
				foreach (var b in actionContext.ActionDescriptor.ActionBinding.ParameterBindings)
				{
					if (b.WillReadBody)
					{
						if (!actionContext.ActionArguments.ContainsKey(b.Descriptor.ParameterName)
							|| actionContext.ActionArguments[b.Descriptor.ParameterName] == null)
						{
							actionContext.Response = actionContext.Request.CreateErrorResponse(
												HttpStatusCode.BadRequest, b.Descriptor.ParameterName + " is required.");
						}
						// Since only one FromBody can exist, we can abort the loop after a body param is found.
						break;
					}
				}
			}

			await base.OnActionExecutingAsync(actionContext, cancellationToken);
		}
	}
}