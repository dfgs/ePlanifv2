using Nager.Date;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class DayViewModel : WCFViewModel<DateTime>
	{

		public string ShortDate
		{
			get { return Model.ToShortDateString(); }
		}
		public string DayOfMonth
		{
			get { return Model.Day.ToString("D2"); }
		}
		public string DayOfWeek
		{
			get { return CultureInfo.InvariantCulture.DateTimeFormat.GetDayName(Model.DayOfWeek); }
		}
		public string MonthWithYear
		{
			get { return CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Model.Month)+" "+Model.Year.ToString(); }
		}
		public DateTime Date
		{
			get { return Model; }
		}


		public DayViewModel(ePlanifServiceViewModel Service)
			: base(Service)
		{
			
		}

		protected override Task<DateTime> OnLoadModelAsync()
		{
			return Task.FromResult(Model);
		}

		

	}
}
