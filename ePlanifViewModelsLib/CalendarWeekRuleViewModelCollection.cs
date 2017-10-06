using ePlanifViewModelsLib.ePlanifService;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class CalendarWeekRuleViewModelCollection:WCFViewModelCollection<CalendarWeekRuleViewModel,CalendarWeekRule>
	{

		public CalendarWeekRuleViewModelCollection(ePlanifServiceViewModel Service)
			:base(Service)
		{
		}

		protected override Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, CalendarWeekRuleViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, CalendarWeekRuleViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, CalendarWeekRuleViewModel ViewModel)
		{
			throw new NotImplementedException();
		}



		protected override Task<CalendarWeekRule> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(CalendarWeekRule.FirstDay);
		}

		protected override Task<CalendarWeekRuleViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new CalendarWeekRuleViewModel(Service));
		}

		

		protected override async Task<IEnumerable<CalendarWeekRule>> OnLoadModelAsync(IePlanifServiceClient Client)
		{

			return await Task.FromResult(Enum.GetValues(typeof(CalendarWeekRule)).Cast<CalendarWeekRule>() );
			
		}

		
	}
}
