using StocksStand.Models.Abstractions;

namespace StocksStand.Models
{
	public class Stock : AFinancialInstrument
	{
		private Industry _Industry;
		public Industry Industry
		{
			get => _Industry;
			set => Set(ref _Industry, value);
		}
	}
}
