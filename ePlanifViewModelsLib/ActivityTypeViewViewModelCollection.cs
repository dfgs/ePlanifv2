using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifViewModelsLib
{
	public class ActivityTypeViewViewModelCollection : WCFViewModelCollection<ActivityTypeViewViewModel, ActivityTypeView>
    {

		


		public ActivityTypeViewViewModelCollection(ePlanifServiceViewModel Service):base(Service) 
        {
		

		}

		

		protected override Task<ActivityTypeView> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new ActivityTypeView() { AccountID = Service.UserAccount.AccountID });
		}

		protected override Task<ActivityTypeViewViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new ActivityTypeViewViewModel(Service));
		}


		protected override async Task<IEnumerable<ActivityTypeView>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			return await Client.GetActivityTypeViewsAsync();
		}
		
		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, ActivityTypeViewViewModel ViewModel)
		{
			ViewModel.ActivityTypeViewID= await Client.CreateActivityTypeViewAsync(ViewModel.Model);
			return ViewModel.ActivityTypeViewID > 0;
			
		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, ActivityTypeViewViewModel ViewModel)
		{
			return await Client.DeleteActivityTypeViewAsync(ViewModel.ActivityTypeViewID.Value);
			

		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, ActivityTypeViewViewModel ViewModel)
		{
			return await Client.UpdateActivityTypeViewAsync(ViewModel.Model);
			

		}



	}
}
