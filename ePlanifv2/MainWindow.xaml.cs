using ePlanifv2.Views;
using ePlanifViewModelsLib;
using ModelLib;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using ViewModelLib;
using ViewLib;

namespace ePlanifv2
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        private ePlanifServiceViewModel vm;
		//private LyncWorker lyncWorker;

		public static readonly DependencyProperty LogVisibilityProperty = DependencyProperty.Register("LogVisibility", typeof(Visibility), typeof(MainWindow),new PropertyMetadata(Visibility.Collapsed));
		public Visibility LogVisibility
		{
			get { return (Visibility)GetValue(LogVisibilityProperty); }
			set { SetValue(LogVisibilityProperty, value); }
		}

		public MainWindow()
        {
			//lyncWorker = new LyncWorker();
			//lyncWorker.Start();
			InitializeComponent();
		}

		/*protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			lyncWorker.Stop();
		}*/

		private async void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((vm!=null) && (vm.IsLoaded)) await vm.LoadAsync();
		}

		
		private void ShowError(Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error occured");
        }


		private void ConnectCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm == null) ; e.Handled = true;
		}

		private async void ConnectCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ConnectWindow window;


			window = new ConnectWindow() { Owner = this };

			window.Server = global::ePlanifv2.Properties.Settings.Default.Server;
			window.Login = (Environment.UserDomainName+"\\"+ Environment.UserName).ToLower();

			
			if (window.ShowDialog()??false)
			{
				global::ePlanifv2.Properties.Settings.Default.Server = window.Server;
				global::ePlanifv2.Properties.Settings.Default.Save();

				vm = new ePlanifServiceViewModel();
				DataContext = vm;
				if (!await vm.ConnectAsync(window.Server))
				{
					DataContext = null;
					vm = null;
					ShowError(new Exception("Cannot connect to server"));
				}
				//else lyncWorker.Refresh(vm);


			}
		}

		private void DisconnectCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm != null); e.Handled = true;
		}

		private void DisconnectCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			vm = null;
			DataContext = null;
			//lyncWorker.Refresh(vm);
		}

		private void AdministrationCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm!=null) && vm.IsLoaded;e.Handled = true;
		}

		private async void AdministrationCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AdministrationWindow window;

			window = new AdministrationWindow() { Owner = this,DataContext=vm };
			window.ShowDialog();
			await vm.LoadAsync();
		}

		private IViewViewModel GetSelectedView()
		{
			TabByActivityTypeViewModel byActivityTypeTab;
			TabByEmployeeViewModel byEmployeeTab;

			if ((vm == null) || (!vm.IsLoaded)) return null;

			byActivityTypeTab = vm.Tabs?.SelectedItem as TabByActivityTypeViewModel;
			byEmployeeTab = vm.Tabs?.SelectedItem as TabByEmployeeViewModel;

			if (byActivityTypeTab != null) return vm.ActivityTypeViews.SelectedItem;
			if (byEmployeeTab != null) return vm.EmployeeViews.SelectedItem;

			return null;
		}
		private void ReplicateCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = GetSelectedView()?.HasActivitySelected??false;
		}

		private void ReplicateCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ReplicateWindow window;
			IViewViewModel table;

			e.Handled = true;

			table = GetSelectedView();
			if (table == null) return;

			window = new ReplicateWindow() { Owner = this,Table=table };
			window.ShowDialog();
		}

		private void BulkDeleteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm != null) && vm.IsLoaded; e.Handled = true;
		}

		private async void BulkDeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			BulkDeleteWindow window;

			e.Handled = true;

			window = new BulkDeleteWindow() { Owner = this, ePlanifServiceViewModel = vm};
			if (window.ShowDialog()??false)	await vm.LoadAsync();
		}

		private void OpenReportCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = (vm != null) && (vm.Reports.SelectedItem != null);
		}

		private void OpenReportCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ReportWindow window;

			window = new ReportWindow() { Owner = this, ReportServerURL = @"http://"+vm.Server+"/ReportServer", ReportPath = @"/ePlanif_Reports/" + vm.Reports.SelectedItem.Model };
			window.ShowDialog();

		}

		private void ShowLogCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true; e.Handled = true;
		}

		private void ShowLogCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (LogVisibility == Visibility.Collapsed) LogVisibility = Visibility.Visible;
			else LogVisibility = Visibility.Collapsed;
		}

		private void AboutCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true; e.Handled = true;
		}

		private void AboutCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AboutWindow window = new AboutWindow() {Owner=this };
			
			window.ShowDialog();
		}

		private void FitColumnsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm!=null)&& (vm.IsLoaded); e.Handled = true;
		}

		private void FitColumnsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DisplayOptions.Instance.ColumnWidth = (System.Windows.SystemParameters.PrimaryScreenWidth - 250) / 7;
		}

		private void RestoreColumnsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm != null) && (vm.IsLoaded); e.Handled = true;
		}

		private void RestoreColumnsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DisplayOptions.Instance.ColumnWidth = 200;
		}

		private void IncColumnsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm != null) && (vm.IsLoaded) && (DisplayOptions.Instance.ColumnWidth < 400); e.Handled = true;
		}

		private void IncColumnsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DisplayOptions.Instance.ColumnWidth +=10;
		}

		private void DecColumnsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm != null) && (vm.IsLoaded) && (DisplayOptions.Instance.ColumnWidth > 100); e.Handled = true;
		}

		private void DecColumnsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DisplayOptions.Instance.ColumnWidth -= 10;
		}

		private void SearchCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = GetSelectedView()!=null; e.Handled = true;
		}

		private void SearchCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SearchWindow window;

			window = new SearchWindow() { Owner = this, ViewViewModel=GetSelectedView() };
			window.ShowDialog();//*/
		}

		private void EditOptionsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (vm != null) && (vm.IsLoaded) && (vm.Option!=null); e.Handled = true;
		}

		private async void EditOptionsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (await vm.Options.EditAsync(vm.Option))
			{
				vm.StartDate = vm.FirstDayOfWeek(DateTime.Now);
				await vm.LoadAsync();
			}
		}






	}
}
