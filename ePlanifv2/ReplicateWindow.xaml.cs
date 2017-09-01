using ePlanifViewModelsLib;
using ModelLib;
using System;
using System.Windows;
using System.Windows.Input;

namespace ePlanifv2
{
	/// <summary>
	/// Logique d'interaction pour ReplicateWindow.xaml
	/// </summary>
	public partial class ReplicateWindow : Window
	{

		public static readonly DependencyProperty TableProperty = DependencyProperty.Register("Table", typeof(IViewViewModel), typeof(ReplicateWindow));
		public IViewViewModel Table
		{
			get { return (IViewViewModel)GetValue(TableProperty); }
			set { SetValue(TableProperty, value); }
		}


		public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register("EndDate", typeof(DateTime), typeof(ReplicateWindow),new PropertyMetadata(DateTime.Now));
		public DateTime EndDate
		{
			get { return (DateTime)GetValue(EndDateProperty); }
			set { SetValue(EndDateProperty, value); }
		}


		public static readonly DependencyProperty SkipPublicHolidaysProperty = DependencyProperty.Register("SkipPublicHolidays", typeof(bool), typeof(ReplicateWindow),new PropertyMetadata(true));
		public bool SkipPublicHolidays
		{
			get { return (bool)GetValue(SkipPublicHolidaysProperty); }
			set { SetValue(SkipPublicHolidaysProperty, value); }
		}

		private bool isRunning;

		public ReplicateWindow()
		{
			InitializeComponent();
		}


		private void OKCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !isRunning;
			e.Handled = true;
		}

		private async void OKCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			isRunning = true;
			DialogResult = await Table.ReplicateActivitiesAsync(EndDate,SkipPublicHolidays);
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
