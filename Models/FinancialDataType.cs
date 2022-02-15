using StocksStand.Models.Abstractions;
using StocksStand.Models.Base;
using System;
using System.Collections.Generic;

namespace StocksStand.Models
{
	public class FinancialDataType : Entity
	{
		private string _enName;
		public string enName
		{
			get => _enName;
			set => Set(ref _enName, value);
		}

		private string _ruName;
		public string ruName
		{
			get => _ruName;
			set => Set(ref _ruName, value);
		}

		private List<FinancialDataValue> _Values = new List<FinancialDataValue>();
		public List<FinancialDataValue> Values
		{
			get => _Values;
			set => Set(ref _Values, value);
		}
	}
}
