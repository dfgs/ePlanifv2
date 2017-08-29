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
    public class TabByActivityTypeViewModel : TabViewModel
    {
		public override string ImageSource
		{
			get { return "Images/categories.png"; }
		}
		public string Header
		{
			get { return "Views by activity types"; }
		}

		public TabByActivityTypeViewModel(ePlanifServiceViewModel Service) : base(Service)
		{
			
		}

		

		

		



	}
}
