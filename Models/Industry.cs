﻿using StocksStand.Models.Abstractions;
using StocksStand.Models.Base;
using System.Collections.ObjectModel;

namespace StocksStand.Models
{
	public class Industry : Entity
	{
		private Sector _Sector;
		public Sector Sector
		{
			get => _Sector;
			set => Set(ref _Sector, value);
		}

		private string _Name;
		public string Name
		{
			get => _Name;
			set => Set(ref _Name, value);
		}

		private ObservableCollection<AFinancialInstrument> _FinancialInstruments;
		public ObservableCollection<AFinancialInstrument> FinancialInstruments
		{
			get => _FinancialInstruments;
			set => Set(ref _FinancialInstruments, value);
		}
	}
}
