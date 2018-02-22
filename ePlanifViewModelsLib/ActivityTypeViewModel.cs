using ePlanifModelsLib;
using ModelLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class ActivityTypeViewModel : DisableViewModel<ActivityType>
    {

        public int? ActivityTypeID
        {
            get { return Model.ActivityTypeID; }
            set { Model.ActivityTypeID = value; OnPropertyChanged(); }
        }


        [TextProperty(Header = "Name", IsMandatory = true, IsReadOnly = false)]
        public Text? Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(); }
        }


		[ColorProperty(Header = "Background color", IsMandatory = true, IsReadOnly = false)]
		public Text? BackgroundColor
		{
			get { return Model.BackgroundColor; }
			set { Model.BackgroundColor = value; OnPropertyChanged(); }
		}

		[ColorProperty(Header = "Text color", IsMandatory = true, IsReadOnly = false)]
		public Text? TextColor
		{
			get { return Model.TextColor; }
			set { Model.TextColor = value; OnPropertyChanged(); }
		}

		[IntListProperty(Header = "Layer", IsMandatory = true, IsReadOnly = false, DisplayMemberPath = "Name", SelectedValuePath = "LayerID", SourcePath = "Service.VisibleLayers")]
		public int? LayerID
		{
			get { return Model.LayerID; }
			set { Model.LayerID = value; OnPropertyChanged(); LayerProperty.Invalidate(this); }
		}


		[IntProperty(Header = "Mininum employees", IsMandatory = false, IsReadOnly = false,MinValue =0)]
		public int? MinEmployees
		{
			get { return Model.MinEmployees; }
			set { Model.MinEmployees = value; OnPropertyChanged(); }
		}

		[TimeProperty(Header = "Default start time AM", IsMandatory = false, IsReadOnly = false)]
		public DateTime? DefaultStartTimeAM
		{
			get { return Model.DefaultStartTimeAM; }
			set { Model.DefaultStartTimeAM = value; OnPropertyChanged(); }
		}
		[TimeSpanProperty(Header = "Default duration AM", IsMandatory = false, IsReadOnly = false)]
		public TimeSpan? DefaultDurationAM
		{
			get { return Model.DefaultDurationAM; }
			set { Model.DefaultDurationAM = value; OnPropertyChanged();}
		}


		[TimeProperty(Header = "Default start time PM", IsMandatory = false, IsReadOnly = false)]
		public DateTime? DefaultStartTimePM
		{
			get { return Model.DefaultStartTimePM; }
			set { Model.DefaultStartTimePM = value; OnPropertyChanged(); }
		}
		[TimeSpanProperty(Header = "Default duration PM", IsMandatory = false, IsReadOnly = false)]
		public TimeSpan? DefaultDurationPM
		{
			get { return Model.DefaultDurationPM; }
			set { Model.DefaultDurationPM = value; OnPropertyChanged(); }
		}

		[TimeSpanProperty(Header = "Default tracked duration", IsMandatory = false, IsReadOnly = false)]
		public TimeSpan? DefaultTrackedDuration
		{
			get { return Model.DefaultTrackedDuration; }
			set { Model.DefaultTrackedDuration = value; OnPropertyChanged(); }
		}

		private static ForeignProperty<ActivityTypeViewModel, LayerViewModel> LayerProperty = new ForeignProperty<ActivityTypeViewModel, LayerViewModel>((component) => component.Service.Layers, (component, item) => component.LayerID == item.LayerID);
		public LayerViewModel Layer
		{
			get { return LayerProperty.GetValue(this); }
		}

		public ActivityTypeViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
			
		}

		protected override Task<ActivityType> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		

	}
}
