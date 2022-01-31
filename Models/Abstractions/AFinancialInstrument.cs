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

		private ObservableCollection<Quote> _Quotes;
		public ObservableCollection<Quote> Quotes
		{
			get => _Quotes;
			set => Set(ref _Quotes, value);
		}

		private Industry _Industry;
		public Industry Industry
		{
			get => _Industry;
			set => Set(ref _Industry, value);
		}
		#endregion

		#region Methods
		public abstract int LoadQuotes();
		#endregion
	}
}
