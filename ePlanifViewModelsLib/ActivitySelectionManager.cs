using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifViewModelsLib
{
	public delegate void ActivitySelectedEventHandler(DependencyObject sender, ActivityViewModel Activity);

	public static class ActivitySelectionManager
	{
		public static event ActivitySelectedEventHandler ActivityUnselected;
		public static event ActivitySelectedEventHandler ActivitySelected;

		public static void OnActivitySelected(DependencyObject sender, ActivityViewModel Activity)
		{
			if (ActivitySelected != null) ActivitySelected(sender, Activity);
		}
		public static void OnActivityUnselected(DependencyObject sender, ActivityViewModel Activity)
		{
			if (ActivityUnselected != null) ActivityUnselected(sender, Activity);
		}

	}



}
