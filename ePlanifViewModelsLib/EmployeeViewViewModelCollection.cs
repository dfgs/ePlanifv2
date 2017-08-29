using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifViewModelsLib
{
	public class EmployeeViewViewModelCollection : WCFViewModelCollection<EmployeeViewViewModel, EmployeeView>
    {



		public EmployeeViewViewModelCollection(ePlanifServiceViewModel Service):base(Service) 
        {
		

		}

		

		protected override Task<EmployeeView> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new EmployeeView() { AccountID = Service.UserAccount.AccountID });
		}

		protected override Task<EmployeeViewViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new EmployeeViewViewModel(Service));
		}


		protected override async Task<IEnumerable<EmployeeView>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			return await Client.GetEmployeeViewsAsync();
		}



		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, EmployeeViewViewModel ViewModel)
		{
			ViewModel.EmployeeViewID = await Client.CreateEmployeeViewAsync(ViewModel.Model);
			return ViewModel.EmployeeViewID > 0;
			

		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, EmployeeViewViewModel ViewModel)
		{
			return await Client.DeleteEmployeeViewAsync(ViewModel.EmployeeViewID.Value);
			

		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, EmployeeViewViewModel ViewModel)
		{
			return await Client.UpdateEmployeeViewAsync(ViewModel.Model);
			

		}



	}
}
