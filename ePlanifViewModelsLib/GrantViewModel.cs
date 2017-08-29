using ePlanifModelsLib;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class GrantViewModel : WCFViewModel<Grant>
    {

        public int? GrantID
        {
            get { return Model.GrantID; }
            set { Model.GrantID = value; OnPropertyChanged(); }
        }

		public int? GroupID
		{
			get { return Model.GroupID; }
			set { Model.GroupID = value; OnPropertyChanged(); OnPropertyChanged("Group"); }
		}

		public int? ProfileID
		{
			get { return Model.ProfileID; }
			set { Model.ProfileID = value; OnPropertyChanged(); }
		}

		public GroupViewModel Group
		{
			get { return (Service).Groups.FirstOrDefault(item => item.GroupID == GroupID); }
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
