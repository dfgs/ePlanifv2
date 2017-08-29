using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class EmployeeViewModelCollection : DisableViewModelCollection<EmployeeViewModel, Employee>
    {
        public EmployeeViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}
		protected override Task<Employee> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new Employee() { });
		}

		protected override Task<EmployeeViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new EmployeeViewModel(Service));
		}

		protected override async Task<IEnumerable<Employee>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			return await Client.GetEmployeesAsync();
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, EmployeeViewModel ViewModel)
		{
			ViewModel.EmployeeID= await Client.CreateEmployeeAsync(ViewModel.Model);
			return ViewModel.EmployeeID > 0;
			


		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, EmployeeViewModel ViewModel)
		{
			return await Client.UpdateEmployeeAsync(ViewModel.Model);
			

		}





	}
}
