using Microsoft.EntityFrameworkCore;
using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Repositories.Base;
using System.Linq;

namespace StocksStand.Repositories
{
	public class QuotesRepository : BaseRepository<Quote>
	{
		public QuotesRepository(BaseDataContext dbContext) : base(dbContext) { }

		public override IQueryable<Quote> GetAll()
		{
			return base.GetAll();
		}
	}
}
