using StocksStand.ViewModels;
using System.Windows;

namespace StocksStand.Views.Windows
{
	/// <summary>
	/// Логика взаимодействия для CreateSectorView.xaml
	/// </summary>
	public partial class NewSectorView : Window
	{
		public NewSectorView(MainViewModel MainViewModel)
		{
			InitializeComponent();

			this.DataContext = MainViewModel;
		}
	}
}
