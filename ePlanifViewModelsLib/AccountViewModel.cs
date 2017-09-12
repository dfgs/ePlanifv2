using ePlanifModelsLib;
using ModelLib;
using System.Threading.Tasks;
using ViewModelLib;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class AccountViewModel : DisableViewModel<Account>
    {

        public int? AccountID
        {
            get { return Model.AccountID; }
            set { Model.AccountID = value; OnPropertyChanged(); }
        }


		[TextProperty(Header = "Login", IsMandatory = true, IsReadOnly = false)]
		public Text? Login
		{
			get { return Model.Login; }
			set { Model.Login = value.ToString().ToLower() ; OnPropertyChanged(); }
		}

		[IntListProperty(Header = "Employee", IsMandatory = false, IsReadOnly = false, DisplayMemberPath = "FullName", SelectedValuePath = "EmployeeID", SourcePath = "Service.Employees")]
		public int? EmployeeID
		{
			get { return Model.EmployeeID; }
			set { Model.EmployeeID = value; OnPropertyChanged(); }
		}


		

		[IntListProperty(Header = "Profile", IsMandatory = true, IsReadOnly = false, DisplayMemberPath = "Name", SelectedValuePath = "ProfileID", SourcePath = "Service.Profiles")]
		public int? ProfileID
		{
			get { return Model.ProfileID; }
			set { Model.ProfileID = value; OnPropertyChanged(); }
		}



		public AccountViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
			
		}
		
		protected override Task<Account> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		
	}
}
