using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace GildedRose.IntegrationTests.Extensions
{
	public static class ApiControllerExtensions
	{
		public static void MockCurrentUser(this ApiController controller, string userId, string userName)
		{
			var identity = new GenericIdentity(userName);
			identity.AddClaims(new List<Claim> {
				new Claim(ClaimTypes.Name, userName),
				new Claim(ClaimTypes.NameIdentifier, userId)
			});

			var principal = new GenericPrincipal(identity, null);

			controller.User = principal;
		}
	}
}