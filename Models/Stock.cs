using Newtonsoft.Json;
using StocksStand.DataContext;
using StocksStand.Models.Abstractions;
using StocksStand.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
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

		public override int LoadQuotes()
		{
			try
			{
				//Скачиваем файл через Stooq.com
				WebClient webClient = new WebClient();
				string url = $"https://stooq.com/q/d/l/?s={this.Ticker}.{this.Industry.Sector.Country.Code}&i=d";
				string savePath = "";
				string fileName = savePath + this.Name;
				webClient.DownloadFile(url, fileName);

				//Читаем все строки в List
				List<string> lines = File.ReadAllLines(fileName).ToList();
				//Удаляем заголовок
				lines.RemoveAt(0);

				//Проходим по List и создаем котировки
				foreach(var line in lines)
				{
					List<string> quotes = line.Split(',').ToList();
					this.Quotes.Add(
						new Quote
						{
							FinancialInstrument = this,
							Date = DateTime.Parse(quotes.ElementAtOrDefault(0)),
							OpenPrice = Double.Parse(quotes.ElementAtOrDefault(1), CultureInfo.InvariantCulture),
							HighPrice = Double.Parse(quotes.ElementAtOrDefault(2), CultureInfo.InvariantCulture),
							LowPrice = Double.Parse(quotes.ElementAtOrDefault(3), CultureInfo.InvariantCulture),
							ClosePrice = Double.Parse(quotes.ElementAtOrDefault(4), CultureInfo.InvariantCulture),
							//Volume = Double.Parse(quotes.ElementAtOrDefault(5), CultureInfo.InvariantCulture)
						});
				}
				//Обновляем репозиторий
				new StocksRepository(new BaseDataContext()).Update(this);

				/*WebClient webClient = new WebClient();
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
			   new StocksRepository(new BaseDataContext()).Update(this);*/
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
