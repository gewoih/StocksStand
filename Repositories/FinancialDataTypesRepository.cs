using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksStand.Repositories
{
	public class FinancialDataTypesRepository : BaseRepository<FinancialDataType>
	{
		public FinancialDataTypesRepository(BaseDataContext dbContext) : base(dbContext) { }

		public override IQueryable<FinancialDataType> GetAll()
		{
			return base.GetAll();
		}
	}
}
