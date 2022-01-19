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
			return base.GetAll();
		}
	}
}
