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
			foreach(var chartPane in this.ChartPaneViewModels)
			{
				chartPane.UpdateDataSeriesByTimeframe(Convert.ToInt32(p));
			}
		}
		#endregion

		#region Methods
		public void AddFinancialInstrument(AFinancialInstrument financialInstrument)
		{
			this.ChartPaneViewModels.Add(new PricePaneViewModel(this, financialInstrument) { IsFirstChartPane = true, ViewportManager = this.ViewportManager });
		}
		#endregion
	}
}
