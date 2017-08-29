using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class GrantViewModelCollection : WCFViewModelCollection<GrantViewModel, Grant>
    {

		private ProfileViewModel profile;

		public GrantViewModelCollection(ePlanifServiceViewModel Service, ProfileViewModel Profile) :base(Service) 
        {
			this.profile = Profile;

		}

		protected override Task<Grant> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new Grant() { ProfileID=(Service).UserAccount.ProfileID});
		}

		protected override Task<GrantViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new GrantViewModel(Service));
		}

		protected override async Task<IEnumerable<Grant>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			if (profile.ProfileID == null) return new Grant[] { };
			return await Client.GetGrantsAsync(profile.ProfileID.Value);
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, GrantViewModel ViewModel)
		{
			ViewModel.GrantID= await Client.CreateGrantAsync(ViewModel.Model);
			return ViewModel.GrantID > 0;
			

		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, GrantViewModel ViewModel)
		{
			return await Client.DeleteGrantAsync(ViewModel.GrantID.Value);
			

		}

		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, GrantViewModel ViewModel)
		{
			throw (new NotImplementedException("Cannot update grant"));

		}





	}
}
