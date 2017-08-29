using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class ReportViewModel : WCFViewModel<string>
	{

		public ReportViewModel(ePlanifServiceViewModel Service):base(Service)
		{

		}

		protected override Task<string> OnLoadModelAsync()
		{
			return Task.FromResult(Model);
		}

		public override string ToString()
		{
			return Model;
		}
	}
}
