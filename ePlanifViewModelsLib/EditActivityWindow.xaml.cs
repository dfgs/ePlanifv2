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

		public static readonly DependencyProperty DefaultAMCommandProperty = DependencyProperty.Register("DefaultAMCommand", typeof(ViewModelCommand), typeof(EditActivityWindow));
		public ViewModelCommand DefaultAMCommand
		{
			get { return (ViewModelCommand)GetValue(DefaultAMCommandProperty); }
			set { SetValue(DefaultAMCommandProperty, value); }
		}


		public static readonly DependencyProperty DefaultPMCommandProperty = DependencyProperty.Register("DefaultPMCommand", typeof(ViewModelCommand), typeof(EditActivityWindow));
		public ViewModelCommand DefaultPMCommand
		{
			get { return (ViewModelCommand)GetValue(DefaultPMCommandProperty); }
			set { SetValue(DefaultPMCommandProperty, value); }
		}


		public static readonly DependencyProperty ActivityTypesProperty = DependencyProperty.Register("ActivityTypes", typeof(IEnumerable<ActivityTypeViewModel>), typeof(EditActivityWindow));
		public IEnumerable<ActivityTypeViewModel> ActivityTypes
		{
			get { return (IEnumerable<ActivityTypeViewModel>)GetValue(ActivityTypesProperty); }
			set { SetValue(ActivityTypesProperty, value); }
		}

		public EditActivityWindow()
		{
			InitializeComponent();
			DefaultAMCommand = new ViewModelCommand(DefaultAMCommandCanExecute, DefaultAMCommandExecute);
			DefaultPMCommand = new ViewModelCommand(DefaultPMCommandCanExecute, DefaultPMCommandExecute);
		}


		private bool DefaultAMCommandCanExecute(object arg)
		{
			ViewModelSchema schema;
			int? activityTypeID;
			ActivityTypeViewModel activityType;

			
			schema = DataContext as ViewModelSchema;
			if (schema == null) return false; ;

			activityTypeID = (int?)schema["ActivityTypeID"].Value;
			activityType = ActivityTypes.FirstOrDefault(item => item.ActivityTypeID == activityTypeID);
			if (activityType == null) return false;
			return activityType.DefaultStartTimeAM!=null;
		}


		private void DefaultAMCommandExecute(object obj)
		{
			ViewModelSchema schema;

			int? activityTypeID;
			ActivityTypeViewModel activityType;

			schema = DataContext as ViewModelSchema;
			if (schema == null) return;

			activityTypeID = (int?)schema["ActivityTypeID"].Value;
			activityType = ActivityTypes.FirstOrDefault(item => item.ActivityTypeID == activityTypeID);
			if (activityType == null) return;

			if (activityType.DefaultStartTimeAM != null)
			{
				schema["StartTime"].Value = activityType.DefaultStartTimeAM.Value; schema["StartTime"].IsLocked = false;
				if (activityType.DefaultDurationAM != null)
				{
					schema["StopTime"].Value = activityType.DefaultStartTimeAM.Value + activityType.DefaultDurationAM.Value;
					schema["StopTime"].IsLocked = false;
				}
			}
		}


		private bool DefaultPMCommandCanExecute(object arg)
		{
			ViewModelSchema schema;
			int? activityTypeID;
			ActivityTypeViewModel activityType;


			schema = DataContext as ViewModelSchema;
			if (schema == null) return false; ;

			activityTypeID = (int?)schema["ActivityTypeID"].Value;
			activityType = ActivityTypes.FirstOrDefault(item => item.ActivityTypeID == activityTypeID);
			if (activityType == null) return false;
			return activityType.DefaultStartTimePM != null;
		}

		private void DefaultPMCommandExecute(object obj)
		{
			ViewModelSchema schema;

			int? activityTypeID;
			ActivityTypeViewModel activityType;

			schema = DataContext as ViewModelSchema;
			if (schema == null) return;

			activityTypeID = (int?)schema["ActivityTypeID"].Value;
			activityType = ActivityTypes.FirstOrDefault(item => item.ActivityTypeID == activityTypeID);
			if (activityType == null) return;

			if (activityType.DefaultStartTimePM != null)
			{
				schema["StartTime"].Value = activityType.DefaultStartTimePM.Value; schema["StartTime"].IsLocked = false;
				if (activityType.DefaultDurationPM != null)
				{
					schema["StopTime"].Value = activityType.DefaultStartTimePM.Value + activityType.DefaultDurationPM.Value;
					schema["StopTime"].IsLocked = false;
				}
			}
		}

		

		

		


	}
}
