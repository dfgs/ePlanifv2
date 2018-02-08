using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public interface IViewViewModel
	{
		event CellEventHandler Updated;
		event CellEventHandler CellFocused;
		event CellEventHandler CellSelectionChanged;
		event CellEventHandler ActivitySelectionChanged;
		//event EventHandler LayerIDChanged;

		bool HasActivitySelected
		{
			get;
		}

		//IEnumerable<int> GetSelectedCellRows();
		//IEnumerable<int> GetSelectedActivitiesRows();

		Task<bool> ReplicateActivitiesAsync(DateTime EndDate,bool SkipPublicHolidays);
		CellViewModel GetCellContent(int Col, int Row);
		void Select(CellViewModel Cell);
		void UnSelectCells();
		void SelectActivity(CellViewModel Cell, int ActivityIndex);
		void UnSelectActivities();
		Task Edit();
		Task Add(CellViewModel Cell);
		ActivityViewModel SearchProject(string Reference, ActivityViewModel CurrentActivity);
		void Focus(ActivityViewModel Activity);

		void ExportToExcel(string FileName);
	}
}