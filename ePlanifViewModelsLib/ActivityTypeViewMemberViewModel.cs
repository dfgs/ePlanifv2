using ePlanifModelsLib;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib;

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
			set { Model.ActivityTypeID = value; OnPropertyChanged(); ActivityTypeProperty.Invalidate(this); }
		}

		public int? ActivityTypeViewID
		{
			get { return Model.ActivityTypeViewID; }
			set { Model.ActivityTypeViewID = value; OnPropertyChanged(); }
		}



		private static ForeignProperty<ActivityTypeViewMemberViewModel, ActivityTypeViewModel> ActivityTypeProperty = new ForeignProperty<ActivityTypeViewMemberViewModel, ActivityTypeViewModel>((component) => component.Service.ActivityTypes, (component, item) => component.ActivityTypeID == item.ActivityTypeID);
		public ActivityTypeViewModel ActivityType
		{
			get { return ActivityTypeProperty.GetValue(this); ; }
		}


		public bool? IsDisabled
		{
			get { return ActivityType?.IsDisabled; }
		}
		public int RowID
		{
			get { return ActivityTypeID.Value; }
		}

		public ActivityTypeViewMemberViewModel(ePlanifServiceViewModel Service):base(Service)
		{
		
		}

		

		protected override Task<ActivityTypeViewMember> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		public bool StartsWith(char Key)
		{
			return ActivityType.Name.Value.Value.StartsWith(Key.ToString(), true, CultureInfo.CurrentCulture);
		}

		// /!\ used by excel export /!\
		public override string ToString()
		{
			return ActivityType.Name.Value.Value;
		}


	}
}
