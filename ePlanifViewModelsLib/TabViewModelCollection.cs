using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifViewModelsLib
{

    public class TabViewModelCollection : WCFViewModelCollection<TabViewModel, object>
    {
		
		
	
		public TabViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}

		protected override Task<object> OnCreateEmptyModelAsync()
        {
			throw (new NotImplementedException());
		}

		protected override Task<TabViewModel> OnCreateViewModelItem(Type ModelType)
        {
			if (ModelType==typeof(EmployeeViewViewModelCollection)) return Task.FromResult<TabViewModel>(new TabByEmployeeViewModel(Service));
			if (ModelType == typeof(ActivityTypeViewViewModelCollection)) return Task.FromResult<TabViewModel>(new TabByActivityTypeViewModel(Service));
			throw (new NotImplementedException());
		}

		public override bool IsModelEqualTo(IEnumerable<object> Other)
		{
			bool result;

			result=base.IsModelEqualTo(Other);
			return result;
		}

		
		
		protected override  Task<IEnumerable<object>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			return Task.FromResult<IEnumerable<object>>(new object[] {Service.EmployeeViews, Service.ActivityTypeViews });
		}

		protected override Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, TabViewModel ViewModel)
		{
			throw (new NotImplementedException());
		}

		protected override Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, TabViewModel ViewModel)
		{
			throw (new NotImplementedException());
		}

		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, TabViewModel ViewModel)
		{
			throw (new NotImplementedException());
		}



		
	}
}
