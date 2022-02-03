using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using StocksStand.Models.Abstractions;
using StocksStand.ViewModels.Base;
using System;
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
		   : base(parentViewModel)
		{
			var stockPrices = new OhlcDataSeries<DateTime, double>() { SeriesName = financialInstrument.Name };

			stockPrices.Append
				(
					financialInstrument.Quotes.Select(q => q.Date),
					financialInstrument.Quotes.Select(q => q.OpenPrice),
					financialInstrument.Quotes.Select(q => q.HighPrice),
					financialInstrument.Quotes.Select(q => q.LowPrice),
					financialInstrument.Quotes.Select(q => q.ClosePrice)
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

			YAxisTextFormatting = $"#0.00";
		}
	}
}
