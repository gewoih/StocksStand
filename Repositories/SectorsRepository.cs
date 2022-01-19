using Microsoft.EntityFrameworkCore;
using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Repositories.Base;
using System.Linq;

namespace StocksStand.Repositories
{
	public class SectorsRepository : BaseRepository<Sector>
	{
		public SectorsRepository(BaseDataContext dbContext) : base(dbContext) { }

		public override IQueryable<Sector> GetAll()
		{
			return base.GetAll().Include(s => s.Industries).ThenInclude(i => i.Stocks);
		}
	}
}
