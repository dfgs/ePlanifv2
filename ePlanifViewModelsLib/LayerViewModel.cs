using ePlanifModelsLib;
using ModelLib;
using System.Threading.Tasks;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class LayerViewModel : DisableViewModel<Layer>
    {

        public int? LayerID
        {
            get { return Model.LayerID; }
            set { Model.LayerID = value; OnPropertyChanged(); }
        }


		[TextProperty(Header = "Name", IsMandatory = true, IsReadOnly = false)]
		public Text? Name
		{
			get { return Model.Name; }
			set { Model.Name = value; OnPropertyChanged(); }
		}

		[ColorProperty(Header = "Color", IsMandatory = true, IsReadOnly = false)]
		public Text? Color
		{
			get { return Model.Color; }
			set { Model.Color = value; OnPropertyChanged(); }
		}



		public LayerViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
			
		}

	
		protected override Task<Layer> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		public override bool IsModelEqualTo(Layer Other)
		{
			return Model.LayerID==Other.LayerID;
		}

	}
}
