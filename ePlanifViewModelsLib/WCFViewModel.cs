using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public abstract class WCFViewModel<ModelType> : ViewModel<ModelType>
	{
		private ePlanifServiceViewModel service;
		public ePlanifServiceViewModel Service
		{
			get { return service; }
		}

		public WCFViewModel(ePlanifServiceViewModel Service)
		{
			this.service = Service;
		}


	}
}
