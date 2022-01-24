using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;
using StocksStand.Commands;
using StocksStand.Models.Abstractions;
using StocksStand.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace StocksStand.ViewModels
{
	public class ChartViewModel : BaseViewModel
	{
		#region Constructor
		public ChartViewModel(AFinancialInstrument financialInstrument)
		{
			this.FinancialInstrument = financialInstrument;
			this.RenderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();

			this.LoadChartCommand = new RelayCommand(OnLoadChartCommandExecuted, CanLoadChartCommandExecute);
			this.LoadChartCommand.Execute(null);
		}
		#endregion

		#region Properties
		private ObservableCollection<IRenderableSeriesViewModel> _RenderableSeries;
		public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries
		{
			get => _RenderableSeries;
			set => Set(ref _RenderableSeries, value);
		}

		//Выбранный Финансовый Инструмент
		private AFinancialInstrument _FinancialInstrument;
		public AFinancialInstrument FinancialInstrument
		{
			get => _FinancialInstrument;
			set => Set(ref _FinancialInstrument, value);
		}
		#endregion

		#region Commands
		public ICommand LoadChartCommand { get; }
		private bool CanLoadChartCommandExecute(object p) => true;
		private void OnLoadChartCommandExecuted(object p)
		{
			IRenderableSeriesViewModel candlestickRenderableSeries = new CandlestickRenderableSeriesViewModel();
			this.RenderableSeries.Add(candlestickRenderableSeries);

			IDataSeries<DateTime, double> ohlcDataSeries = new OhlcDataSeries<DateTime, double>();
			foreach (var quote in this.FinancialInstrument.Quotes)
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
		}
		#endregion
	}
}
