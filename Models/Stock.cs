using Newtonsoft.Json;
using StocksStand.DataContext;
using StocksStand.Models.Abstractions;
using StocksStand.Repositories;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Windows;

namespace StocksStand.Models
{
	public class Stock : AFinancialInstrument
	{
		public Stock()
		{
			this.Quotes = new System.Collections.ObjectModel.ObservableCollection<Quote>();
		}

		private Industry _Industry;
		public Industry Industry
		{
			get => _Industry;
			set => Set(ref _Industry, value);
		}

		public override int LoadQuotes()
		{
			//Смотрим на уже существующие котировки. Если их нет или дата самой поздней котировки старше 10 лет, то сначала загружаем
			//через Api Nasdaq, а потом ДОГРУЖАЕМ через Api YahooFinance. ИНАЧЕ просто загружаем через YahooFinance.
			//В итоге получаем самые полные котировки с начала прошлого века по сегодняшнюю дату

			try
			{
				/*WebClient webClient = new WebClient();

				string response = webClient.DownloadString($"https://data.nasdaq.com/api/v3/datasets/WIKI/{this.Ticker}/data.json?order=asc&api_key={ConfigurationManager.AppSettings["NasdaqApi"]}");
				dynamic obj = JsonConvert.DeserializeObject(response);
				var result = obj.dataset_data.data;

				foreach (var quote in result)
				{
					this.Quotes.Add(
						new Quote 
						{ 
							FinancialInstrument = this,
							Date = quote[0],
							OpenPrice = quote[1],
							HighPrice = quote[2],
							LowPrice = quote[3],
							ClosePrice = quote[4],
							Volume = quote[5]
						});
				}*/

				WebClient webClient = new WebClient();
				webClient.Headers.Add("accept: application/json");
				webClient.Headers.Add($"X-API-KEY: {ConfigurationManager.AppSettings["YahooFinanceApi"]}");

				string response = webClient.DownloadString($"https://yfapi.net/v8/finance/chart/{this.Ticker}?range=10y&interval=1d");
				dynamic obj = JsonConvert.DeserializeObject(response);
				var result = obj.chart.result[0];

				for (int i = 0; i < result.timestamp.Count; i++)
				{
					this.Quotes.Add(
						new Quote
						{
							FinancialInstrument = this,
							Date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double)result.timestamp[i]).ToLocalTime(),
							OpenPrice = result.indicators.quote[0].open[i],
							HighPrice = result.indicators.quote[0].high[i],
							LowPrice = result.indicators.quote[0].low[i],
							ClosePrice = result.indicators.quote[0].close[i],
							Volume = result.indicators.quote[0].volume[i]
						});
				}
			   new StocksRepository(new BaseDataContext()).Update(this);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return 0;
			}

			return this.Quotes.Count();
		}
	}
}
