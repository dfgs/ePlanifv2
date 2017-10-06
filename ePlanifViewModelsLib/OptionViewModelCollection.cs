using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class OptionViewModelCollection : WCFViewModelCollection<OptionViewModel, Option>
    {
        public OptionViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}
		protected override Task<Option> OnCreateEmptyModelAsync()
		{
			throw (new NotImplementedException());
		}

		protected override Task<OptionViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new OptionViewModel(Service));
		}
		

		protected override async Task<IEnumerable<Option>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			Option option;
			option=await Client.GetOptionAsync();
			if (option == null) return null;
			return new Option[] { option };
		}

		protected override Task<bool> OnAddInModelAsync(IePlanifServiceClient Client,OptionViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, OptionViewModel ViewModel)
		{
			throw new NotImplementedException();
		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client,OptionViewModel ViewModel)
		{
			return await Client.UpdateOptionAsync(ViewModel.Model);
		}

		

	}
}
