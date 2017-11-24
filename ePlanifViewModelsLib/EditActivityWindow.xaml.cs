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
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	/// <summary>
	/// Logique d'interaction pour EditActivityWindow.xaml
	/// </summary>
	public partial class EditActivityWindow : Window
	{
		public EditActivityWindow()
		{
			InitializeComponent();
		}

		private void ButtonAM_Click(object sender, RoutedEventArgs e)
		{
			ViewModelSchema schema;
			ActivityViewModel activity;
			

			schema = DataContext as ViewModelSchema;
			if (schema == null) return;

			activity = (ActivityViewModel)schema.ViewModel;
			if (activity.ActivityType.DefaultStartTimeAM != null) 
			{
				schema["StartTime"].Value=activity.ActivityType.DefaultStartTimeAM.Value;
				if (activity.ActivityType.DefaultDurationAM != null) schema["StopTime"].Value = activity.ActivityType.DefaultStartTimeAM.Value + activity.ActivityType.DefaultDurationAM.Value;
			}
			

		}

		private void ButtonPM_Click(object sender, RoutedEventArgs e)
		{
			ViewModelSchema schema;
			ActivityViewModel activity;


			schema = DataContext as ViewModelSchema;
			if (schema == null) return;

			activity = (ActivityViewModel)schema.ViewModel;
			if (activity.ActivityType.DefaultStartTimePM != null)
			{
				schema["StartTime"].Value = activity.ActivityType.DefaultStartTimePM.Value;
				if (activity.ActivityType.DefaultDurationPM != null) schema["StopTime"].Value = activity.ActivityType.DefaultStartTimePM.Value + activity.ActivityType.DefaultDurationPM.Value;
			}

		}


	}
}
