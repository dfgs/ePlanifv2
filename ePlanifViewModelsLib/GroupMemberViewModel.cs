using ePlanifModelsLib;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class GroupMemberViewModel : WCFViewModel<GroupMember>
    {

        public int? GroupMemberID
        {
            get { return Model.GroupMemberID; }
            set { Model.GroupMemberID = value; OnPropertyChanged(); }
        }

		public int? EmployeeID
		{
			get { return Model.EmployeeID; }
			set { Model.EmployeeID = value; OnPropertyChanged(); EmployeeProperty.Invalidate(this); }
		}

		public int? GroupID
		{
			get { return Model.GroupID; }
			set { Model.GroupID = value; OnPropertyChanged(); }
		}

		private static ForeignProperty<GroupMemberViewModel, EmployeeViewModel> EmployeeProperty = new ForeignProperty<GroupMemberViewModel, EmployeeViewModel>((component) => component.Service.Employees, (component, item) => component.EmployeeID == item.EmployeeID);
		public EmployeeViewModel Employee
		{
			get { return EmployeeProperty.GetValue(this); }
		}

		public GroupMemberViewModel(ePlanifServiceViewModel Service):base(Service)
		{
			
		}
		
		protected override Task<GroupMember> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		
	}
}
