using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Repositories;
using StocksStand.ViewModels.Base;
using System.Collections.ObjectModel;

namespace StocksStand.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		#region Constructor
		public MainViewModel()
		{
			this.Sectors = new ObservableCollection<Sector>(new SectorsRepository(new BaseDataContext()).GetAll());
		}
		#endregion

		#region Properties
		private ObservableCollection<Sector> _Sectors;
		public ObservableCollection<Sector> Sectors
		{
			get => _Sectors;
			set => Set(ref _Sectors, value);
		}

		private ObservableCollection<Industry> _Industries;
		public ObservableCollection<Industry> Industries
		{
			get => _Industries;
			set => Set(ref _Industries, value);
		}
		#endregion

		#region Commands

		#endregion
	}
}
