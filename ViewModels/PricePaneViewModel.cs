using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using StocksStand.Models;
using StocksStand.Models.Abstractions;
using StocksStand.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StocksStand.ViewModels
{
	public class PricePaneViewModel : BaseChartPaneViewModel
	{
		public PricePaneViewModel(ChartViewModel parentViewModel, AFinancialInstrument financialInstrument)
		   : base(parentViewModel, financialInstrument)
		{
		}

		protected override void LoadDataSeries()
		{
			var stockPrices = new OhlcDataSeries<DateTime, double>() { SeriesName = this.FinancialInstrument.Name };

			stockPrices.Append
				(
					this.FinancialInstrument.Quotes.Select(q => q.Date),
					this.FinancialInstrument.Quotes.Select(q => q.OpenPrice),
					this.FinancialInstrument.Quotes.Select(q => q.HighPrice),
					this.FinancialInstrument.Quotes.Select(q => q.LowPrice),
					this.FinancialInstrument.Quotes.Select(q => q.ClosePrice)
				);

			this.ChartSeriesViewModels.Add(new CandlestickRenderableSeriesViewModel
			{
				DataSeries = stockPrices,

				AntiAliasing = false,
				FillUp = new SolidColorBrush(Color.FromRgb(38, 166, 154)),
				FillDown = new SolidColorBrush(Color.FromRgb(239, 83, 80)),

				StrokeUp = Color.FromRgb(38, 166, 154),
				StrokeDown = Color.FromRgb(239, 83, 80),

				//Черные и белые свечи
				/*FillUp = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
				FillDown = new SolidColorBrush(Color.FromRgb(0, 0, 0)),

				StrokeUp = Color.FromRgb(255, 255, 255),
				StrokeDown = Color.FromRgb(0, 0, 0),*/
				StrokeThickness = 1
			});

			this.YAxisTextFormatting = $"#0.00";
		}

		public void UpdateDataSeriesByTimeframe(int timeframe)
		{
			foreach (var dataSeriesViewModel in this.ChartSeriesViewModels)
			{
				if (dataSeriesViewModel.DataSeries is IOhlcDataSeries ohlcDataSeries)
				{
					var sectionedSeries = this.FinancialInstrument.Quotes
														.Select((x, y) => new Tuple<int, Quote>(y, x))
														.GroupBy(x => x.Item1 / timeframe)
														.ToList();

					var newDataSeries = new OhlcDataSeries<DateTime, double>() { SeriesName = this.FinancialInstrument.Name };
					for (int i = 0; i < sectionedSeries.Count; i++)
					{
						var newDate = sectionedSeries[i].First().Item2.Date;
						var newOpen = sectionedSeries[i].First().Item2.OpenPrice;
						var newClose = sectionedSeries[i].Last().Item2.ClosePrice;
						var newHigh = sectionedSeries[i].Max(t => t.Item2.HighPrice);
						var newLow = sectionedSeries[i].Min(t => t.Item2.LowPrice);

						newDataSeries.Append(newDate, newOpen, newHigh, newLow, newClose);
					}
					dataSeriesViewModel.DataSeries = newDataSeries;
				}
			}
		}
	}
}
