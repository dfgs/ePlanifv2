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
	/// Logique d'interaction pour SearchWindow.xaml
	/// </summary>
	public partial class SearchWindow : Window
	{
		private ActivityViewModel currentActivity;

		/*public static readonly DependencyProperty ActivitiesProperty = DependencyProperty.Register("Activities", typeof(ActivityViewModelCollection), typeof(SearchWindow));
		public ActivityViewModelCollection Activities
		{
			get { return (ActivityViewModelCollection)GetValue(ActivitiesProperty); }
			set { SetValue(ActivitiesProperty, value); }
		}*/


		public static readonly DependencyProperty ViewViewModelProperty = DependencyProperty.Register("ViewViewModel", typeof(IViewViewModel), typeof(SearchWindow));
		public IViewViewModel ViewViewModel
		{
			get { return (IViewViewModel)GetValue(ViewViewModelProperty); }
			set { SetValue(ViewViewModelProperty, value); }
		}


		public static readonly DependencyProperty ProjectNumberProperty = DependencyProperty.Register("ProjectNumber", typeof(string), typeof(SearchWindow));
		public string ProjectNumber
		{
			get { return (string)GetValue(ProjectNumberProperty); }
			set { SetValue(ProjectNumberProperty, value); }
		}

		public SearchWindow()
		{
			InitializeComponent();
			currentActivity = null;
		}
		private void searchWindow_Loaded(object sender, RoutedEventArgs e)
		{
			textBox.Focus();
		}

		private void CancelCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
			e.Handled = true;
		}

		private void CancelCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = false;
		}


		private void FindNextCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = ProjectNumber!=null;
			e.Handled = true;
		}

		private void FindNextCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			currentActivity = ViewViewModel.SearchProject(ProjectNumber, currentActivity);
			if (currentActivity == null) MessageBox.Show(this,  "No more activity found", "Done", MessageBoxButton.OK);
			else ViewViewModel.Focus(currentActivity);
		}
		

		
	}
}
