using ePlanifModelsLib;
using System.Linq;
using System.Threading.Tasks;

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
			set { Model.EmployeeID = value; OnPropertyChanged(); OnPropertyChanged("Employee"); }
		}

		public int? GroupID
		{
			get { return Model.GroupID; }
			set { Model.GroupID = value; OnPropertyChanged(); }
		}

		public EmployeeViewModel Employee
		{
			get { return (Service).Employees.FirstOrDefault(item => item.EmployeeID == EmployeeID); }
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
