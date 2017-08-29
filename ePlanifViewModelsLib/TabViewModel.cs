using DatabaseModelLib;
using ePlanifModelsLib;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
    public abstract class TabViewModel : WCFViewModel<object>
    {
		public abstract string ImageSource
		{
			get;
		}

		public TabViewModel(ePlanifServiceViewModel Service) : base(Service)
		{
			
		}

		protected override Task<object> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
		}

		

		



	}
}
