using Microsoft.EntityFrameworkCore;
using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Repositories.Base;
using System.Linq;

namespace StocksStand.Repositories
{
	public class CountriesRepository : BaseRepository<Country>
	{
		public CountriesRepository(BaseDataContext dbContext) : base(dbContext) { }

		public override IQueryable<Country> GetAll()
		{
			return base.GetAll().Include(c => c.Sectors).ThenInclude(s => s.Industries).ThenInclude(i => i.FinancialInstruments).ThenInclude(fi => fi.Quotes);
		}
	}
}
