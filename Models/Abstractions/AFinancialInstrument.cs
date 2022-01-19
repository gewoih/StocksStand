using StocksStand.Models.Base;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;

namespace StocksStand.Models.Abstractions
{
	public abstract class AFinancialInstrument : Entity
	{
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

		public int LoadQuotesNasdaq()
		{
			WebClient client = new WebClient();

			string result = client.DownloadString($"https://data.nasdaq.com/api/v3/datasets/WIKI/{this.Ticker}/data.json?api_key={ConfigurationManager.AppSettings["NasdaqApi"]}");
			return 1;
		}
	}
}
