using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using ModelLib;
using Nager.Date;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib;
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

		[TextProperty(Header = "eMail", IsMandatory = false, IsReadOnly = false)]
		public Text? eMail
		{
			get { return Model.eMail; }
			set { Model.eMail = value; OnPropertyChanged();  }
		}

		[TextListProperty(Header = "Country", IsMandatory = true, IsReadOnly = false,  SelectedValuePath = "Model", DisplayMemberPath = null, SourcePath = "Service.CountryCodes")]
		public Text? CountryCode
		{
			get { return Model.CountryCode; }
			set { Model.CountryCode = value; OnPropertyChanged(); }
		}
		
		[LargeTimeSpanProperty(Header = "Working time per week", IsMandatory = false, IsReadOnly = false)]
		public TimeSpan? WorkingTimePerWeek
		{
			get { return Model.WorkingTimePerWeek.HasValue?TimeSpan.FromMinutes(Model.WorkingTimePerWeek.Value):(TimeSpan?)null; }
			set
			{
				if (value == null) Model.WorkingTimePerWeek = null;
				else Model.WorkingTimePerWeek = (ushort)value.Value.TotalMinutes;
				OnPropertyChanged();
			}
		}

		[LargeTimeSpanProperty(Header = "Max working time per week", IsMandatory = false, IsReadOnly = false)]
		public TimeSpan? MaxWorkingTimePerWeek
		{
			get { return Model.MaxWorkingTimePerWeek.HasValue ? TimeSpan.FromMinutes(Model.MaxWorkingTimePerWeek.Value) : (TimeSpan?)null; }
			set
			{
				if (value == null) Model.MaxWorkingTimePerWeek = null;
				else Model.MaxWorkingTimePerWeek = (ushort)value.Value.TotalMinutes;
				OnPropertyChanged();
			}
		}

		

		public bool? WriteAccess
		{
			get { return Model.WriteAccess; }
		}


		public string FullName
		{
			get { return LastName + " " + FirstName; }
		}

		private static ForeignProperty<EmployeeViewModel,PhotoViewModel> PhotoProperty=new ForeignProperty<EmployeeViewModel, PhotoViewModel>((component)=>component.Service.Photos, (component,item)=>component.EmployeeID==item.EmployeeID);
		public PhotoViewModel Photo
		{
			get { return PhotoProperty.GetValue(this); }
		}



		public EmployeeViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
		}

		public TimeSpan GetTotalHours()
		{
			return TimeSpan.FromTicks( Service.Activities.Where(item =>(item.ActivityType?.LayerID==Service.VisibleLayers.SelectedItem?.LayerID) &&  (item.EmployeeID == EmployeeID) && (item.Date<Service.StartDate.AddDays(7))  ).Sum(item=> item.TrackedDuration.HasValue?item.TrackedDuration.Value.Ticks:item.Duration.Value.Ticks) );
		}

		public async Task DeletePhotoAsync()
		{
			if (Photo == null) return;
			if (await Service.Photos.RemoveAsync(Photo)) PhotoProperty.Invalidate(this);
		}

		public async Task UploadPhotoAsync(byte[] PhotoData)
		{
			Photo model;

			
			if (Photo==null)
			{
				model = new ePlanifModelsLib.Photo() { EmployeeID=this.EmployeeID,Data=PhotoData };
				if (await Service.Photos.AddAsync(model, false) != null) PhotoProperty.Invalidate(this); 
			}
			else
			{
				Photo.Model.Data = PhotoData;
				if (await Service.Photos.EditAsync(Photo, false)) Photo.RefreshImage();
			}
		}

		protected override Task<Employee> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }
		public CountryCode GetCountryCode()
		{
			CountryCode code;
			if (!Enum.TryParse<CountryCode>(CountryCode.ToString(), out code)) code = Nager.Date.CountryCode.US;

			return code;
		}

		

	}
}
