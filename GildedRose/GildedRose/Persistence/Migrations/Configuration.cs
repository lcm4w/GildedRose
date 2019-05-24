namespace GildedRose.Migrations
{
	using GildedRose.Models;
	using GildedRose.Persistence;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
		public Configuration()
        {
            AutomaticMigrationsEnabled = false;
			MigrationsDirectory = @"Persistence\Migrations";
		}

		protected override void Seed(ApplicationDbContext context)
        {
			if (!context.Users.Any(u => u.UserName == "test@domain.com"))
			{
				var store = new UserStore<ApplicationUser>(context);
				var manager = new ApplicationUserManager(store);
				var user = new ApplicationUser { UserName = "test@domain.com" };

				manager.Create(user, "Test1234!");
			}
		}
    }
}