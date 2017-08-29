using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public static class Clipboard
	{
		private static bool isCutting;
		public static bool IsCutting
		{
			get { return isCutting; }
		}

		private static List<ActivityViewModel> items;
		public static IEnumerable<ActivityViewModel> Items
		{
			get { return items; }
		}

		public static bool CanPaste
		{
			get { return items != null; }
		}

		public static void Copy(IEnumerable<ActivityViewModel> Items)
		{
			items = new List<ActivityViewModel>(Items);
			isCutting = false;
		}
		public static void Cut(IEnumerable<ActivityViewModel> Items)
		{
			items = new List<ActivityViewModel>(Items);
			isCutting = true;
		}
		public static void Clear()
		{
			items = null;
			isCutting = false;
		}
	}
}
