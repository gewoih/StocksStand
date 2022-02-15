using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using StocksStand.Models;
using StocksStand.Models.Abstractions;
using StocksStand.ViewModels.Base;
using System;
using System.Linq;

namespace StocksStand.ViewModels
{
	public class FinancialDataPaneViewModel : BaseChartPaneViewModel
	{
		public FinancialDataPaneViewModel(ChartViewModel parentViewModel, AFinancialInstrument financialInstrument, FinancialDataType dataType) : base(parentViewModel, financialInstrument)
		{
			this.FinancialDataType = dataType;

			this.LoadDataSeries();
		}

		#region Properties
		private FinancialDataType _FinancialDataType;
		public FinancialDataType FinancialDataType
		{
			get => _FinancialDataType;
			set => Set(ref _FinancialDataType, value);
		}
		#endregion

		protected override void LoadDataSeries()
		{
			var stockFinancialData = new XyDataSeries<DateTime, double>() { SeriesName = $"[{this.FinancialInstrument.Name}] - Финансовые показатели" };

			stockFinancialData.Append
				(
					((Stock)this.FinancialInstrument).FinancialData.FirstOrDefault(fd => fd.enName == this.FinancialDataType.enName).Values.Select(v => v.date),
					((Stock)this.FinancialInstrument).FinancialData.FirstOrDefault(fd => fd.enName == this.FinancialDataType.enName).Values.Select(v => v.value)
				);

			this.ChartSeriesViewModels.Add(new ColumnRenderableSeriesViewModel
			{
				DataSeries = stockFinancialData,

				AntiAliasing = false,
				StrokeThickness = 1
			});

			this.YAxisTextFormatting = $"#0.00";
		}
	}
}
