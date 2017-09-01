using ePlanifViewModelsLib.ePlanifService;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class CountryCodeViewModelCollection:WCFViewModelCollection<CountryCodeViewModel,string>
	{

		public CountryCodeViewModelCollection(ePlanifServiceViewModel Service)
			:base(Service)
		{
		}

		protected override Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, CountryCodeViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, CountryCodeViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, CountryCodeViewModel ViewModel)
		{
			throw new NotImplementedException();
		}



		protected override Task<string> OnCreateEmptyModelAsync()
		{
			return Task.FromResult("NA");
		}

		protected override Task<CountryCodeViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new CountryCodeViewModel(Service));
		}

		

		protected override async Task<IEnumerable<string>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			List<string> items;
			

			items = new List<string>();
			foreach(string model in Enum.GetNames(typeof(CountryCode)))
			{
				items.Add(model);
			}
			await Task.Yield();
			return items;

			
		}

		
	}
}
