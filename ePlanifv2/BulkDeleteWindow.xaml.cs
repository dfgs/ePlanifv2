using ePlanifViewModelsLib;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ePlanifv2
{
	/// <summary>
	/// Logique d'interaction pour BulkDeleteWindow.xaml
	/// </summary>
	public partial class BulkDeleteWindow : Window
	{


		public static readonly DependencyProperty ePlanifServiceViewModelProperty = DependencyProperty.Register("ePlanifServiceViewModel", typeof(ePlanifServiceViewModel), typeof(BulkDeleteWindow));
		public ePlanifServiceViewModel ePlanifServiceViewModel
		{
			get { return (ePlanifServiceViewModel)GetValue(ePlanifServiceViewModelProperty); }
			set { SetValue(ePlanifServiceViewModelProperty, value); }
		}

		public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(DateTime), typeof(BulkDeleteWindow), new PropertyMetadata(DateTime.Now));
		public DateTime StartDate
		{
			get { return (DateTime)GetValue(StartDateProperty); }
			set { SetValue(StartDateProperty, value); }
		}

		public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register("EndDate", typeof(DateTime), typeof(BulkDeleteWindow), new PropertyMetadata(DateTime.Now));
		public DateTime EndDate
		{
			get { return (DateTime)GetValue(EndDateProperty); }
			set { SetValue(EndDateProperty, value); }
		}

		public static readonly DependencyProperty EmployeeIDProperty = DependencyProperty.Register("EmployeeID", typeof(int?), typeof(BulkDeleteWindow));
		public int? EmployeeID
		{
			get { return (int?)GetValue(EmployeeIDProperty); }
			set { SetValue(EmployeeIDProperty, value); }
		}

		private bool isRunning;

		public BulkDeleteWindow()
		{
			InitializeComponent();
		}


		private void OKCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (EmployeeID.HasValue) && (!isRunning);
			e.Handled = true;
		}

		private async void OKCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (MessageBox.Show(Application.Current.MainWindow, "Are you sure to delete all activities ?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				isRunning = true;

				DialogResult = await ePlanifServiceViewModel.Activities.BulkDeleteAsync(StartDate.Date, EndDate.Date.AddHours(24).AddMinutes(59).AddSeconds(59), EmployeeID.Value);
			}
		}
		private void CancelCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !isRunning;
			e.Handled = true;
		}

		private void CancelCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = false;
		}

	}
}
