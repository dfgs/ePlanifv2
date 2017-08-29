using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public interface IViewViewModel
	{
		event EventHandler Updated;
		event CellFocusedEventHandler CellFocused;

		bool HasActivitySelected
		{
			get;
		}

		Task ReplicateActivitiesAsync(DateTime EndDate);

		CellViewModel GetCellContent(int Col, int Row);
		//DayViewModel GetColumnContent(int Index);
		//IRowViewModel GetRowContent(int Index);
		//object GetCornerContent();
		void Select(CellViewModel Cell);
		void UnSelectCells();
		void SelectActivity(CellViewModel Cell, int ActivityIndex);
		void UnSelectActivities();
		Task Edit();
		Task Add(CellViewModel Cell);
		ActivityViewModel SearchProject(string Reference, ActivityViewModel CurrentActivity);
		void Focus(ActivityViewModel Activity);
	}
}