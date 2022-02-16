using StocksStand.Models.Abstractions;
using StocksStand.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using SciChart.Data.Model;
using System.Windows.Input;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.TradeChart;
using SciChart.Charting.Common.Helpers;
using StocksStand.Commands;
using StocksStand.Models.Enums;

namespace StocksStand.ViewModels
{
	public class ChartViewModel : BaseViewModel
	{
		public ChartViewModel()
		{
			this.VerticalChartGroupId = Guid.NewGuid().ToString();
			this.ViewportManager = new DefaultViewportManager();
			this.ZoomExtentsCommand = new ActionCommand(ZoomExtends);

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
		public ICommand ZoomExtentsCommand { get; private set; }

		public ICommand ChangeTimeframeCommand { get; }
		private bool CanChangeTimeframeCommandExecute(object p) => true;
		private void OnChangeTimeframeCommandExecuted(object p)
		{
			foreach (var chartPane in this.ChartPaneViewModels)
			{
				if (chartPane is PricePaneViewModel pricePane)
				{
					pricePane.UpdateDataSeriesByTimeframe((CandleSize)Convert.ToInt32(p));
				}
			}

			this.ZoomExtentsCommand.Execute(null);
		}
		#endregion

		#region Methods
		private void ZoomExtends()
		{
			this.ViewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
		}

		public void AddFinancialInstrument(AFinancialInstrument financialInstrument)
		{
			this.ChartPaneViewModels.Add(new PricePaneViewModel(this, financialInstrument) { IsFirstChartPane = true, ViewportManager = this.ViewportManager, ClosePaneCommand = new ActionCommand<IChildPane>(pane => ChartPaneViewModels.Remove((BaseChartPaneViewModel)pane)) });
		}
		#endregion
	}
}
