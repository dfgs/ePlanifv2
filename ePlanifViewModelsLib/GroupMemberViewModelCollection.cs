using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class GroupMemberViewModelCollection : WCFViewModelCollection<GroupMemberViewModel, GroupMember>
    {

		private GroupViewModel group;


		public GroupMemberViewModelCollection(ePlanifServiceViewModel Service, GroupViewModel Group):base(Service) 
        {
			this.group = Group;

		}

		

		protected override Task<GroupMember> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new GroupMember() );
		}

		protected override Task<GroupMemberViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new GroupMemberViewModel(Service));
		}


		protected override async Task<IEnumerable<GroupMember>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			if (group.GroupID == null) return new GroupMember[] { };
			return await Client.GetGroupMembersAsync(group.GroupID.Value);
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, GroupMemberViewModel ViewModel)
		{
			ViewModel.GroupMemberID= await Client.CreateGroupMemberAsync(ViewModel.Model);
			return ViewModel.GroupMemberID > 0;
			

		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, GroupMemberViewModel ViewModel)
		{
			return await Client.DeleteGroupMemberAsync(ViewModel.GroupMemberID.Value);
			

		}

		protected override  Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, GroupMemberViewModel ViewModel)
		{
			throw (new NotImplementedException("Cannot update group member"));

		}



	}
}
