using StocksStand.Models.Abstractions;
using StocksStand.Models.Base;
using System;

namespace StocksStand.Models
{
	public class Quote : Entity
	{
		private AFinancialInstrument _FinancialInstrument;
		public AFinancialInstrument FinancialInstrument
		{
			get => _FinancialInstrument;
			set => Set(ref _FinancialInstrument, value);
		}

		private DateTime _Date;
		public DateTime Date
		{
			get => _Date;
			set => Set(ref _Date, value);
		}

		private decimal _OpenPrice;
		public decimal OpenPrice
		{
			get => _OpenPrice;
			set => Set(ref _OpenPrice, value);
		}

		private decimal _HighPrice;
		public decimal HighPrice
		{
			get => _HighPrice;
			set => Set(ref _HighPrice, value);
		}

		private decimal _LowPrice;
		public decimal LowPrice
		{
			get => _LowPrice;
			set => Set(ref _LowPrice, value);
		}

		private decimal _ClosePrice;
		public decimal ClosePrice
		{
			get => _ClosePrice;
			set => Set(ref _ClosePrice, value);
		}

		private decimal _Volume;
		public decimal Volume
		{
			get => _Volume;
			set => Set(ref _Volume, value);
		}
	}
}
