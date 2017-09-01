using ModelLib;
using Nager.Date;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class CountryCodeViewModel : WCFViewModel<string>
	{
		// Images/Flags/af.png
		public string ImageSource
		{
			get { return $"Images/Flags/{Model}.png"; }
		}
		

		public CountryCodeViewModel(ePlanifServiceViewModel Service)
			: base(Service)
		{
			
		}

		protected override Task<string> OnLoadModelAsync()
		{
			return Task.FromResult(Model);
		}

		

	}
}
