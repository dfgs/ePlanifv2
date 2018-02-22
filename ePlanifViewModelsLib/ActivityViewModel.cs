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


		[DateTimeProperty(Header = "Date", IsMandatory = true, IsReadOnly = false,Category ="Date/Time")]
		public DateTime? Date
		{
			get { return Model.StartDate; }
			set
			{
				Model.StartDate =DateTime.SpecifyKind(  value.Value.Date + Model.StartDate.Value.TimeOfDay, DateTimeKind.Unspecified); OnPropertyChanged();
			}
		}
		[TimeProperty(Header = "Start time", IsMandatory = true, IsReadOnly = false, Category = "Date/Time")]
		public DateTime? StartTime
		{
			get { return Model.StartDate; }
			set
			{
				Model.StartDate = DateTime.SpecifyKind(Model.StartDate.Value.Date + value.Value.TimeOfDay,DateTimeKind.Unspecified); OnPropertyChanged(); OnPropertyChanged("StopTime");
			}
		}
		[TimeProperty(Header = "Stop time", IsMandatory = true, IsReadOnly = false, Category = "Date/Time")]
		public DateTime? StopTime
		{
			get { return Model.StartDate+Model.Duration; }
			set
			{
				Duration = value.Value.TimeOfDay - StartTime.Value.TimeOfDay;
			}
		}

		//[TimeSpanProperty(Header = "Duration", IsMandatory = true, IsReadOnly = false)]
		public TimeSpan? Duration
		{
			get { return Model.Duration; }
			set { Model.Duration = value; OnPropertyChanged(); OnPropertyChanged("StopTime"); }
		}
		[TimeSpanProperty(Header = "Tracked duration", IsMandatory = false, IsReadOnly = false, Category = "Information")]
		public TimeSpan? TrackedDuration
		{
			get { return Model.TrackedDuration; }
			set { Model.TrackedDuration = value; OnPropertyChanged();  }
		}

		[TextProperty(Header = "Comment", IsMandatory = false, IsReadOnly = false, Category = "Information")]
        public Text? Comment
        {
            get { return Model.Comment; }
            set { Model.Comment = value; OnPropertyChanged(); }
        }


		[IntListProperty(Header = "Activity", IsMandatory = true, IsReadOnly = false, DisplayMemberPath = "Name", SelectedValuePath = "ActivityTypeID", SourcePath = "Service.VisibleActivityTypes", Category = "Information")]
		public int? ActivityTypeID
		{
			get { return Model.ActivityTypeID; }
			set
			{
				Model.ActivityTypeID = value; OnPropertyChanged(); ActivityTypeProperty.Invalidate(this);
				if (ActivityType.DefaultTrackedDuration != null) TrackedDuration = ActivityType.DefaultTrackedDuration;
			}
		}

		private static ForeignProperty<ActivityViewModel, ActivityTypeViewModel> ActivityTypeProperty = new ForeignProperty<ActivityViewModel, ActivityTypeViewModel>((component) => component.Service.ActivityTypes, (component, item) => component.ActivityTypeID == item.ActivityTypeID);
		public ActivityTypeViewModel ActivityType
		{
			get { return ActivityTypeProperty.GetValue(this); }
		}

		[IntListProperty(Header = "Employee", IsMandatory = true, IsReadOnly = false, DisplayMemberPath = "FullName", SelectedValuePath = "EmployeeID", SourcePath = "Service.WriteableEmployees", Category = "Information")]
		public int? EmployeeID
		{
			get { return Model.EmployeeID; }
			set { Model.EmployeeID = value;  OnPropertyChanged(); EmployeeProperty.Invalidate(this); }
		}

		[TextProperty(Header = "Project number", IsMandatory = false, IsReadOnly = false, Category = "Information")]	// text property because it is more a ref than a number
		public int? ProjectNumber
		{
			get { return Model.ProjectNumber; }
			set { Model.ProjectNumber = value; OnPropertyChanged(); }
		}



		[TextProperty(Header = "Remedy reference", IsMandatory = true, IsReadOnly = false, Category = "Information")]
		public Text? RemedyRef
		{
			get { return Model.RemedyRef; }
			set { Model.RemedyRef = value; OnPropertyChanged(); }
		}

		[BoolProperty(Header = "Is draft activity", IsMandatory = true, IsReadOnly = false, Category = "Information")]
		public bool? IsDraft
		{
			get { return Model.IsDraft; }
			set { Model.IsDraft = value; OnPropertyChanged(); }
		}


		private static ForeignProperty<ActivityViewModel, EmployeeViewModel> EmployeeProperty=new ForeignProperty<ActivityViewModel, EmployeeViewModel>( (component) => component.Service.Employees, (component,item) => component.EmployeeID==item.EmployeeID);
		public EmployeeViewModel Employee
		{
			get { return EmployeeProperty.GetValue(this); }
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

		/*public int Compare(ActivityViewModel Other)
		{
			return Model.StartDate.Value.CompareTo(Other.Model.StartDate.Value);
		}*/

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
