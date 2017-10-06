using ModelLib;
using Nager.Date;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class DayOfWeekViewModel : WCFViewModel<DayOfWeek>
	{
		private string description;
		public string Description
		{
			get { return description; }
		}
		

		public DayOfWeekViewModel(ePlanifServiceViewModel Service)
			: base(Service)
		{
			
		}

		protected override Task<DayOfWeek> OnLoadModelAsync()
		{
			return Task.FromResult(Model);
		}
		protected override async Task OnLoadedAsync()
		{
			description = Enum.GetName(typeof(DayOfWeek), Model);
			await base.OnLoadedAsync();
		}


	}
}
