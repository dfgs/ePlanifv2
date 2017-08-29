using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class GroupViewModelCollection : WCFViewModelCollection<GroupViewModel, Group>
    {
		private ObservableCollection<GroupViewModel> items;
		public ObservableCollection<GroupViewModel> Items
		{
			get { return items; }
		}


		

		public GroupViewModelCollection(ePlanifServiceViewModel Service):base(Service) 
        {
			items = new ObservableCollection<GroupViewModel>();
		}

		protected override async Task OnLoadedAsync()
		{
			await base.OnLoadedAsync();

			GroupViewModel parent;
			items.Clear();
			foreach(GroupViewModel item in this)
			{
				parent = this.FirstOrDefault(p=> p.GroupID == item.ParentGroupID);
				if (parent == null) items.Add(item);
				else parent.Items.Add(item);
			}


		}

		protected override bool OnRemoveCommandCanExecute(object Parameter)
		{
			return (SelectedItem!=null) && (SelectedItem.ParentGroupID!=null) && (SelectedItem.Items.Count==0) && (SelectedItem.Members.Count==0);
		}
		protected override bool OnAddCommandCanExecute(object Parameter)
		{
			return (SelectedItem != null);
		}
		

		protected override Task<Group> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new Group() {ParentGroupID=SelectedItem?.GroupID } );
		}

		protected override Task<GroupViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new GroupViewModel(Service));
		}
		public override async Task OnAddCommandExecuted(GroupViewModel ViewModel)
		{
			GroupViewModel parent;

			parent = this.FirstOrDefault(item => item.GroupID == ViewModel.ParentGroupID);
			if (parent != null) parent.Items.Add(ViewModel);

			await base.OnAddCommandExecuted(ViewModel);
		}
		public override async Task OnRemoveCommandExecuted(GroupViewModel ViewModel)
		{
			GroupViewModel parent;

			parent = this.FirstOrDefault(item => item.GroupID == ViewModel.ParentGroupID);
			if (parent != null) parent.Items.Remove(ViewModel);

			await base.OnRemoveCommandExecuted(ViewModel);
		}


		protected override async Task<IEnumerable<Group>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			return await Client.GetGroupsAsync();
			
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, GroupViewModel ViewModel)
		{
			ViewModel.GroupID=await Client.CreateGroupAsync(ViewModel.Model);
			return ViewModel.GroupID > 0;
			

		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, GroupViewModel ViewModel)
		{
			return await Client.DeleteGroupAsync(ViewModel.GroupID.Value);
			
		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, GroupViewModel ViewModel)
		{
			return await Client.UpdateGroupAsync(ViewModel.Model);
			

		}




	}
}
