using ModelLib;
using Nager.Date;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class CalendarWeekRuleViewModel : WCFViewModel<CalendarWeekRule>
	{
		private string description;
		public string Description
		{
			get { return description; }
		}
		

		public CalendarWeekRuleViewModel(ePlanifServiceViewModel Service)
			: base(Service)
		{
			
		}

		protected override Task<CalendarWeekRule> OnLoadModelAsync()
		{
			return Task.FromResult(Model);
		}
		protected override async Task OnLoadedAsync()
		{
			description = Enum.GetName(typeof(CalendarWeekRule), Model);
			await base.OnLoadedAsync();
		}


	}
}
