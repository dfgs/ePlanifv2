﻿using ePlanifModelsLib;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModelLib;

namespace ConfigurationTool
{
	public class Context : DependencyObject, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private ePlanifDatabase database;
		private ePlanifUpgrader upgrader;


		public static readonly DependencyProperty CreateDatabaseCommandProperty = DependencyProperty.Register("CreateDatabaseCommand", typeof(ViewModelCommand), typeof(Context));
		public ViewModelCommand CreateDatabaseCommand
		{
			get { return (ViewModelCommand)GetValue(CreateDatabaseCommandProperty); }
			private set { SetValue(CreateDatabaseCommandProperty, value); }
		}


		public static readonly DependencyProperty UpgradeDatabaseCommandProperty = DependencyProperty.Register("UpgradeDatabaseCommand", typeof(ViewModelCommand), typeof(Context));
		public ViewModelCommand UpgradeDatabaseCommand
		{
			get { return (ViewModelCommand)GetValue(UpgradeDatabaseCommandProperty); }
			set { SetValue(UpgradeDatabaseCommandProperty, value); }
		}

		public static readonly DependencyProperty CreateAccountCommandProperty = DependencyProperty.Register("CreateAccountCommand", typeof(ViewModelCommand), typeof(Context));
		public ViewModelCommand CreateAccountCommand
		{
			get { return (ViewModelCommand)GetValue(CreateAccountCommandProperty); }
			set { SetValue(CreateAccountCommandProperty, value); }
		}

		public static readonly DependencyProperty CreateSQLLoginCommandProperty = DependencyProperty.Register("CreateSQLLoginCommand", typeof(ViewModelCommand), typeof(Context));
		public ViewModelCommand CreateSQLLoginCommand
		{
			get { return (ViewModelCommand)GetValue(CreateSQLLoginCommandProperty); }
			set { SetValue(CreateSQLLoginCommandProperty, value); }
		}

