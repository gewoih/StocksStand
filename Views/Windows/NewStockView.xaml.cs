using StocksStand.ViewModels;
using System.Windows;

namespace StocksStand.Views.Windows
{
	/// <summary>
	/// Логика взаимодействия для NewStockView.xaml
	/// </summary>
	public partial class NewStockView : Window
	{
		public NewStockView(MainViewModel mainViewModel)
		{
			InitializeComponent();

			this.DataContext = mainViewModel;
		}
	}
}
