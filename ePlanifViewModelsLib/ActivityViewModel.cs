using DatabaseModelLib;
using ePlanifModelsLib;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
    public class ActivityViewModel : WCFViewModel<Activity>
    {
		public int? ActivityID
        {
            get { return Model.ActivityID; }
            set { Model.ActivityID = value; OnPropertyChanged(); }
        }


		[DateTimeProperty(Header = "Date", IsMandatory = true, IsReadOnly = false)]
		public DateTime? Date
		{
			get { return Model.StartDate; }
			set
			{
				Model.StartDate = value.Value.Date + Model.StartDate.Value.TimeOfDay; OnPropertyChanged();
			}
		}
		[TimeProperty(Header = "Start time", IsMandatory = true, IsReadOnly = false)]
		public DateTime? StartTime
		{
			get { return Model.StartDate; }
			set
			{
				Model.StartDate = Model.StartDate.Value.Date + value.Value.TimeOfDay; OnPropertyChanged(); OnPropertyChanged("StopTime");
			}
		}
		public DateTime? StopTime
		{
			get { return Model.StartDate+Model.Duration; }
		}

		[TimeSpanProperty(Header = "Duration", IsMandatory = true, IsReadOnly = false)]
		public TimeSpan? Duration
		{
			get { return Model.Duration; }
			set { Model.Duration = value; OnPropertyChanged(); OnPropertyChanged("StopTime"); }
		}
		[TimeSpanProperty(Header = "Tracked duration", IsMandatory = false, IsReadOnly = false)]
		public TimeSpan? TrackedDuration
		{
			get { return Model.TrackedDuration; }
			set { Model.TrackedDuration = value; OnPropertyChanged();  }
		}

		/*[BoolProperty(Header = "Is all day", IsMandatory = true, IsReadOnly = false)]
		public bool? IsAllDay
		{
			get { return Model.IsAllDay; }
			set { Model.IsAllDay = value; OnPropertyChanged(); }
		}*/


		[TextProperty(Header = "Comment", IsMandatory = false, IsReadOnly = false)]
        public Text? Comment
        {
            get { return Model.Comment; }
            set { Model.Comment = value; OnPropertyChanged(); }
        }


		[ListProperty(Header = "Activity", IsMandatory = true, IsReadOnly = false, DisplayMemberPath = "Name", SelectedValuePath = "ActivityTypeID", SourcePath = "Service.VisibleActivityTypes")]
		public int? ActivityTypeID
		{
			get { return Model.ActivityTypeID; }
			set { Model.ActivityTypeID = value; OnPropertyChanged(); OnPropertyChanged("ActivityType"); }
		}
		public ActivityTypeViewModel ActivityType
		{
			get { return (Service).ActivityTypes.FirstOrDefault(item => item.ActivityTypeID == ActivityTypeID); }
		}

		[ListProperty(Header = "Employee", IsMandatory = true, IsReadOnly = false, DisplayMemberPath = "FullName", SelectedValuePath = "EmployeeID", SourcePath = "Service.VisibleEmployees")]
		public int? EmployeeID
		{
			get { return Model.EmployeeID; }
			set { Model.EmployeeID = value; OnPropertyChanged(); OnPropertyChanged("Employee"); }
		}

		[TextProperty(Header = "Project number", IsMandatory = false, IsReadOnly = false)]
		public int? ProjectNumber
		{
			get { return Model.ProjectNumber; }
			set { Model.ProjectNumber = value; OnPropertyChanged(); }
		}



		[TextProperty(Header = "Remedy reference", IsMandatory = true, IsReadOnly = false)]
		public Text? RemedyRef
		{
			get { return Model.RemedyRef; }
			set { Model.RemedyRef = value; OnPropertyChanged(); }
		}

		[BoolProperty(Header = "Is draft activity", IsMandatory = true, IsReadOnly = false)]
		public bool? IsDraft
		{
			get { return Model.IsDraft; }
			set { Model.IsDraft = value; OnPropertyChanged(); }
		}



		public EmployeeViewModel Employee
		{
			get { return (Service).Employees.FirstOrDefault(item => item.EmployeeID == EmployeeID); }
		}


		public Text? Color
		{
			get { return ActivityType?.BackgroundColor; }
		}
		public Text? TextColor
		{
			get { return ActivityType?.TextColor; }
		}


		public ActivityViewModel(ePlanifServiceViewModel Service) : base(Service)
		{
			
			
		}

		public override bool IsModelEqualTo(Activity Other)
		{
			return Other.ActivityID == Model.ActivityID;
		}

		protected override Task<Activity> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
		}

		public int Compare(ActivityViewModel Other)
		{
			return Model.StartDate.Value.CompareTo(Other.Model.StartDate.Value);
		}

		public async Task<ActivityViewModel> CloneAsync()
		{
			ActivityViewModel result;
			Activity model;

			model = new Activity();Schema<Activity>.Clone(Model, model);
			result = new ActivityViewModel(Service);
			await result.LoadAsync(model);
			return result;
		}

		



	}
}
