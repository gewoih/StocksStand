using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace StocksStand.DataContext
{
	public class BaseDataContext : DbContext
	{
		public BaseDataContext()
		{
			Database.EnsureCreated();
		}

		public BaseDataContext(DbContextOptions<BaseDataContext> contextOptions) : base(contextOptions) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, new MySqlServerVersion(new Version(5, 7, 27)));
				optionsBuilder.EnableSensitiveDataLogging(true);
				optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
			}
		}
	}
}
