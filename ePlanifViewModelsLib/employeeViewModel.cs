using ePlanifModelsLib;
using ModelLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class EmployeeViewModel : DisableViewModel<Employee>
    {

        public int? EmployeeID
        {
            get { return Model.EmployeeID; }
            set { Model.EmployeeID = value; OnPropertyChanged(); }
        }


		[TextProperty(Header = "First name", IsMandatory = true, IsReadOnly = false)]
		public Text? FirstName
		{
			get { return Model.FirstName; }
			set { Model.FirstName = value; OnPropertyChanged(); OnPropertyChanged("FullName"); }
		}

		[TextProperty(Header = "Last name", IsMandatory = true, IsReadOnly = false)]
		public Text? LastName
		{
			get { return Model.LastName; }
			set { Model.LastName = value; OnPropertyChanged(); OnPropertyChanged("FullName"); }
		}

		[ByteProperty(Header = "Max working hours per week", IsMandatory = false, IsReadOnly = false)]
		public byte? MaxWorkingHoursPerWeek
		{
			get { return Model.MaxWorkingHoursPerWeek; }
			set { Model.MaxWorkingHoursPerWeek = value; OnPropertyChanged(); }
		}

		public string FullName
		{
			get { return LastName + " " + FirstName; }
		}




		public EmployeeViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
			
		}
		public TimeSpan GetTotalHours()
		{
			return TimeSpan.FromTicks( Service.Activities.Where(item =>(item.ActivityType?.LayerID==Service.VisibleLayers.SelectedItem?.LayerID) &&  (item.EmployeeID == EmployeeID) && (item.Date<Service.StartDate.AddDays(7))  ).Sum(item=> item.TrackedDuration.HasValue?item.TrackedDuration.Value.Ticks:item.Duration.Value.Ticks) );
		}

		protected override Task<Employee> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		

	}
}
