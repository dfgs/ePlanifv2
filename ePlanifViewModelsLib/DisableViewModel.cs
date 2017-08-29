using ePlanifModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public abstract class DisableViewModel<ModelType>:WCFViewModel<ModelType>
		where ModelType:ePlanifModel
	{

		public bool? IsDisabled
		{
			get { return Model.IsDisabled; }
			set { Model.IsDisabled = value; OnPropertyChanged(); }
		}

		public DisableViewModel(ePlanifServiceViewModel Service) : base(Service)
		{
		}

		
	}

}
