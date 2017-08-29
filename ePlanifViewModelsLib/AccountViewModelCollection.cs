using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class AccountViewModelCollection : DisableViewModelCollection<AccountViewModel, Account>
    {
        public AccountViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}
		protected override Task<Account> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new Account() { });
		}

		protected override Task<AccountViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new AccountViewModel(Service));
		}
		

		protected override async Task<IEnumerable<Account>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			
			return await Client.GetAccountsAsync();
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client,AccountViewModel ViewModel)
		{
			ViewModel.AccountID= await Client.CreateAccountAsync(ViewModel.Model);
			return ViewModel.AccountID > 0;
		}


		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client,AccountViewModel ViewModel)
		{
			return await Client.UpdateAccountAsync(ViewModel.Model);
		}



	}
}
