namespace GildedRose.Migrations
{
	using GildedRose.Models;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GildedRose.Models.ApplicationDbContext>
    {
		public Configuration()
        {
            AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(GildedRose.Models.ApplicationDbContext context)
        {
			if (!context.Users.Any(u => u.UserName == "test@domain.com"))
			{
				var store = new UserStore<ApplicationUser>(context);
				var manager = new UserManager<ApplicationUser>(store);
				var user = new ApplicationUser { UserName = "test@domain.com" };

				manager.Create(user, "Test1234!");
			}
		}
    }
}