using ePlanifModelsLib;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class ActivityTypeViewMemberViewModel : WCFViewModel<ActivityTypeViewMember>,IRowViewModel
    {

        public int? ActivityTypeViewMemberID
        {
            get { return Model.ActivityTypeViewMemberID; }
            set { Model.ActivityTypeViewMemberID = value; OnPropertyChanged(); }
        }

		public int? ActivityTypeID
		{
			get { return Model.ActivityTypeID; }
			set { Model.ActivityTypeID = value; OnPropertyChanged(); OnPropertyChanged("ActivityType"); }
		}

		public int? ActivityTypeViewID
		{
			get { return Model.ActivityTypeViewID; }
			set { Model.ActivityTypeViewID = value; OnPropertyChanged(); }
		}

		

		private ActivityTypeViewModel activityType;
		public ActivityTypeViewModel ActivityType
		{
			get { return activityType; }
		}
		public bool? IsDisabled
		{
			get { return activityType?.IsDisabled; }
		}
		public int RowID
		{
			get { return ActivityTypeID.Value; }
		}

		public ActivityTypeViewMemberViewModel(ePlanifServiceViewModel Service):base(Service)
		{
		
		}

		protected override async Task OnLoadedAsync()
		{
			await base.OnLoadedAsync();
			activityType = Service.ActivityTypes.FirstOrDefault(item => item.ActivityTypeID == ActivityTypeID);
		}

		protected override Task<ActivityTypeViewMember> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		public bool StartsWith(char Key)
		{
			return ActivityType.Name.Value.Value.StartsWith(Key.ToString(), true, CultureInfo.CurrentCulture);
		}


	}
}
