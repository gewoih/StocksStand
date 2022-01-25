using StocksStand.Commands;
using StocksStand.DataContext;
using StocksStand.Models;
using StocksStand.Models.Abstractions;
using StocksStand.Repositories;
using StocksStand.ViewModels.Base;
using StocksStand.Views;
using StocksStand.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

			this.ShowNewStockWindowCommand = new RelayCommand(OnShowNewStockWindowCommandExecuted, CanShowNewStockWindowCommandExecute);
			this.AddStockCommand = new RelayCommand(OnAddStockCommandExecuted, CanAddStockCommandExecute);

			this.LoadQuotesForInstrumentCommand = new RelayCommand(OnLoadQuotesForInstrumentCommandExecuted, CanLoadQuotesForInstrumentCommandExecute);

			this.ShowInstrumentQuotesCommand = new RelayCommand(OnShowInstrumentQuotesCommandExecuted, CanShowInstrumentQuotesCommandExecute);
			this.HideInstrumentQuotesCommand = new RelayCommand(OnHideInstrumentQuotesCommandExecuted, CanHideInstrumentQuotesCommandExecute);

			this.Sectors = new ObservableCollection<Sector>(new SectorsRepository(new BaseDataContext()).GetAll());
			this.Countries = new ObservableCollection<Country>(new CountriesRepository(new BaseDataContext()).GetAll());
			this.ChartViewModel = new ChartViewModel();
		}
		#endregion

		#region Properties
		//Коллекция окон для работы с Финансовыми Инструментами (графики, фин. показатели, индикаторы и т.д.)
		private ChartViewModel _ChartViewModel;
		public ChartViewModel ChartViewModel
		{
			get => _ChartViewModel;
			set => Set(ref _ChartViewModel, value);
		}

		//Список всех Секторов
		private ObservableCollection<Sector> _Sectors;
		public ObservableCollection<Sector> Sectors
		{
			get => _Sectors;
			set => Set(ref _Sectors, value);
		}

		private ObservableCollection<Country> _Countries;
		public ObservableCollection<Country> Countries
		{
			get => _Countries;
			set => Set(ref _Countries, value);
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



		//Объект новой Отрасли
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



		//Объект нового Акции
		private Stock _NewStock;
		public Stock NewStock
		{
			get => _NewStock;
			set => Set(ref _NewStock, value);
		}

		//Форма создания/редактирования Акции
		private NewStockView _NewStockView;
		public NewStockView NewStockView
		{
			get => _NewStockView;
			set => Set(ref _NewStockView, value);
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
			this.NewSector = new Sector { Industries = new ObservableCollection<Industry>() };

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
			this.NewIndustry = new Industry { Stocks = new ObservableCollection<Stock>(), Sector = this.SelectedSector };

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



		//Отображение окна для добавления новой Акции
		public ICommand ShowNewStockWindowCommand { get; }
		private bool CanShowNewStockWindowCommandExecute(object p) => this.SelectedIndustry != null;
		private void OnShowNewStockWindowCommandExecuted(object p)
		{
			//Инициализируем новую Акцию
			this.NewStock = new Stock { Industry = this.SelectedIndustry, Quotes = new ObservableCollection<Quote>() };

			//Инициализируем и вызываем форму для создания новой Акции
			this.NewStockView = new NewStockView(this);
			this.NewStockView.ShowDialog();
		}

		//Добавление новой Акции
		public ICommand AddStockCommand { get; }
		private bool CanAddStockCommandExecute(object p) => true;
		private void OnAddStockCommandExecuted(object p)
		{
			//Пустое ли имя Акции?
			if (!String.IsNullOrEmpty(this.NewStock.Name))
			{
				if (!String.IsNullOrEmpty(this.NewStock.Ticker))
				{
					if (this.NewStock.Country != null)
					{
						StocksRepository StocksRepository = new StocksRepository(new BaseDataContext());
						//Проверяем есть ли в базе Акция с таким тикером (без учета регистра)
						if (StocksRepository.GetAll().FirstOrDefault(c => c.Ticker.ToLower() == this.NewStock.Ticker.ToLower()) == null)
						{
							//Если такая Акция не найдена - создаем новую и добавляем ее к выбранной Отрасли
							this.SelectedIndustry.Stocks.Add(StocksRepository.Create(this.NewStock));

							//Закрываем форму создания Акции
							this.NewStockView.Close();
							MessageBox.Show($"Бумага '{this.NewStock.Name}'[{this.NewStock.Ticker}] успешно добавлена.");
						}
						else
							MessageBox.Show("Бумага с таким тикером уже существует.");
					}
					else
						MessageBox.Show("Выберите страну!");
				}
				else
					MessageBox.Show("Введите тикер бумаги");
			}
			else
				MessageBox.Show("Введите наименование бумаги!");
		}



		//Загрузка котировок по выбранному Финансовому Инструменту
		public ICommand LoadQuotesForInstrumentCommand { get; }
		private bool CanLoadQuotesForInstrumentCommandExecute(object p) => this.SelectedFinancialInstrument != null;
		private void OnLoadQuotesForInstrumentCommandExecuted(object p)
		{
			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();
			int loadedQuotes = this.SelectedFinancialInstrument.LoadQuotes();
			stopwatch.Stop();

			MessageBox.Show($"Было загружено {loadedQuotes} котировок за {stopwatch.ElapsedMilliseconds / 1000} секунд.");
		}



		//Добавление Финансового Инструмента в рабочую область
		public ICommand ShowInstrumentQuotesCommand { get; }
		private bool CanShowInstrumentQuotesCommandExecute(object p) => this.SelectedFinancialInstrument != null;
		private void OnShowInstrumentQuotesCommandExecuted(object p)
		{
			this.ChartViewModel.AddFinancialInstrument(this.SelectedFinancialInstrument);
		}

		public ICommand HideInstrumentQuotesCommand { get; }
		private bool CanHideInstrumentQuotesCommandExecute(object p) => this.SelectedFinancialInstrument != null;
		private void OnHideInstrumentQuotesCommandExecuted(object p)
		{
			this.ChartViewModel.RemoveFinancialInstrument(this.SelectedFinancialInstrument);
		}
		#endregion
	}
}
