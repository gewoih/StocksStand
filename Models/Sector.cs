using StocksStand.Models.Base;
using System.Collections.ObjectModel;

namespace StocksStand.Models
{
	public class Sector : Entity
	{
		private string _Name;
		public string Name
		{
			get => _Name;
			set => Set(ref _Name, value);
		}

		private Country _Country;
		public Country Country
		{
			get => _Country;
			set => Set(ref _Country, value);
		}

		private ObservableCollection<Industry> _Industries;
		public ObservableCollection<Industry> Industries
		{
			get => _Industries;
			set => Set(ref _Industries, value);
		}
	}
}
