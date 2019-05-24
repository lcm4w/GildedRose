using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GildedRose.Tests.Extensions
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