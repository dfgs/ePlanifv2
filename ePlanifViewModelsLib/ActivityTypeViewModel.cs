using ePlanifModelsLib;
using ModelLib;
using System.Linq;
using System.Threading.Tasks;
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

		[ListProperty(Header = "Layer", IsMandatory = true, IsReadOnly = false, DisplayMemberPath = "Name", SelectedValuePath = "LayerID", SourcePath = "Service.VisibleLayers")]
		public int? LayerID
		{
			get { return Model.LayerID; }
			set { Model.LayerID = value; OnPropertyChanged(); OnPropertyChanged("Layer"); }
		}


		[IntProperty(Header = "Mininum employees", IsMandatory = false, IsReadOnly = false,MinValue =0)]
		public int? MinEmployees
		{
			get { return Model.MinEmployees; }
			set { Model.MinEmployees = value; OnPropertyChanged(); }
		}

		public LayerViewModel Layer
		{
			get { return Service.Layers.FirstOrDefault(item=>item.LayerID==LayerID); }
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
