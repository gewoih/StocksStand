using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.TradeChart;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StocksStand.ViewModels.Base
{
	public abstract class BaseChartPaneViewModel : BaseViewModel, IChildPane
	{
        private readonly ObservableCollection<IRenderableSeriesViewModel> _chartSeriesViewModels;

        protected BaseChartPaneViewModel(ChartViewModel parentViewModel)
        {
            _chartSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();
            _ParentViewModel = parentViewModel;
            
            ViewportManager = new DefaultViewportManager();
        }

        private readonly ChartViewModel _ParentViewModel;
        public ChartViewModel ParentViewModel
        {
            get => _ParentViewModel;
        }

        public ObservableCollection<IRenderableSeriesViewModel> ChartSeriesViewModels
        {
            get => _chartSeriesViewModels;
        }

        public IViewportManager ViewportManager { get; set; }

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

        public void ZoomExtents()
        {
        }

        public ICommand ClosePaneCommand
        {
            get; set;
        }
    }
}