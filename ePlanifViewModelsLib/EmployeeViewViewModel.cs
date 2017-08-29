using ePlanifModelsLib;
using ModelLib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModelLib.Attributes;
using System;
using System.Collections.ObjectModel;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class EmployeeViewViewModel : ViewViewModel<EmployeeView,EmployeeViewMemberViewModel>,IGroupViewModel
    {
		
		public int? EmployeeViewID
        {
            get { return Model.EmployeeViewID; }
            set { Model.EmployeeViewID = value; OnPropertyChanged(); }
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
			get { return Service.VisibleLayers.SelectedItem?.LayerID??0; }
		}

		private EmployeeViewMemberViewModelCollection members;
		public EmployeeViewMemberViewModelCollection Members
		{
			get { return members; }
		}

		private FilteredViewModelCollection<EmployeeViewMemberViewModel, EmployeeViewMember> visibleMembers;
		public override IViewModelCollection<EmployeeViewMemberViewModel> VisibleMembers
		{
			get { return visibleMembers; }
		}


		public EmployeeViewViewModel(ePlanifServiceViewModel Service):base(Service)
		{
			members = new EmployeeViewMemberViewModelCollection(Service, this);Children.Add(members);
			visibleMembers = new FilteredViewModelCollection<EmployeeViewMemberViewModel, EmployeeViewMember>(members, (item) => item.IsDisabled != true); Children.Add(visibleMembers);
		}

		protected override int GetLayerID(ActivityViewModel Activity)
		{
			return Activity.ActivityType?.LayerID??0;
		}

		public override bool IsModelEqualTo(EmployeeView Other)
		{
			return Other.EmployeeViewID == Model.EmployeeViewID;
		}

		protected override Task<EmployeeView> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		

		async Task<bool> IGroupViewModel.AddMemberAsync(object Member)
		{
			EmployeeViewModel vm;
			EmployeeViewMember member;

			vm = Member as EmployeeViewModel;
			if (vm == null) return false;
			if (!members.IsLoaded)
			{
				if (!await members.LoadAsync()) return false;
			}
			member = new EmployeeViewMember() { EmployeeViewID = this.EmployeeViewID, EmployeeID = vm.EmployeeID};
			return (await members.AddAsync(member)!=null);
		}


		protected override bool IsActivityBindedTo(ActivityViewModel Activity, EmployeeViewMemberViewModel Row)
		{
			return Activity.EmployeeID == Row.EmployeeID;
		}

		protected override void SetRowID(ActivityViewModel Activity, int RowID)
		{
			Activity.EmployeeID = RowID;
		}

		protected override bool OnValidateCell(CellViewModel Cell)
		{
			ObservableCollection<ActivityViewModel> activities;

			activities = Cell.GetActivities(LayerID);
			
			if (activities.Count> 1)
			{
				for (int t = 0; t < activities.Count - 1; t++)
				{
					if (Overlap(activities[t], activities[t + 1])) return true;
				}
			}
			return false;
		}


	}
}
