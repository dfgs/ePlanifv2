using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class ActivityTypeViewMemberViewModelCollection : WCFViewModelCollection<ActivityTypeViewMemberViewModel, ActivityTypeViewMember>
    {

		private ActivityTypeViewViewModel view;

		public ActivityTypeViewMemberViewModelCollection(ePlanifServiceViewModel Service, ActivityTypeViewViewModel View):base(Service) 
        {
			this.view = View;

		}


		

		protected override Task<ActivityTypeViewMember> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new ActivityTypeViewMember() );
		}

		protected override Task<ActivityTypeViewMemberViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new ActivityTypeViewMemberViewModel(Service));
		}


		protected override async Task<IEnumerable<ActivityTypeViewMember>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			if (view.ActivityTypeViewID == null) return new ActivityTypeViewMember[] { };
			return await Client.GetActivityTypeViewMembersAsync(view.ActivityTypeViewID.Value);
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, ActivityTypeViewMemberViewModel ViewModel)
		{
			ViewModel.ActivityTypeViewMemberID= await Client.CreateActivityTypeViewMemberAsync(ViewModel.Model);
			return ViewModel.ActivityTypeViewMemberID > 0;
			

		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, ActivityTypeViewMemberViewModel ViewModel)
		{
			return await Client.DeleteActivityTypeViewMemberAsync(ViewModel.ActivityTypeViewMemberID.Value);
			
		}

		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, ActivityTypeViewMemberViewModel ViewModel)
		{
			throw (new NotImplementedException("Cannot update view member"));
		}



	}
}
