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

		private double _OpenPrice;
		public double OpenPrice
		{
			get => _OpenPrice;
			set => Set(ref _OpenPrice, value);
		}

		private double _HighPrice;
		public double HighPrice
		{
			get => _HighPrice;
			set => Set(ref _HighPrice, value);
		}

		private double _LowPrice;
		public double LowPrice
		{
			get => _LowPrice;
			set => Set(ref _LowPrice, value);
		}

		private double _ClosePrice;
		public double ClosePrice
		{
			get => _ClosePrice;
			set => Set(ref _ClosePrice, value);
		}

		private double _Volume;
		public double Volume
		{
			get => _Volume;
			set => Set(ref _Volume, value);
		}
	}
}
