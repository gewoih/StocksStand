using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StocksStand.DataContext;
using StocksStand.Models.Abstractions;
using StocksStand.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Windows;

namespace StocksStand.Models
{
	public class Stock : AFinancialInstrument
	{
		public Stock()
		{
			this.Quotes = new System.Collections.ObjectModel.ObservableCollection<Quote>();
		}

		private List<FinancialDataType> _FinancialData;
		public List<FinancialDataType> FinancialData
		{
			get => _FinancialData;
			set => Set(ref _FinancialData, value);
		}

		public override int LoadQuotes()
		{
			//Счетчик добавленных котировок
			int quotesCounter = 0;

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

					//Если в котировках акции уже есть котировка с такой датой, то добавлять такую же котировку не нужно
					if (this.Quotes.FirstOrDefault(q => q.Date == DateTime.Parse(quotes.ElementAtOrDefault(0))) == null)
					{
						this.Quotes.Add(new Quote
						{
							FinancialInstrument = this,
							Date = DateTime.Parse(quotes.ElementAtOrDefault(0)),
							OpenPrice = Double.Parse(quotes.ElementAtOrDefault(1), CultureInfo.InvariantCulture),
							HighPrice = Double.Parse(quotes.ElementAtOrDefault(2), CultureInfo.InvariantCulture),
							LowPrice = Double.Parse(quotes.ElementAtOrDefault(3), CultureInfo.InvariantCulture),
							ClosePrice = Double.Parse(quotes.ElementAtOrDefault(4), CultureInfo.InvariantCulture),
							//Volume = Double.Parse(quotes.ElementAtOrDefault(5), CultureInfo.InvariantCulture)
						});
						quotesCounter++;
					}
				}
				//Обновляем репозиторий
				new StocksRepository(new BaseDataContext()).Update(this);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return 0;
			}
			
			return quotesCounter;
		}

		public int LoadFinancialData()
		{
			//Счетчик загруженных финансовых показателей
			int loadedDataCounter = 0;

			try
			{
				WebClient webClient = new WebClient();
				List<dynamic> jsonObjects = new List<dynamic>();
				jsonObjects.Add(JsonConvert.DeserializeObject(webClient.DownloadString($"https://financialmodelingprep.com/api/v3/cash-flow-statement/{this.Ticker}?apikey={ConfigurationManager.AppSettings["FinancialModelingPrepApi"]}")));
				Thread.Sleep(1000);
				jsonObjects.Add(JsonConvert.DeserializeObject(webClient.DownloadString($"https://financialmodelingprep.com/api/v3/balance-sheet-statement/{this.Ticker}?apikey={ConfigurationManager.AppSettings["FinancialModelingPrepApi"]}")));
				Thread.Sleep(1000);
				jsonObjects.Add(JsonConvert.DeserializeObject(webClient.DownloadString($"https://financialmodelingprep.com/api/v3/income-statement/{this.Ticker}?apikey={ConfigurationManager.AppSettings["FinancialModelingPrepApi"]}")));

				var financialDataTypesRepository = new FinancialDataTypesRepository(new BaseDataContext());
				var financialDataTypes = financialDataTypesRepository.GetAll().ToList();
				foreach (var dataType in financialDataTypes) //Проходим по всем существующим финансовым показателям
				{
					foreach (var jsonObject in jsonObjects) //Проходим по всем Api ответам в формате Json
					{
						if (jsonObject[0][dataType.enName] != null)
						{
							foreach (var property in jsonObject) //Проходим по данным за последние 5 лет
							{
								if (dataType.Values.FirstOrDefault(v => v.FinancialDataType == dataType && v.date == (DateTime)property["date"]) == null)
								{
									dataType.Values.Add(
										new FinancialDataValue
										{
											Stock = this,
											FinancialDataType = dataType,
											date = property["date"],
											value = property[dataType.enName]
										});

									loadedDataCounter++;
								}
							}
						}
					}
					financialDataTypesRepository.Update(dataType);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

			return loadedDataCounter;
		}
	}
}
