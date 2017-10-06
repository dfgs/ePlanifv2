using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using ModelLib;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ViewModelLib;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class OptionViewModel :WCFViewModel<Option>
    {

        public int? OptionID
        {
            get { return Model.OptionID; }
            set { Model.OptionID = value; OnPropertyChanged(); }
        }


		[TextListProperty(Header = "First day of week", IsMandatory = true, IsReadOnly = false, SelectedValuePath ="Model", DisplayMemberPath = "Description", SourcePath = "Service.DaysOfWeek")]
		public DayOfWeek? FirstDayOfWeek
		{
			get { return Model.FirstDayOfWeek; }
			set { Model.FirstDayOfWeek = value; OnPropertyChanged(); }
		}


		[TextListProperty(Header = "Calendar week rule", IsMandatory = true, IsReadOnly = false, SelectedValuePath = "Model", DisplayMemberPath = "Description", SourcePath = "Service.CalendarWeekRules")]
		public CalendarWeekRule? CalendarWeekRule
		{
			get { return Model.CalendarWeekRule; }
			set { Model.CalendarWeekRule = value; OnPropertyChanged(); }
		}


		public OptionViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
			
		}
		
		protected override async Task<Option> OnLoadModelAsync()
        {
			return await Task.FromResult(Model);
		}

		
	}
}
