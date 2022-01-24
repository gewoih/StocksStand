using Microsoft.EntityFrameworkCore;
using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Repositories.Base;
using System.Linq;

namespace StocksStand.Repositories
{
	public class StocksRepository : BaseRepository<Stock>
	{
		public StocksRepository(BaseDataContext dbContext) : base(dbContext) { }

		public override IQueryable<Stock> GetAll()
		{
			return base.GetAll().Include(s => s.Quotes);
		}
	}
}
