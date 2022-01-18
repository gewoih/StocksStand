using Microsoft.EntityFrameworkCore;
using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Repositories.Base;
using System.Linq;

namespace StocksStand.Repositories
{
	public class IndustriesRepository : BaseRepository<Industry>
	{
		public IndustriesRepository(BaseDataContext dbContext) : base(dbContext) { }

		public override IQueryable<Industry> GetAll()
		{
			return base.GetAll().Include(i => i.Stocks);
		}
	}
}
