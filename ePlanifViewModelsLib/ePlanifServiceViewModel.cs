using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class ePlanifServiceViewModel : ViewModel<string>
    {

		#region commands
		public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register("RefreshCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand RefreshCommand
		{
			get { return (ViewModelCommand)GetValue(RefreshCommandProperty); }
			private set { SetValue(RefreshCommandProperty, value); }
		}
		public static readonly DependencyProperty UndoCommandProperty = DependencyProperty.Register("UndoCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand UndoCommand
		{
			get { return (ViewModelCommand)GetValue(UndoCommandProperty); }
			private set { SetValue(UndoCommandProperty, value); }
		}
		public static readonly DependencyProperty RedoCommandProperty = DependencyProperty.Register("RedoCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand RedoCommand
		{
			get { return (ViewModelCommand)GetValue(RedoCommandProperty); }
			private set { SetValue(RedoCommandProperty, value); }
		}
		public static readonly DependencyProperty CurrentWeekCommandProperty = DependencyProperty.Register("CurrentWeekCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand CurrentWeekCommand
		{
			get { return (ViewModelCommand)GetValue(CurrentWeekCommandProperty); }
			private set { SetValue(CurrentWeekCommandProperty, value); }
		}
		public static readonly DependencyProperty PreviousWeekCommandProperty = DependencyProperty.Register("PreviousWeekCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand PreviousWeekCommand
		{
			get { return (ViewModelCommand)GetValue(PreviousWeekCommandProperty); }
			private set { SetValue(PreviousWeekCommandProperty, value); }
		}
		public static readonly DependencyProperty NextWeekCommandProperty = DependencyProperty.Register("NextWeekCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand NextWeekCommand
		{
			get { return (ViewModelCommand)GetValue(NextWeekCommandProperty); }
			private set { SetValue(NextWeekCommandProperty, value); }
		}

		public static readonly DependencyProperty RemoveDaysCommandProperty = DependencyProperty.Register("RemoveDaysCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand RemoveDaysCommand
		{
			get { return (ViewModelCommand)GetValue(RemoveDaysCommandProperty); }
			private set { SetValue(RemoveDaysCommandProperty, value); }
		}

		public static readonly DependencyProperty AddDaysCommandProperty = DependencyProperty.Register("AddDaysCommand", typeof(ViewModelCommand), typeof(ePlanifServiceViewModel));
		public ViewModelCommand AddDaysCommand
		{
			get { return (ViewModelCommand)GetValue(AddDaysCommandProperty); }
			private set { SetValue(AddDaysCommandProperty, value); }
		}
		#endregion

		//private IePlanifServiceClient client;

		private string server;
		public string Server
		{
			get { return server; }
		}

		private CommandManager commandManager;
		public CommandManager CommandManager
		{
			get { return commandManager; }
		}





		public static readonly DependencyProperty UserAccountProperty = DependencyProperty.Register("UserAccount", typeof(Account), typeof(ePlanifServiceViewModel));
		public Account UserAccount
		{
			get { return (Account)GetValue(UserAccountProperty); }
			private set { SetValue(UserAccountProperty, value); }
		}


		public static readonly DependencyProperty UserProfileProperty = DependencyProperty.Register("UserProfile", typeof(Profile), typeof(ePlanifServiceViewModel));
		public Profile UserProfile
		{
			get { return (Profile)GetValue(UserProfileProperty); }
			private set { SetValue(UserProfileProperty, value); }
		}


		


		public static readonly DependencyProperty WeekNameProperty = DependencyProperty.Register("WeekName", typeof(string), typeof(ePlanifServiceViewModel));
		public string WeekName
		{
			get { return (string)GetValue(WeekNameProperty); }
			private set { SetValue(WeekNameProperty, value); }
		}

		private DateTime startDate; // cannot be dependency property, in order to call setter in binding
		public DateTime StartDate
		{
			get { return startDate; }
			set	{ startDate= FirstDayOfWeek(value);UpdateWeekNumber();OnPropertyChanged(); }
		}


		public static readonly DependencyProperty DaysCountProperty = DependencyProperty.Register("DaysCount", typeof(int), typeof(ePlanifServiceViewModel),new PropertyMetadata(7));
		public int DaysCount
		{
			get { return (int)GetValue(DaysCountProperty); }
			set { SetValue(DaysCountProperty, value); }
		}



		private ReportViewModelCollection reports;
		public ReportViewModelCollection Reports
		{
			get { return reports; }
		}

		private OptionViewModelCollection options;
		public OptionViewModelCollection Options
		{
			get { return options; }
		}
		public OptionViewModel Option
		{
			get { return options?.FirstOrDefault(); }
		}

		private CountryCodeViewModelCollection countryCodes;
		public CountryCodeViewModelCollection CountryCodes
		{
			get { return countryCodes; }
		}

		private DayViewModelCollection days;
		public DayViewModelCollection Days
		{
			get { return days; }
		}

		private ActivityViewModelCollection activities;
		public ActivityViewModelCollection Activities
		{
			get { return activities; }
		}
		

		private ActivityTypeViewModelCollection activityTypes;
		public ActivityTypeViewModelCollection ActivityTypes
		{
			get { return activityTypes; }
		}

		private FilteredViewModelCollection<ActivityTypeViewModel, ActivityType> visibleActivityTypes;
		public FilteredViewModelCollection<ActivityTypeViewModel,ActivityType> VisibleActivityTypes
		{
			get { return visibleActivityTypes; }
		}




		private EmployeeViewModelCollection employees;
		public EmployeeViewModelCollection Employees
		{
			get { return employees; }
		}

		

		private FilteredViewModelCollection<EmployeeViewModel, Employee> visibleEmployees;
		public FilteredViewModelCollection<EmployeeViewModel,Employee> VisibleEmployees
		{
			get { return visibleEmployees; }
		}

		private FilteredViewModelCollection<EmployeeViewModel, Employee> writeableEmployees;
		public FilteredViewModelCollection<EmployeeViewModel, Employee> WriteableEmployees
		{
			get { return writeableEmployees; }
		}

		private AccountViewModelCollection accounts;
		public AccountViewModelCollection Accounts
		{
			get { return accounts; }
		}

		private ProfileViewModelCollection profiles;
		public ProfileViewModelCollection Profiles
		{
			get { return profiles; }
		}

		
		private GroupViewModelCollection groups;
		public GroupViewModelCollection Groups
		{
			get { return groups; }
		}

		
		private LayerViewModelCollection layers;
		public LayerViewModelCollection Layers
		{
			get { return layers; }
		}

		private FilteredViewModelCollection<LayerViewModel, Layer> visibleLayers;
		public FilteredViewModelCollection<LayerViewModel,Layer> VisibleLayers
		{
			get { return visibleLayers; }
		}

		private EmployeeViewViewModelCollection employeeViews;
		public EmployeeViewViewModelCollection EmployeeViews
		{
			get { return employeeViews; }
		}
		private ActivityTypeViewViewModelCollection activityTypeViews;
		public ActivityTypeViewViewModelCollection ActivityTypeViews
		{
			get { return activityTypeViews; }
		}

		private PhotoViewModelCollection photos;
		public PhotoViewModelCollection Photos
		{
			get { return photos; }
		}


		private TabViewModelCollection tabs;
		public TabViewModelCollection Tabs
		{
			get { return tabs; }
		}

		private DayOfWeekViewModelCollection daysOfWeek;
		public DayOfWeekViewModelCollection DaysOfWeek
		{
			get { return daysOfWeek; }
		}

		private CalendarWeekRuleViewModelCollection calendarWeekRules;
		public CalendarWeekRuleViewModelCollection CalendarWeekRules
		{
			get { return calendarWeekRules; }
		}

		public ePlanifServiceViewModel()
        {
			commandManager = new CommandManager(10);


	
			RefreshCommand = new ViewModelCommand(OnRefreshCommandCanExecute, OnRefreshCommandExecute);
			UndoCommand = new ViewModelCommand(OnUndoCommandCanExecute, OnUndoCommandExecute);
			RedoCommand = new ViewModelCommand(OnRedoCommandCanExecute, OnRedoCommandExecute);
			CurrentWeekCommand = new ViewModelCommand(OnCurrentWeekCommandCanExecute, OnCurrentWeekCommandExecute);
			PreviousWeekCommand = new ViewModelCommand(OnPreviousWeekCommandCanExecute, OnPreviousWeekCommandExecute);
			NextWeekCommand = new ViewModelCommand(OnNextWeekCommandCanExecute, OnNextWeekCommandExecute);

			RemoveDaysCommand = new ViewModelCommand(OnRemoveDaysCommandCanExecute, OnRemoveDaysCommandExecute);
			AddDaysCommand = new ViewModelCommand(OnAddDaysCommandCanExecute, OnAddDaysCommandExecute);

			StartDate = DateTime.Now;

			reports = new ReportViewModelCollection(this);Children.Add(reports);
			options = new OptionViewModelCollection(this); // do not add options into children. Option are loaded at connection. Children.Add(options);


			countryCodes = new CountryCodeViewModelCollection(this); Children.Add(countryCodes);
			daysOfWeek = new DayOfWeekViewModelCollection(this); Children.Add(daysOfWeek);
			calendarWeekRules = new CalendarWeekRuleViewModelCollection(this);Children.Add(calendarWeekRules);
			days = new DayViewModelCollection(this);Children.Add(days);
			layers = new LayerViewModelCollection(this); Children.Add(layers);
			visibleLayers = new FilteredViewModelCollection<LayerViewModel, Layer>(layers, (item) => { return item.IsDisabled!=true; }); Children.Add(visibleLayers);

			activityTypes = new ActivityTypeViewModelCollection(this);Children.Add(activityTypes);
			visibleActivityTypes = new FilteredViewModelCollection<ActivityTypeViewModel, ActivityType>(activityTypes, (item) => { return (item.IsDisabled!=true) ; }); Children.Add(visibleActivityTypes);
			profiles = new ProfileViewModelCollection(this); Children.Add(profiles); // profiles must be loaded before users in order to build query
			employees = new EmployeeViewModelCollection(this);Children.Add(employees);
			visibleEmployees = new FilteredViewModelCollection<EmployeeViewModel, Employee>(employees, (item) => { return item.IsDisabled !=true; }); Children.Add(visibleEmployees);
			writeableEmployees = new FilteredViewModelCollection<EmployeeViewModel, Employee>(employees, (item) => { return (item.IsDisabled != true) && (item.WriteAccess==true); }); Children.Add(writeableEmployees);
			accounts = new AccountViewModelCollection(this);Children.Add(accounts);
			groups = new GroupViewModelCollection(this);Children.Add(groups);

			activities = new ActivityViewModelCollection(this); Children.Add(activities);
			employeeViews = new EmployeeViewViewModelCollection(this);Children.Add(employeeViews);
			activityTypeViews = new ActivityTypeViewViewModelCollection(this);Children.Add(activityTypeViews);

			photos = new PhotoViewModelCollection(this);Children.Add(photos);

			tabs = new TabViewModelCollection(this);Children.Add(tabs);
			
		}

		public IePlanifServiceClient CreateClient()
		{
			IePlanifServiceClient client;

			try
			{
				client = new IePlanifServiceClient("ePlanif", "net.tcp://" + server + ":8523/ePlanif");
				client.Open();
				return client;
			}
			catch (Exception ex)
			{
				ViewModel.Log(ex);
				return null;
			}
		}

		public async Task<bool> ConnectAsync(string HostName)
		{
			this.server = HostName;

			IePlanifServiceClient client = CreateClient();
			if (client == null) return false;
			try
			{
				UserAccount = await client.GetCurrentAccountAsync();
				UserProfile = await client.GetCurrentProfileAsync();
				client.Close();
				await Options.LoadAsync();
				StartDate = FirstDayOfWeek(DateTime.Now);
			}
			catch (Exception ex)
			{
				Log(ex);
				client.Abort();
				return false;
			}
			if ((UserAccount == null) || (UserProfile == null)) return false;
			return await LoadAsync();
			
		}

		
		



		public DateTime FirstDayOfWeek(DateTime Date)
		{
			DayOfWeek firstDayOfWeek;
			
			if (Option == null) firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			else firstDayOfWeek = Option.FirstDayOfWeek.Value;

			int diff = Date.DayOfWeek - firstDayOfWeek;
			if (diff < 0) diff += 7;
			return Date.AddDays(-1 * diff).Date;
			
		}

		private void UpdateWeekNumber()
		{
			//DayOfWeek d;
			CalendarWeekRule rule;
			DayOfWeek firstDayOfWeek;

			if (Option == null)
			{
				rule = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
				firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			}
			else
			{
				rule = Option.CalendarWeekRule.Value;
				firstDayOfWeek = Option.FirstDayOfWeek.Value;
			}

			//DateTimeFormatInfo dfi = CultureInfo.CurrentCulture.DateTimeFormat;
			//Calendar cal = dfi.Calendar;
			//int t=(int)DayOfWeek.Friday;
			//d = (DayOfWeek)t;
						
			WeekName = "W"+ CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(StartDate, rule, firstDayOfWeek).ToString("D2");
		}

		

		protected override async Task<string> OnLoadModelAsync()
		{
			return await System.Threading.Tasks.Task.FromResult("ePlanifService");
		}


		protected override async Task<bool> OnLoadingAsync()
		{
			commandManager.Clear();
			return await base.OnLoadingAsync();
		}

		private bool OnRefreshCommandCanExecute(object Parameter)
		{
			return IsLoaded && (!IsLoading);
		}
		private async void OnRefreshCommandExecute(object Parameter)
		{
			await LoadAsync();
		}


		private bool OnUndoCommandCanExecute(object Parameter)
		{
			return commandManager.CanUndo && (!IsLoading);
		}
		private async void OnUndoCommandExecute(object Parameter)
		{
			await commandManager.UndoAsync();
		}

		private bool OnRedoCommandCanExecute(object Parameter)
		{
			return commandManager.CanRedo && (!IsLoading);
		}
		private async void OnRedoCommandExecute(object Parameter)
		{
			await commandManager.RedoAsync();
		}




		private bool OnCurrentWeekCommandCanExecute(object Parameter)
		{
			return IsLoaded && (!IsLoading);
		}
		private async void OnCurrentWeekCommandExecute(object Parameter)
		{
			StartDate = FirstDayOfWeek(DateTime.Now);
			await LoadAsync();
		}
		private bool OnPreviousWeekCommandCanExecute(object Parameter)
		{
			return IsLoaded && (!IsLoading);
		}
		private async void OnPreviousWeekCommandExecute(object Parameter)
		{
			StartDate=StartDate.AddDays(-7);
			await LoadAsync();
		}
		private bool OnNextWeekCommandCanExecute(object Parameter)
		{
			return IsLoaded && (!IsLoading);
		}
		private async void OnNextWeekCommandExecute(object Parameter)
		{
			StartDate = StartDate.AddDays(7);
			await LoadAsync();
		}

		private bool OnRemoveDaysCommandCanExecute(object Parameter)
		{
			return (IsLoaded) && (DaysCount > 7) && (!IsLoading);
		}
		private async void OnRemoveDaysCommandExecute(object Parameter)
		{
			DaysCount-=7;
			await LoadAsync();
		}
		private bool OnAddDaysCommandCanExecute(object Parameter)
		{
			return (IsLoaded) && (DaysCount < 28) && (!IsLoading);
		}
		private async void OnAddDaysCommandExecute(object Parameter)
		{
			DaysCount+=7;
			await LoadAsync();
		}

		

	}
}
