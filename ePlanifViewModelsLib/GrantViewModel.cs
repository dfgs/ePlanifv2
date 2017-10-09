using ePlanifModelsLib;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class GrantViewModel : WCFViewModel<Grant>
    {

        public int? GrantID
        {
            get { return Model.GrantID; }
            set { Model.GrantID = value; OnPropertyChanged(); }
        }

		[IntListProperty(Header = "Group", IsMandatory = true, IsReadOnly = true, DisplayMemberPath = "Name", SelectedValuePath = "GroupID", SourcePath = "Service.Groups")]
		public int? GroupID
		{
			get { return Model.GroupID; }
			set { Model.GroupID = value; OnPropertyChanged(); GroupProperty.Invalidate(this); }
		}

		public int? ProfileID
		{
			get { return Model.ProfileID; }
			set { Model.ProfileID = value; OnPropertyChanged(); }
		}


		[BoolProperty(Header = "Write access", IsMandatory = true, IsReadOnly = false)]
		public bool? WriteAccess
		{
			get { return Model.WriteAccess; }
			set { Model.WriteAccess = value; OnPropertyChanged(); }
		}

		private static ForeignProperty<GrantViewModel, GroupViewModel> GroupProperty = new ForeignProperty<GrantViewModel, GroupViewModel>((component) => component.Service.Groups, (component, item) => component.GroupID == item.GroupID);
		public GroupViewModel Group
		{
			get { return GroupProperty.GetValue(this) ; }
		}

		public GrantViewModel(ePlanifServiceViewModel Service):base(Service)
		{
			
		}
		
		protected override Task<Grant> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		
	}
}
