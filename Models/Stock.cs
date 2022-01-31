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

		public void LoadParamsByTicker()
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Headers.Add("accept: application/json");
				webClient.Headers.Add($"X-API-KEY: {ConfigurationManager.AppSettings["YahooFinanceApi"]}");

				string response = webClient.DownloadString($"https://yfapi.net/v6/finance/quote?symbols={this.Ticker}");
				dynamic obj = JsonConvert.DeserializeObject(response);
				var result = obj.quoteResponse.result;

				if (result.Count == 0)
					throw new Exception("Инструмент с данным тикером не найден!");
				else
				{
					this.Ticker = result[0].symbol;
					this.Name = result[0].shortName;
				}
			}
			catch
			{
				throw;
			}
		}

		public override int LoadQuotes()
		{
			try
			{
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
