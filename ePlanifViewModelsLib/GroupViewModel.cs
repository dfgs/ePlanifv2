using ePlanifModelsLib;
using ModelLib;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class GroupViewModel : WCFViewModel<Group>, IGroupViewModel
	{

        public int? GroupID
        {
            get { return Model.GroupID; }
            set { Model.GroupID = value; OnPropertyChanged(); }
        }

		public int? ParentGroupID
		{
			get { return Model.ParentGroupID; }
			set { Model.ParentGroupID = value; OnPropertyChanged();  }
		}

		[TextProperty(Header = "Name", IsMandatory = true, IsReadOnly = false)]
		public Text? Name
		{
			get { return Model.Name; }
			set { Model.Name = value; OnPropertyChanged(); }
		}

		private ObservableCollection<GroupViewModel> items;
		public ObservableCollection<GroupViewModel> Items
		{
			get { return items; }
		}

		private GroupMemberViewModelCollection members;
		public GroupMemberViewModelCollection Members
		{
			get { return members; }
		}


		public GroupViewModel(ePlanifServiceViewModel Service):base(Service)
		{
			items = new ObservableCollection<GroupViewModel>();
			members = new GroupMemberViewModelCollection(Service,this); Children.Add(members); // members must be loaded before groups in order to build viewmodels


		}

		protected override Task<Group> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		async Task<bool> IGroupViewModel.AddMemberAsync(object Member)
		{
			EmployeeViewModel vm;
			GroupMember member;

			vm = Member as EmployeeViewModel;
			if (vm == null) return false;
			if (!members.IsLoaded)
			{
				if (!await members.LoadAsync()) return false;
			}
			
			member = new GroupMember() { GroupID = this.GroupID, EmployeeID = vm.EmployeeID };
			return (await members.AddAsync(member)!=null);
		}


	}
}