		public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(Context),new PropertyMetadata("Idle"));
		public string Status
		{
			get { return (string)GetValue(StatusProperty); }
			private set { SetValue(StatusProperty, value); }
		}


		public static readonly DependencyProperty IsDatabaseFoundProperty = DependencyProperty.Register("IsDatabaseFound", typeof(bool), typeof(Context));
		public bool IsDatabaseFound
		{
			get { return (bool)GetValue(IsDatabaseFoundProperty); }
			private set { SetValue(IsDatabaseFoundProperty, value); }
		}


		public static readonly DependencyProperty IsDatabaseUpToDateProperty = DependencyProperty.Register("IsDatabaseUpToDate", typeof(bool), typeof(Context));
		public bool IsDatabaseUpToDate
		{
			get { return (bool)GetValue(IsDatabaseUpToDateProperty); }
			private set { SetValue(IsDatabaseUpToDateProperty, value); }
		}


		public static readonly DependencyProperty DatabaseRevisionProperty = DependencyProperty.Register("DatabaseRevision", typeof(int), typeof(Context));
		public int DatabaseRevision
		{
			get { return (int)GetValue(DatabaseRevisionProperty); }
			private set { SetValue(DatabaseRevisionProperty, value); }
		}



		public static readonly DependencyProperty SQLLoginProperty = DependencyProperty.Register("SQLLogin", typeof(string), typeof(Context));
		public string SQLLogin
		{
			get { return (string)GetValue(SQLLoginProperty); }
			set { SetValue(SQLLoginProperty, value); }
		}


		public static readonly DependencyProperty LoginProperty = DependencyProperty.Register("Login", typeof(string), typeof(Context));
		public string Login
		{
			get { return (string)GetValue(LoginProperty); }
			set { SetValue(LoginProperty, value); }
		}


		public Context()
		{
			CreateDatabaseCommand = new ViewModelCommand(OnCreateDatabaseCommandCanExecute, OnCreateDatabaseCommandExecute);
			UpgradeDatabaseCommand = new ViewModelCommand(OnUpgradeDatabaseCommandCanExecute, OnUpgradeDatabaseCommandExecute);
			CreateSQLLoginCommand = new ViewModelCommand(OnCreateSQLLoginCommandCanExecute, OnCreateSQLLoginCommandExecute);
			CreateAccountCommand = new ViewModelCommand(OnCreateAccountCommandCanExecute, OnCreateAccountCommandExecute);

			database = new ePlanifDatabase("localhost");
			upgrader = new ePlanifUpgrader(database);

			Login = (Environment.UserDomainName + "\\" + Environment.UserName).ToLower();
		}


		protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName=null)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
		}

		public async Task LoadAsync()
		{
			IsDatabaseUpToDate = false;
			DatabaseRevision = 0;

			IsDatabaseFound = await database.ExistsAsync();
			if (!IsDatabaseFound)
			{
				CommandManager.InvalidateRequerySuggested();
				return;
			}

			DatabaseRevision = await upgrader.GetDatabaseRevisionAsync();
			IsDatabaseUpToDate = DatabaseRevision == upgrader.GetTargetRevision();

			try
			{ 
				RegistryKey localMachineKey = Registry.LocalMachine; //sets the regKey for Local Machine node in regedit.
				RegistryKey fileServiceKey = localMachineKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\ePlanif server");
				SQLLogin = (String)fileServiceKey.GetValue("ObjectName");
			}
			catch
			{
				SQLLogin = null;
			}

			CommandManager.InvalidateRequerySuggested();
		}



		private bool OnCreateDatabaseCommandCanExecute(object arg)
		{
			return !IsDatabaseFound;
		}

		private async void OnCreateDatabaseCommandExecute(object obj)
		{
			try
			{
				Status = "Running";
				database.SetMaxRevision(0);
				await database.CreateAsync();
				Status = "Idle";
			}
			catch (Exception ex)
			{
				Status = ex.Message;
			}
			await LoadAsync();
		}

		private bool OnUpgradeDatabaseCommandCanExecute(object arg)
		{
			return (IsDatabaseFound) && (!IsDatabaseUpToDate);
		}

		private async void OnUpgradeDatabaseCommandExecute(object obj)
		{
			Status = "Running";
			if (!await upgrader.UpgradeAsync()) Status = upgrader.ErrorMessage;
			else Status = "Idle";
			await LoadAsync();
		}

		private bool OnCreateAccountCommandCanExecute(object arg)
		{
			return (IsDatabaseUpToDate) && (!string.IsNullOrEmpty(Login));
		}

		private async void OnCreateAccountCommandExecute(object obj)
		{
			Account account;

			account = new Account() { Login=Login.ToLower(),ProfileID=1 };
			try
			{
				Status = "Running";
				await database.InsertAsync(account);
				Status = "Idle";
				MessageBox.Show(Application.Current.MainWindow, $"Account {account.Login} created succesfully");
			}
			catch (Exception ex)
			{
				Status = ex.Message;
			}
			await LoadAsync();
		}

		private bool OnCreateSQLLoginCommandCanExecute(object arg)
		{
			return (IsDatabaseUpToDate) && (!string.IsNullOrEmpty(SQLLogin));
		}

		private async void OnCreateSQLLoginCommandExecute(object obj)
		{
			SqlCommand command;


			try
			{
				Status = "Running";
				command = new SqlCommand(
					$"if not exists (select loginname from master.dbo.syslogins  where name = '{SQLLogin}' ) " +
					"begin " +
						$"CREATE LOGIN [{SQLLogin}] FROM WINDOWS WITH DEFAULT_DATABASE = ePlanifDatabase; " +
					"end"
					);
				await database.ExecuteAsync(command);

				command = new SqlCommand(
					"if not exists (select * from sys.database_principals where name = 'ePlanifService' ) " +
					"begin " +
						$"CREATE USER ePlanifService FOR LOGIN[{SQLLogin}]; " +
					"end"
					);
				await database.ExecuteAsync(command);

				command = new SqlCommand("ALTER ROLE db_datareader ADD MEMBER ePlanifService;");
				await database.ExecuteAsync(command);

				command = new SqlCommand("ALTER ROLE db_datawriter ADD MEMBER ePlanifService;");
				await database.ExecuteAsync(command);

				command = new SqlCommand("ALTER ROLE db_procexecutor ADD MEMBER ePlanifService;");
				await database.ExecuteAsync(command);

				Status = "Idle";
				MessageBox.Show(Application.Current.MainWindow, $"SQL login {SQLLogin} created succesfully");
			}
			catch (Exception ex)
			{
				Status = ex.Message;
			}
			await LoadAsync();
		}


	}
}
