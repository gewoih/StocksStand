using SciChart.Charting.Model.DataSeries;
using StocksStand.Models.Abstractions;
using StocksStand.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using SciChart.Data.Model;
using System.Linq;
using System.Windows.Input;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.TradeChart;
using SciChart.Charting.Common.Helpers;
using StocksStand.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace StocksStand.ViewModels
{
	public class ChartViewModel : BaseViewModel
	{
		public ChartViewModel()
		{
			this.VerticalChartGroupId = Guid.NewGuid().ToString();
			this.ViewportManager = new DefaultViewportManager();

			var closePaneCommand = new ActionCommand<IChildPane>(pane => ChartPaneViewModels.Remove((BaseChartPaneViewModel)pane));

			this.ChangeTimeframeCommand = new RelayCommand(OnChangeTimeframeCommandExecuted, CanChangeTimeframeCommandExecute);
		}

		private IViewportManager _ViewportManager;
		public IViewportManager ViewportManager
		{
			get => _ViewportManager;
			set => Set(ref _ViewportManager, value);
		}

		private string _VerticalChartGroupId;
		public string VerticalChartGroupId
		{
			get => _VerticalChartGroupId;
			set => Set(ref _VerticalChartGroupId, value);
		}

		private IndexRange _XVisibleRange;
		public IndexRange XVisibleRange
		{
			get => _XVisibleRange;
			set => Set(ref _XVisibleRange, value);
		}

		private ObservableCollection<BaseChartPaneViewModel> _ChartPaneViewModels = new ObservableCollection<BaseChartPaneViewModel>();
		public ObservableCollection<BaseChartPaneViewModel> ChartPaneViewModels
		{
			get => _ChartPaneViewModels;
			set => Set(ref _ChartPaneViewModels, value);
		}

		#region Commands
		public ICommand ClosePaneCommand { get; }

		public ICommand ChangeTimeframeCommand { get; }
		private bool CanChangeTimeframeCommandExecute(object p) => true;
		private void OnChangeTimeframeCommandExecuted(object p)
		{
			this.ChangeAllCandlesticksTimeframe(7);
		}
		#endregion

		#region Methods
		public void AddFinancialInstrument(AFinancialInstrument financialInstrument)
		{
			this.ChartPaneViewModels.Add(new PricePaneViewModel(this, financialInstrument) { IsFirstChartPane = true, ViewportManager = this.ViewportManager });
		}

		public void ChangeAllCandlesticksTimeframe(int timeframe)
		{
			//Stopwatch sw = new Stopwatch();
			//sw.Start();

			foreach(var chart in this.ChartPaneViewModels)
			{
				foreach(var dataSeries in chart.ChartSeriesViewModels)
				{
					if (dataSeries.DataSeries is IOhlcDataSeries ohlcDataSeries)
					{
						var dates = ohlcDataSeries.XValues.Cast<DateTime>()
															.Select((x, y) => new Tuple<int, DateTime>(y, x))
															.GroupBy(x => x.Item1 / timeframe)
															.ToList();

						var sectionedSeries = ohlcDataSeries.YValues.Cast<double>()
															.Select((x, y) => new Tuple<int, double>(y, x))
															.GroupBy(x => x.Item1 / timeframe)
															.ToList();

						var newDataSeries = new OhlcDataSeries<DateTime, double>();
						for (int i = 0; i < dates.Count; i++)
						{
							var newDate = dates[i].First();
							var newOpen = sectionedSeries[i].First();
							var newClose = sectionedSeries[i].Last();
							var newHigh = sectionedSeries[i].Max();
							var newLow = sectionedSeries[i].Min();

							newDataSeries.Append(newDate.Item2, newOpen.Item2, newHigh.Item2, newLow.Item2, newClose.Item2);
						}
						dataSeries.DataSeries = newDataSeries;
					}
				}
			}

			//sw.Stop();
			//MessageBox.Show(sw.ElapsedMilliseconds.ToString());
		}
		#endregion
	}
}
