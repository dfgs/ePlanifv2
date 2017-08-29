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
    public class TabByEmployeeViewModel : TabViewModel
	{
		public override string ImageSource
		{
			get { return "Images/user.png"; }
		}
		public string Header
		{
			get { return "Views by employees"; }
		}

		public TabByEmployeeViewModel(ePlanifServiceViewModel Service) : base(Service)
		{
			
		}

		
		

		



	}
}
