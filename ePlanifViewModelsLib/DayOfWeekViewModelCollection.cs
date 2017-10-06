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
	public class DayOfWeekViewModelCollection:WCFViewModelCollection<DayOfWeekViewModel,DayOfWeek>
	{

		public DayOfWeekViewModelCollection(ePlanifServiceViewModel Service)
			:base(Service)
		{
		}

		protected override Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, DayOfWeekViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, DayOfWeekViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, DayOfWeekViewModel ViewModel)
		{
			throw new NotImplementedException();
		}



		protected override Task<DayOfWeek> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(DayOfWeek.Monday);
		}

		protected override Task<DayOfWeekViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new DayOfWeekViewModel(Service));
		}

		

		protected override async Task<IEnumerable<DayOfWeek>> OnLoadModelAsync(IePlanifServiceClient Client)
		{

			return await Task.FromResult(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>() );
			
		}

		
	}
}
