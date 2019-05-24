using GildedRose.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GildedRose.Persistence
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
	{
		public DbSet<Item> Items { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
	}
}