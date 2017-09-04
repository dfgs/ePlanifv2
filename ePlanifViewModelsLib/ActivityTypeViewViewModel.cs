using ePlanifModelsLib;
using ModelLib;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib.Attributes;
using System;
using System.Collections.Generic;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class ActivityTypeViewViewModel : ViewViewModel<ActivityTypeView,ActivityTypeViewMemberViewModel>,IGroupViewModel
    {
		

		public int? ActivityTypeViewID
        {
            get { return Model.ActivityTypeViewID; }
            set { Model.ActivityTypeViewID = value; OnPropertyChanged(); }
        }

		public int? AccountID
		{
			get { return Model.AccountID; }
			set { Model.AccountID = value; OnPropertyChanged();  }
		}


		[TextProperty(Header = "Name", IsMandatory = true, IsReadOnly = false)]
		public Text? Name
		{
			get { return Model.Name; }
			set { Model.Name = value; OnPropertyChanged(); }
		}

		public override int LayerID
		{
			get { return 0; }
		}


		private ActivityTypeViewMemberViewModelCollection members;
		public ActivityTypeViewMemberViewModelCollection Members
		{
			get { return members; }
		}

		private FilteredViewModelCollection<ActivityTypeViewMemberViewModel, ActivityTypeViewMember> visibleMembers;
		public override IViewModelCollection<ActivityTypeViewMemberViewModel> VisibleMembers
		{
			get { return visibleMembers; }
		}


		public ActivityTypeViewViewModel(ePlanifServiceViewModel Service):base(Service)
		{
			members = new ActivityTypeViewMemberViewModelCollection(Service, this); Children.Add(members);
			visibleMembers = new FilteredViewModelCollection<ActivityTypeViewMemberViewModel, ActivityTypeViewMember>(members,(item)=>item.IsDisabled!=true);Children.Add(visibleMembers);
		}

		protected override bool GetIsPublicHolyday(DateTime Data, int Row)
		{
			return false;
		}

		protected override int GetLayerID(ActivityViewModel Activity)
		{
			return 0;
		}

		public override bool IsModelEqualTo(ActivityTypeView Other)
		{
			return Other.ActivityTypeViewID == Model.ActivityTypeViewID;
		}

		protected override Task<ActivityTypeView> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		protected override bool HasWriteAccessOnRow(int RowID)
		{
			return true;
		}


		async Task<bool> IGroupViewModel.AddMemberAsync(object Member)
		{
			ActivityTypeViewModel vm;
			ActivityTypeViewMember member;

			vm=Member as ActivityTypeViewModel;
			if (vm == null) return false ;
			if (!members.IsLoaded)
			{
				if (!await members.LoadAsync()) return false;
			}
			member = new ActivityTypeViewMember() { ActivityTypeViewID=this.ActivityTypeViewID, ActivityTypeID= vm.ActivityTypeID };
			return (await members.AddAsync(member)!=null);
		}

		protected override bool IsActivityBindedTo(ActivityViewModel Activity, ActivityTypeViewMemberViewModel Row)
		{
			return Activity.ActivityTypeID == Row.ActivityTypeID;
		}

		protected override void SetRowID(ActivityViewModel Activity, int RowID)
		{
			Activity.ActivityTypeID = RowID;
		}

		protected override bool OnValidateCell(CellViewModel Cell)
		{
			ActivityTypeViewModel activityType;

			activityType = Service.ActivityTypes.FirstOrDefault(item => item.ActivityTypeID == Cell.RowID);

			if (!activityType.MinEmployees.HasValue) return false;
			return (Cell.GetActivities(LayerID)?.Count < activityType.MinEmployees.Value);

		}

		



	}
}
