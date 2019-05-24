using GildedRose.Repositories;
using System.Threading.Tasks;

namespace GildedRose.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		public IItemRepository Items { get; private set; }
		public IOrderRepository Orders { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Items = new ItemRepository(context);
			Orders = new OrderRepository(context);
		}

		public async Task CompleteAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}