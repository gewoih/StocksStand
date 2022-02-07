using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.TradeChart;
using StocksStand.Models.Abstractions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StocksStand.ViewModels.Base
{
	public abstract class BaseChartPaneViewModel : BaseViewModel, IChildPane
	{
        protected BaseChartPaneViewModel(ChartViewModel parentViewModel, AFinancialInstrument financialInstrument)
        {
            this.ChartSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();
            this.ParentViewModel = parentViewModel;
            this.FinancialInstrument = financialInstrument;
            
            this.ViewportManager = new DefaultViewportManager();
        }

        private ObservableCollection<IRenderableSeriesViewModel> _ChartSeriesViewModels;
        public ObservableCollection<IRenderableSeriesViewModel> ChartSeriesViewModels
        {
            get => _ChartSeriesViewModels;
            set => Set(ref _ChartSeriesViewModels, value);
        }

        private ChartViewModel _ParentViewModel;
        public ChartViewModel ParentViewModel
        {
            get => _ParentViewModel;
            set => Set(ref _ParentViewModel, value);
        }

        private AFinancialInstrument _FinancialInstrument;
        public AFinancialInstrument FinancialInstrument
		{
            get => _FinancialInstrument;
            set => Set(ref _FinancialInstrument, value);
		}

        private IViewportManager _ViewportManager;
        public IViewportManager ViewportManager
		{
            get => _ViewportManager;
            set => Set(ref _ViewportManager, value);
		}

        private string _YAxisTextFormatting;
        public string YAxisTextFormatting
        {
            get => _YAxisTextFormatting;
            set => Set(ref _YAxisTextFormatting, value);
        }

        private string _Title;
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private bool _IsFirstChartPane;
        public bool IsFirstChartPane
        {
            get => _IsFirstChartPane;
            set => Set(ref _IsFirstChartPane, value);
        }

        private bool _IsLastChartPane;
        public bool IsLastChartPane
        {
            get => _IsLastChartPane;
            set => Set(ref _IsLastChartPane, value);
        }

        private double _Height = double.NaN;
        public double Height
        {
            get => _Height;
            set => Set(ref _Height, value);
        }

		#region Methods
		public abstract void UpdateDataSeriesByTimeframe(int timeframe);

        public void ZoomExtents()
        {
        }
		#endregion

		#region Commands
		public ICommand ClosePaneCommand
        {
            get; set;
        }
		#endregion
	}
}