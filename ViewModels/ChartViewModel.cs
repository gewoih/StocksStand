using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;
using StocksStand.Commands;
using StocksStand.Models.Abstractions;
using StocksStand.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using SciChart.Data.Model;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using SciChart.Charting.Visuals.TradeChart;

namespace StocksStand.ViewModels
{
	public class ChartViewModel : BaseViewModel
	{
		#region Constructor
		public ChartViewModel()
		{
			this.FinancialInstruments = new ObservableCollection<SciChartSurface>();

			this.SharedXRange = new IndexRange();
		}
		#endregion

		#region Properties
		//Выбранный Финансовый Инструмент
		private ObservableCollection<SciChartSurface> _FinancialInstruments;
		public ObservableCollection<SciChartSurface> FinancialInstruments
		{
			get => _FinancialInstruments;
			set => Set(ref _FinancialInstruments, value);
		}

		private IRange _SharedXRange;
		public IRange SharedXRange
		{
			get => _SharedXRange;
			set => Set(ref _SharedXRange, value);
		}
		#endregion

		#region Commands
		public void AddFinancialInstrument(AFinancialInstrument financialInstrument)
		{
			IRenderableSeries candlestickRenderableSeries = new FastCandlestickRenderableSeries()
			{
				AntiAliasing = false,

				FillUp = new SolidColorBrush(Color.FromRgb(38, 166, 154)),
				FillDown = new SolidColorBrush(Color.FromRgb(239, 83, 80)),
				
				StrokeUp = Color.FromRgb(38, 166, 154),
				StrokeDown = Color.FromRgb(239, 83, 80),
				StrokeThickness = 1,

				BorderThickness = new Thickness(0, 0, 0, 0)
			};

			var ohlcDataSeries = new OhlcDataSeries<DateTime, double>();
			foreach (var quote in financialInstrument.Quotes)
			{
				ohlcDataSeries.Append
				(
					quote.Date,
					quote.OpenPrice,
					quote.HighPrice,
					quote.LowPrice,
					quote.ClosePrice
				);
			}
			candlestickRenderableSeries.DataSeries = ohlcDataSeries;

			this.FinancialInstruments.Add(
				new SciChartSurface()
				{
					Background = new SolidColorBrush(Color.FromRgb(22, 26, 37)),
					Height = 340,
					ChartTitle = financialInstrument.Name,
					FontSize = 14,
					XAxis = new CategoryDateTimeAxis() { TextFormatting = "dd MMM yyyy", DrawMinorGridLines = false },
					YAxis = new NumericAxis() { TextFormatting = "#.00", DrawMinorGridLines = false, AutoRange = AutoRange.Always },
					RenderableSeries = new ObservableCollection<IRenderableSeries> { candlestickRenderableSeries },

					ChartModifier = new ModifierGroup
									(
										new ZoomPanModifier() { ClipModeX = ClipMode.ClipAtExtents, ZoomExtentsY = false, XyDirection = XyDirection.XDirection, ReceiveHandledEvents = true },
										new MouseWheelZoomModifier() { ExecuteOn = ExecuteOn.MouseDoubleClick, XyDirection = XyDirection.XDirection, ReceiveHandledEvents = true },
										new ZoomExtentsModifier() { ReceiveHandledEvents = true },
										new CursorModifier() { ReceiveHandledEvents = true },
										new RolloverModifier() { ShowAxisLabels = true, IsEnabled = true, DrawVerticalLine = true, SourceMode = SourceMode.AllVisibleSeries, ShowTooltipOn = ShowTooltipOptions.Always, ReceiveHandledEvents = true }
									) { MouseEventGroup = "MainGroup" }
				});

				Binding visibleRangeBinding = new Binding("SharedXRange");
				visibleRangeBinding.Source = this;
				visibleRangeBinding.Mode = BindingMode.TwoWay;

				BindingOperations.SetBinding((DependencyObject)this.FinancialInstruments.Last().XAxis, AxisBase.VisibleRangeProperty, visibleRangeBinding);
		}

		public void RemoveFinancialInstrument(AFinancialInstrument financialInstrument)
		{
			//Костыль
			this.FinancialInstruments.Remove(this.FinancialInstruments.FirstOrDefault(fi => fi.ChartTitle == financialInstrument.Name));
		}
		#endregion
	}
}
