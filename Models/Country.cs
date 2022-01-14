using StocksStand.Models.Base;

namespace StocksStand.Models
{
	public class Country : Entity
	{
		private string _Name;
		public string Name
		{
			get => _Name;
			set => Set(ref _Name, value);
		}

		private Currency _Currency;
		public Currency Currency
		{
			get => _Currency;
			set => Set(ref _Currency, value);
		}
	}
}
