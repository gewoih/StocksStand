using StocksStand.Commands;
using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Models.Abstractions;
using StocksStand.Repositories;
using StocksStand.ViewModels.Base;
using StocksStand.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StocksStand.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		#region Constructor
		public MainViewModel()
		{
			this.ChangeSelectedItemCommand = new RelayCommand(OnChangeSelectedItemCommandExecuted, CanChangeSelectedItemCommandExecute);
			this.ShowNewSectorWindowCommand = new RelayCommand(OnShowNewSectorWindowCommandExecuted, CanShowNewSectorWindowCommandExecute);
			this.AddSectorCommand = new RelayCommand(OnAddSectorCommandExecuted, CanAddSectorCommandExecute);

			this.Sectors = new ObservableCollection<Sector>(new SectorsRepository(new BaseDataContext()).GetAll());
		}
		#endregion

		#region Properties
		//Список всех секторов
		private ObservableCollection<Sector> _Sectors;
		public ObservableCollection<Sector> Sectors
		{
			get => _Sectors;
			set => Set(ref _Sectors, value);
		}

		private Sector _SelectedSector;
		public Sector SelectedSector
		{
			get => _SelectedSector;
			set => Set(ref _SelectedSector, value);
		}

		private Industry _SelectedIndustry;
		public Industry SelectedIndustry
		{
			get => _SelectedIndustry;
			set => Set(ref _SelectedIndustry, value);
		}

		private AFinancialInstrument _SelectedFinancialInstrument;
		public AFinancialInstrument SelectedFinancialInstrument
		{
			get => _SelectedFinancialInstrument;
			set => Set(ref _SelectedFinancialInstrument, value);
		}

		//Объект нового сектора
		private Sector _NewSector;
		public Sector NewSector
		{
			get => _NewSector;
			set => Set(ref _NewSector, value);
		}

		//Форма создания/редактирования сектора
		private NewSectorView _NewSectorView;
		public NewSectorView NewSectorView
		{
			get => _NewSectorView;
			set => Set(ref _NewSectorView, value);
		}
		#endregion

		#region Commands
		public ICommand ChangeSelectedItemCommand { get; }
		private bool CanChangeSelectedItemCommandExecute(object p) => true;
		private void OnChangeSelectedItemCommandExecuted(object p)
		{
			//Проверка выбранного элемента TreeView
			if (p is Sector)
				this.SelectedSector = (Sector)p;
			else if (p is Industry)
				this.SelectedIndustry = (Industry)p;
			else if (p is AFinancialInstrument)
				this.SelectedFinancialInstrument = (AFinancialInstrument)p;
		}

		//Отображение окна для добавления нового Sector
		public ICommand ShowNewSectorWindowCommand { get; }
		private bool CanShowNewSectorWindowCommandExecute(object p) => true;
		private void OnShowNewSectorWindowCommandExecuted(object p)
		{
			//Инициализируем новый сектор
			this.NewSector = new Sector();

			//Инициализируем и вызываем форму для создания нового сектора
			this.NewSectorView = new NewSectorView(this);
			this.NewSectorView.ShowDialog();
		}

		//Добавление нового Sector
		public ICommand AddSectorCommand { get; }
		private bool CanAddSectorCommandExecute(object p) => true;
		private void OnAddSectorCommandExecuted(object p)
		{
			//Пустое ли имя сектора?
			if (this.NewSector.Name != String.Empty)
			{
				SectorsRepository SectorsRepository = new SectorsRepository(new BaseDataContext());
				//Проверяем есть ли в базе сектор с таким названием (без учета регистра)
				if (SectorsRepository.GetAll().FirstOrDefault(c => c.Name.ToLower() == this.NewSector.Name.ToLower()) == null)
				{
					//Если такой сектор не найден - создаем новый
					this.Sectors.Add(SectorsRepository.Create(this.NewSector));

					//Закрываем форму создания сектора
					this.NewSectorView.Close();
					MessageBox.Show($"Сектор '{this.NewSector.Name}' успешно создан.");
				}
				else
					MessageBox.Show("Сектор с таким названием уже существует.");
			}
			else
				MessageBox.Show("Заполните имя сектора!");
		}
		#endregion
	}
}
