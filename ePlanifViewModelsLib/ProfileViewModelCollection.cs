using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class ProfileViewModelCollection : DisableViewModelCollection<ProfileViewModel, Profile>
    {
		

		public ProfileViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {

		}
		

		protected override Task<Profile> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new Profile() { });
		}

		protected override Task<ProfileViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new ProfileViewModel(Service));
		}
		

						

		protected override async Task<IEnumerable<Profile>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			if (Service.UserProfile.AdministrateAccounts == true) return await Client.GetProfilesAsync();
			else return Enumerable.Empty<Profile>();
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, ProfileViewModel ViewModel)
		{
			ViewModel.ProfileID= await Client.CreateProfileAsync(ViewModel.Model);
			return ViewModel.ProfileID > 0;
			
		}


		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, ProfileViewModel ViewModel)
		{
			return await Client.UpdateProfileAsync(ViewModel.Model);
			

		}



	}
}
