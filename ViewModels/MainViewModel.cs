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

			this.ShowNewIndustryWindowCommand = new RelayCommand(OnShowNewIndustryWindowCommandExecuted, CanShowNewIndustryWindowCommandExecute);
			this.AddIndustryCommand = new RelayCommand(OnAddIndustryCommandExecuted, CanAddIndustryCommandExecute);

			this.Sectors = new ObservableCollection<Sector>(new SectorsRepository(new BaseDataContext()).GetAll());
		}
		#endregion

		#region Properties
		//Список всех Секторов
		private ObservableCollection<Sector> _Sectors;
		public ObservableCollection<Sector> Sectors
		{
			get => _Sectors;
			set => Set(ref _Sectors, value);
		}

		//Выбранный Сектор
		private Sector _SelectedSector;
		public Sector SelectedSector
		{
			get => _SelectedSector;
			set => Set(ref _SelectedSector, value);
		}

		//Выбранная Отрасль
		private Industry _SelectedIndustry;
		public Industry SelectedIndustry
		{
			get => _SelectedIndustry;
			set => Set(ref _SelectedIndustry, value);
		}

		//Выбранный Финансовый Инструмент
		private AFinancialInstrument _SelectedFinancialInstrument;
		public AFinancialInstrument SelectedFinancialInstrument
		{
			get => _SelectedFinancialInstrument;
			set => Set(ref _SelectedFinancialInstrument, value);
		}

		//Объект нового Сектора
		private Sector _NewSector;
		public Sector NewSector
		{
			get => _NewSector;
			set => Set(ref _NewSector, value);
		}

		//Форма создания/редактирования Сектора
		private NewSectorView _NewSectorView;
		public NewSectorView NewSectorView
		{
			get => _NewSectorView;
			set => Set(ref _NewSectorView, value);
		}



		//Объект нового Отрасли
		private Industry _NewIndustry;
		public Industry NewIndustry
		{
			get => _NewIndustry;
			set => Set(ref _NewIndustry, value);
		}

		//Форма создания/редактирования Отрасли
		private NewIndustryView _NewIndustryView;
		public NewIndustryView NewIndustryView
		{
			get => _NewIndustryView;
			set => Set(ref _NewIndustryView, value);
		}
		#endregion

		#region Commands
		//Изменение выбранного элемента в TreeView
		public ICommand ChangeSelectedItemCommand { get; }
		private bool CanChangeSelectedItemCommandExecute(object p) => true;
		private void OnChangeSelectedItemCommandExecuted(object p)
		{
			//Проверка выбранного элемента TreeView
			if (p is Sector sector)
			{
				this.SelectedSector = sector;
				this.SelectedIndustry = null;
				this.SelectedFinancialInstrument = null;
			}
			else if (p is Industry industry)
			{
				this.SelectedIndustry = industry;
				this.SelectedSector = this.SelectedIndustry.Sector;
				this.SelectedFinancialInstrument = null;
			}
			else if (p is Stock stock)
			{
				this.SelectedFinancialInstrument = stock;
				this.SelectedIndustry = stock.Industry;
				this.SelectedSector = stock.Industry.Sector;
			}
		}

		//Отображение окна для добавления нового Сектора
		public ICommand ShowNewSectorWindowCommand { get; }
		private bool CanShowNewSectorWindowCommandExecute(object p) => true;
		private void OnShowNewSectorWindowCommandExecuted(object p)
		{
			//Инициализируем новый Сектор
			this.NewSector = new Sector();

			//Инициализируем и вызываем форму для создания нового Сектора
			this.NewSectorView = new NewSectorView(this);
			this.NewSectorView.ShowDialog();
		}

		//Добавление нового Сектора
		public ICommand AddSectorCommand { get; }
		private bool CanAddSectorCommandExecute(object p) => true;
		private void OnAddSectorCommandExecuted(object p)
		{
			//Пустое ли имя сектора?
			if (!String.IsNullOrEmpty(this.NewSector.Name))
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



		//Отображение окна для добавления новой Отрасли
		public ICommand ShowNewIndustryWindowCommand { get; }
		private bool CanShowNewIndustryWindowCommandExecute(object p) => this.SelectedSector != null;
		private void OnShowNewIndustryWindowCommandExecuted(object p)
		{
			//Инициализируем новую Отрасль
			this.NewIndustry = new Industry();

			//Инициализируем и вызываем форму для создания новой Отрасли
			this.NewIndustryView = new NewIndustryView(this);
			this.NewIndustryView.ShowDialog();
		}

		//Добавление новой Отрасли
		public ICommand AddIndustryCommand { get; }
		private bool CanAddIndustryCommandExecute(object p) => true;
		private void OnAddIndustryCommandExecuted(object p)
		{
			//Пустое ли имя Отрасли?
			if (!String.IsNullOrEmpty(this.NewIndustry.Name))
			{
				IndustriesRepository IndustriesRepository = new IndustriesRepository(new BaseDataContext());
				//Проверяем есть ли в базе Отрасль с таким названием (без учета регистра)
				if (IndustriesRepository.GetAll().FirstOrDefault(c => c.Name.ToLower() == this.NewIndustry.Name.ToLower()) == null)
				{
					//Если такая Отрасль не найдена - создаем новую и добавляем ее к выбранному Сектору
					this.SelectedSector.Industries.Add(IndustriesRepository.Create(this.NewIndustry));

					//Закрываем форму создания Отрасли
					this.NewIndustryView.Close();
					MessageBox.Show($"Отрасль '{this.NewIndustry.Name}' успешно создана.");
				}
				else
					MessageBox.Show("Отрасль с таким названием уже существует.");
			}
			else
				MessageBox.Show("Заполните название отрасли!");
		}
		#endregion
	}
}
