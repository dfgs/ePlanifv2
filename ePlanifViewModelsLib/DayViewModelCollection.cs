using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class DayViewModelCollection:WCFViewModelCollection<DayViewModel,DateTime>
	{

		public DayViewModelCollection(ePlanifServiceViewModel Service)
			:base(Service)
		{
		}

		protected override Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, DayViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, DayViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, DayViewModel ViewModel)
		{
			throw new NotImplementedException();
		}



		protected override Task<DateTime> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(DateTime.Now);
		}

		protected override Task<DayViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new DayViewModel(Service));
		}

		

		protected override async Task<IEnumerable<DateTime>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			List<DateTime> items;

			items = new List<DateTime>();
			for(int t=0;t<Service.DaysCount;t++)
			{
				items.Add(Service.StartDate.AddDays(t).Date);
			}
			await Task.Yield();
			return items;

			
		}

		
	}
}
