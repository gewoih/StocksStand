using Microsoft.EntityFrameworkCore;
using StocksStand.Models;
using System;
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

		public BaseDataContext() : base()
		{
			/*Database.EnsureDeleted();
			Database.EnsureCreated();

			var s1 = new Sector { Name = "Технологии" };
			var s2 = new Sector { Name = "Финансы" };
			Sectors.AddRange(s1, s2);

			var i1 = new Industry { Name = "Банки", Sector = s2 };
			var i2 = new Industry { Name = "Полупроводники", Sector = s1 };
			var i3 = new Industry { Name = "Разработка ПО", Sector = s1 };
			Industries.AddRange(i1, i2, i3);

			var c1 = new Currency { Fullname = "United Stated Dollar", Shortname = "USD", Symbol = '$' };
			Currencies.AddRange(c1);

			var co1 = new Country { Currency = c1, Name = "США" };
			Countries.AddRange(co1);

			var st1 = new Stock { Country = co1, Industry = i1, Name = "Bank of America", Ticker = "BAC" };
			var st2 = new Stock { Country = co1, Industry = i2, Name = "Texas Instruments", Ticker = "TXN" };
			var st3 = new Stock { Country = co1, Industry = i2, Name = "Applied Materials", Ticker = "AMAT" };
			var st4 = new Stock { Country = co1, Industry = i3, Name = "Microsoft", Ticker = "MSFT" };
			Stocks.AddRange(st1, st2, st3, st4);

			this.SaveChanges();*/
		}

		public BaseDataContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, new MySqlServerVersion(new Version(5, 7, 27)));
				optionsBuilder.EnableSensitiveDataLogging(true);
				optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
