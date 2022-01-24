using Newtonsoft.Json;
using StocksStand.DataContext;
using StocksStand.Models.Base;
using StocksStand.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Windows;

namespace StocksStand.Models.Abstractions
{
	public abstract class AFinancialInstrument : Entity
	{
		#region Properties
		private string _Name;
		public string Name
		{
			get => _Name;
			set => Set(ref _Name, value);
		}

		private string _Ticker;
		public string Ticker
		{
			get => _Ticker;
			set => Set(ref _Ticker, value);
		}

		private Country _Country;
		public Country Country
		{
			get => _Country;
			set => Set(ref _Country, value);
		}

		private ObservableCollection<Quote> _Quotes;
		public ObservableCollection<Quote> Quotes
		{
			get => _Quotes;
			set => Set(ref _Quotes, value);
		}
		#endregion

		#region Methods
		public abstract int LoadQuotes();
		#endregion
	}
}
