using StocksStand.ViewModels;
using System.Windows;

namespace StocksStand.Views.Windows
{
	/// <summary>
	/// Логика взаимодействия для NewIndustryView.xaml
	/// </summary>
	public partial class NewIndustryView : Window
	{
		public NewIndustryView(MainViewModel mainViewModel)
		{
			InitializeComponent();

			this.DataContext = mainViewModel;
		}
	}
}
