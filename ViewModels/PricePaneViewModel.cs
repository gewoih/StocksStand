using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using StocksStand.Models;
using StocksStand.Models.Abstractions;
using StocksStand.Models.Enums;
using StocksStand.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

		public void UpdateDataSeriesByTimeframe(CandleSize candleSize)
		{
			foreach (var dataSeriesViewModel in this.ChartSeriesViewModels)
			{
				if (dataSeriesViewModel.DataSeries is IOhlcDataSeries ohlcDataSeries)
				{
					var sectionedSeries = this.FinancialInstrument.Quotes.GroupBy(q => new KeyValuePair<int, int>(q.Date.Year, q.Date.DayOfYear));
					if (candleSize == CandleSize.Weekly)
						sectionedSeries = this.FinancialInstrument.Quotes.GroupBy(q => new KeyValuePair<int, int>(q.Date.Year, new GregorianCalendar().GetWeekOfYear(q.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)));
					else if (candleSize == CandleSize.Monthly)
						sectionedSeries = this.FinancialInstrument.Quotes.GroupBy(q => new KeyValuePair<int, int>(q.Date.Year, q.Date.Month));

					var newDataSeries = new OhlcDataSeries<DateTime, double>() { SeriesName = this.FinancialInstrument.Name };
					foreach (var series in sectionedSeries)
					{
						var newDate = series.First().Date;
						var newOpen = series.First().OpenPrice;
						var newClose = series.Last().ClosePrice;
						var newHigh = series.Max(t => t.HighPrice);
						var newLow = series.Min(t => t.LowPrice);

						newDataSeries.Append(newDate, newOpen, newHigh, newLow, newClose);
					}
					dataSeriesViewModel.DataSeries = newDataSeries;
				}
			}
		}
	}
}
