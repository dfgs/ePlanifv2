using ePlanifModelsLib;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib;

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
			set { Model.EmployeeID = value; OnPropertyChanged(); EmployeeProperty.Invalidate(this); }
		}

		public int? EmployeeViewID
		{
			get { return Model.EmployeeViewID; }
			set { Model.EmployeeViewID = value; OnPropertyChanged(); }
		}

		private static ForeignProperty<EmployeeViewMemberViewModel, EmployeeViewModel> EmployeeProperty = new ForeignProperty<EmployeeViewMemberViewModel, EmployeeViewModel>((component) => component.Service.Employees, (component, item) => component.EmployeeID == item.EmployeeID);
		public EmployeeViewModel Employee
		{
			get { return EmployeeProperty.GetValue(this); }
		}
		public bool? IsDisabled
		{
			get { return Employee?.IsDisabled; }
		}
		public int RowID
		{
			get { return EmployeeID.Value; }
		}

		public EmployeeViewMemberViewModel(ePlanifServiceViewModel Service):base(Service)
		{
			
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
