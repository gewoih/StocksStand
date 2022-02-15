using Microsoft.EntityFrameworkCore;
using StocksStand.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace StocksStand.DataContext
{
	public class BaseDataContext : DbContext
	{
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Sector> Sectors { get; set; }
		public DbSet<Industry> Industries { get; set; }
		public DbSet<Stock> Stocks { get; set; }
		public DbSet<Quote> Quotes { get; set; }
		public DbSet<FinancialDataType> FinancialDataTypes { get; set; }
		public DbSet<FinancialDataValue> FinancialDataValues { get; set; }

		public BaseDataContext() : base()
		{
			/*Database.EnsureDeleted();
			Database.EnsureCreated();

			var c1 = new Currency { Fullname = "United States Dollar", Shortname = "USD", Symbol = '$' };
			Currencies.AddRange(c1);

			var co1 = new Country { Currency = c1, NameRu = "США", NameEn = "USA", Code = "US" };
			Countries.AddRange(co1);

			var s1 = new Sector { Name = "Технологии", Country = co1 };
			var s2 = new Sector { Name = "Финансы", Country = co1 };
			Sectors.AddRange(s1, s2);

			var i1 = new Industry { Name = "Банки", Sector = s2 };
			var i2 = new Industry { Name = "Полупроводники", Sector = s1 };
			var i3 = new Industry { Name = "Разработка ПО", Sector = s1 };
			Industries.AddRange(i1, i2, i3);

			var fd1 = new FinancialDataType { enName = "cashAndCashEquivalents", ruName = "Денежные средства и их эквиваленты" };
			var fd2 = new FinancialDataType { enName = "totalDebt", ruName = "Общая задолженность" };
			var fd3 = new FinancialDataType { enName = "revenue", ruName = "Общий доход" };
			var fd4 = new FinancialDataType { enName = "freeCashFlow", ruName = "Свободные денежные средства" };
			var fd5 = new FinancialDataType { enName = "totalEquity", ruName = "Общий капитал" };
			var fd6 = new FinancialDataType { enName = "eps", ruName = "Прибыль на акцию" };
			var fd7 = new FinancialDataType { enName = "grossProfit", ruName = "Валовая прибыль" };
			var fd8 = new FinancialDataType { enName = "operatingCashFlow", ruName = "Операционный денежный поток" };
			var fd9 = new FinancialDataType { enName = "ebitda", ruName = "Прибыль до вычета процентов, налогов и амортизации" };
			FinancialDataTypes.AddRange(fd1, fd2, fd3, fd4, fd5, fd6, fd7, fd8, fd9);

			var st1 = new Stock { Ticker = "jpm", Industry = i1 };
			var st2 = new Stock { Ticker = "aapl", Industry = i3 };
			var st3 = new Stock { Ticker = "amat", Industry = i2 };
			var st4 = new Stock { Ticker = "msft", Industry = i3 };
			st1.LoadParamsByTicker();
			st2.LoadParamsByTicker();
			st3.LoadParamsByTicker();
			st4.LoadParamsByTicker();
			Stocks.AddRange(st1, st2, st3, st4);

			this.SaveChanges();*/
		}

		public BaseDataContext(DbContextOptions options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, new MySqlServerVersion(new Version(5, 7, 27)));
				//optionsBuilder.EnableSensitiveDataLogging(true);
				//optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
