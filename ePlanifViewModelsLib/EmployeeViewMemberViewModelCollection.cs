using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifViewModelsLib
{
	public class EmployeeViewMemberViewModelCollection : WCFViewModelCollection<EmployeeViewMemberViewModel, EmployeeViewMember>
    {


		private EmployeeViewViewModel view;

		public EmployeeViewMemberViewModelCollection(ePlanifServiceViewModel Service, EmployeeViewViewModel View) :base(Service) 
        {
			this.view = View;

		}





		protected override Task<EmployeeViewMember> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new EmployeeViewMember() );
		}

		protected override Task<EmployeeViewMemberViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new EmployeeViewMemberViewModel(Service));
		}


		protected override async Task<IEnumerable<EmployeeViewMember>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			if (view.EmployeeViewID == null) return new EmployeeViewMember[] { };
			return await Client.GetEmployeeViewMembersAsync(view.EmployeeViewID.Value);
		}
		

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, EmployeeViewMemberViewModel ViewModel)
		{
			ViewModel.EmployeeViewMemberID= await Client.CreateEmployeeViewMemberAsync(ViewModel.Model);
			return ViewModel.EmployeeViewMemberID > 0;
		
		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, EmployeeViewMemberViewModel ViewModel)
		{
			return await Client.DeleteEmployeeViewMemberAsync(ViewModel.EmployeeViewMemberID.Value);
			

		}

		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, EmployeeViewMemberViewModel ViewModel)
		{
			throw (new NotImplementedException("Cannot update view member"));


		}



	}
}
