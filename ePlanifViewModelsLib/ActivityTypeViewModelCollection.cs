using ePlanifModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;
using ModelLib;
using ePlanifViewModelsLib.ePlanifService;

namespace ePlanifViewModelsLib
{
    public class ActivityTypeViewModelCollection : DisableViewModelCollection<ActivityTypeViewModel, ActivityType>
    {
        public ActivityTypeViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}
		protected override Task<ActivityType> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new ActivityType() { });
		}

		protected override Task<ActivityTypeViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new ActivityTypeViewModel(Service));
		}

		protected override async Task<IEnumerable<ActivityType>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			return await Client.GetActivityTypesAsync();
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, ActivityTypeViewModel ViewModel)
		{
			ViewModel.ActivityTypeID= await Client.CreateActivityTypeAsync(ViewModel.Model);
			return ViewModel.ActivityTypeID > 0;
			
		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, ActivityTypeViewModel ViewModel)
		{
			return await Client.UpdateActivityTypeAsync(ViewModel.Model);
			
		}




	}
}
