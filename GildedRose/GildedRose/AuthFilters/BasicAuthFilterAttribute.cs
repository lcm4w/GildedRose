using GildedRose.Models;
using GildedRose.Persistence;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace GildedRose.AuthFilters
{
	/// <summary>
	/// Authentication filter template, with attribute support so it can be set per-controller or per-route.
	/// </summary>
	public class BasicAuthFilterAttribute : Attribute, IAuthenticationFilter
	{
		private ApplicationDbContext _context { get; set; }

		/// <summary>
		/// Set to the Authorization header Scheme value that this filter is intended to support.
		/// </summary>
		public const string SupportedTokenScheme = "Basic";

		public bool AllowMultiple { get { return false; } }

		/// <summary>
		/// True if the filter supports WWW-Authenticate challenge headers; defaults to false.
		/// </summary>
		public bool SendChallenge { get; set; }

		public BasicAuthFilterAttribute()
		{
			_context = new ApplicationDbContext();
		}

		public async Task AuthenticateAsync(HttpAuthenticationContext context,
			CancellationToken cancellationToken)
		{
			var authHeader = context.Request.Headers.Authorization;

			// If there are no credentials or scheme is not basic, return an error.
			// Normally, this should just abort out (no error) because other filters may handle the case
			if (authHeader == null || !authHeader.Scheme.Equals(SupportedTokenScheme))
			{
				context.ErrorResult = new AuthenticationFailureResult(context.Request);
				return;
			}

			var credentials = authHeader.Parameter;
			if (String.IsNullOrEmpty(credentials))
			{
				// If there are no credentials sent with the scheme, return an error.
				context.ErrorResult = new AuthenticationFailureResult("Missing credentials", context.Request);
				return;
			}

			// Return an error if invalid, else set the IPrincipal on the context.
			IPrincipal principal = await ValidateCredentialsAsync(credentials, cancellationToken);
			if (principal == null)
			{
				context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", context.Request);
			}
			else
			{
				// We have a valid, authenticated user; save off the IPrincipal instance
				context.Principal = principal;
			}
		}

		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			// If this filter wants to support WWW-Authenticate header challenges, add one to the result.
			if (SendChallenge)
			{
				context.Result = new AddChallengeOnUnauthorizedResult(
					new AuthenticationHeaderValue(SupportedTokenScheme),
					context.Result);
			}

			return Task.FromResult(0);
		}

		private async Task<IPrincipal> ValidateCredentialsAsync(string credentials, CancellationToken cancellationToken)
		{
			var subject = ParseBasicAuthCredential(credentials);

			var store = new UserStore<ApplicationUser>(_context);
			var manager = new ApplicationUserManager(store);
			var user = await manager.FindByNameAsync(subject.Item1);

			if (!await manager.CheckPasswordAsync(user, subject.Item2))
				return null;

			var identity = await manager.CreateIdentityAsync(user, SupportedTokenScheme);
			identity.AddClaims(new List<Claim> {
				new Claim(ClaimTypes.Name, subject.Item1),
				new Claim(ClaimTypes.AuthenticationInstant, DateTime.UtcNow.ToString("o"))
			});

			var principal = new ClaimsPrincipal(identity);

			return await Task.FromResult(principal);
		}

		/// <summary>
		/// Parse a basic auth credential string into username and password
		/// </summary>
		private Tuple<string, string> ParseBasicAuthCredential(string credential)
		{
			string password = null;
			var subject = (Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(credential)));
			if (String.IsNullOrEmpty(subject))
				return new Tuple<string, string>(null, null);

			if (subject.Contains(":"))
			{
				var index = subject.IndexOf(':');
				password = subject.Substring(index + 1);
				subject = subject.Substring(0, index);
			}

			return new Tuple<string, string>(subject, password);
		}

	}
}