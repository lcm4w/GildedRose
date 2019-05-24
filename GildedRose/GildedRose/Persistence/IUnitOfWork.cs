using System.Threading.Tasks;
using GildedRose.Repositories;

namespace GildedRose.Persistence
{
	public interface IUnitOfWork
	{
		IItemRepository Items { get; }

		IOrderRepository Orders { get; }

		void Complete();

		Task CompleteAsync();
	}
}