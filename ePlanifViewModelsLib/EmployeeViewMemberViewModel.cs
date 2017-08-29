using ePlanifModelsLib;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class EmployeeViewMemberViewModel : WCFViewModel<EmployeeViewMember>,IRowViewModel
    {

        public int? EmployeeViewMemberID
        {
            get { return Model.EmployeeViewMemberID; }
            set { Model.EmployeeViewMemberID = value; OnPropertyChanged(); }
        }

		public int? EmployeeID
		{
			get { return Model.EmployeeID; }
			set { Model.EmployeeID = value; OnPropertyChanged(); OnPropertyChanged("Employee"); }
		}

		public int? EmployeeViewID
		{
			get { return Model.EmployeeViewID; }
			set { Model.EmployeeViewID = value; OnPropertyChanged(); }
		}

		private EmployeeViewModel employee;
		public EmployeeViewModel Employee
		{
			get { return employee; }
		}
		public bool? IsDisabled
		{
			get { return employee?.IsDisabled; }
		}
		public int RowID
		{
			get { return EmployeeID.Value; }
		}

		public EmployeeViewMemberViewModel(ePlanifServiceViewModel Service):base(Service)
		{
			
		}

		protected override async Task OnLoadedAsync()
		{
			await base.OnLoadedAsync();
			employee = Service.Employees.FirstOrDefault(item => item.EmployeeID == EmployeeID);
		}


		protected override Task<EmployeeViewMember> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		public bool StartsWith(char Key)
		{
			return Employee.LastName.Value.Value.StartsWith(Key.ToString(), true, CultureInfo.CurrentCulture);
		}


	}
}
