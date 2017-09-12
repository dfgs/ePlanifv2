using ePlanifModelsLib;
using ModelLib;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class ProfileViewModel : DisableViewModel<Profile>, IGroupViewModel
	{

        public int? ProfileID
        {
            get { return Model.ProfileID; }
            set { Model.ProfileID = value; OnPropertyChanged(); }
        }


		[TextProperty(Header = "Name", IsMandatory = true, IsReadOnly = false)]
		public Text? Name
		{
			get { return Model.Name; }
			set { Model.Name=value ; OnPropertyChanged(); }
		}



		[BoolProperty(Header = "Administrate employees", IsMandatory = true, IsReadOnly = false)]
		public bool? AdministrateEmployees
		{
			get { return Model.AdministrateEmployees; }
			set { Model.AdministrateEmployees = value; OnPropertyChanged(); }
		}

		[BoolProperty(Header = "Administrate activity types", IsMandatory = true, IsReadOnly = false)]
		public bool? AdministrateActivityTypes
		{
			get { return Model.AdministrateActivityTypes; }
			set { Model.AdministrateActivityTypes = value; OnPropertyChanged(); }
		}

		

		[BoolProperty(Header = "Administrate accounts", IsMandatory = true, IsReadOnly = false)]
		public bool? AdministrateAccounts
		{
			get { return Model.AdministrateAccounts; }
			set { Model.AdministrateAccounts = value; OnPropertyChanged(); }
		}

		[BoolProperty(Header = "Can run reports", IsMandatory = true, IsReadOnly = false)]
		public bool? CanRunReports
		{
			get { return Model.CanRunReports; }
			set { Model.CanRunReports = value; OnPropertyChanged(); }
		}//*/

		[BoolProperty(Header = "Self write access", IsMandatory = true, IsReadOnly = false)]
		public bool? SelfWriteAccess
		{
			get { return Model.SelfWriteAccess; }
			set { Model.SelfWriteAccess = value; OnPropertyChanged(); }
		}

		private GrantViewModelCollection members;
		public GrantViewModelCollection Members
		{
			get { return members; }
		}

		public ProfileViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
			members = new GrantViewModelCollection(Service, this);Children.Add(members) ;
		}
		
		protected override Task<Profile> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		async Task<bool> IGroupViewModel.AddMemberAsync(object Member)
		{
			GroupViewModel vm;
			Grant member;

			vm = Member as GroupViewModel;
			if (vm == null) return false;
			if (!members.IsLoaded)
			{
				if (!await members.LoadAsync()) return false;
			}
			member = new Grant() { ProfileID = this.ProfileID, GroupID = vm.GroupID};
			return (await members.AddAsync(member) != null);
		}


	}
}
