using StocksStand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksStand.Models
{
	public class FinancialDataValue : Entity
	{
		private Stock _Stock;
		public Stock Stock
		{
			get => _Stock;
			set => Set(ref _Stock, value);
		}

		private FinancialDataType _FinancialDataType;
		public FinancialDataType FinancialDataType
		{
			get => _FinancialDataType;
			set => Set(ref _FinancialDataType, value);
		}

		private DateTime _date;
		public DateTime date
		{
			get => _date;
			set => Set(ref _date, value);
		}

		private double _value;
		public double value
		{
			get => _value;
			set => Set(ref _value, value);
		}
	}
}
