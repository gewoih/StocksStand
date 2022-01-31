using StocksStand.Models.Base;
using System.Collections.ObjectModel;

namespace StocksStand.Models
{
	public class Country : Entity
	{
		private string _NameRu;
		public string NameRu
		{
			get => _NameRu;
			set => Set(ref _NameRu, value);
		}

		private string _NameEn;
		public string NameEn
		{
			get => _NameEn;
			set => Set(ref _NameEn, value);
		}

		private string _Code;
		public string Code
		{
			get => _Code;
			set => Set(ref _Code, value);
		}

		private Currency _Currency;
		public Currency Currency
		{
			get => _Currency;
			set => Set(ref _Currency, value);
		}

		private ObservableCollection<Sector> _Sectors;
		public ObservableCollection<Sector> Sectors
		{
			get => _Sectors;
			set => Set(ref _Sectors, value);
		}
	}
}
