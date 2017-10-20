using DatabaseModelLib;
using ModelLib;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace ePlanifModelsLib
{
	[DataContract]
	public class Option
	{


		public static readonly Column<Option, int> OptionIDColumn = new Column<Option, int>() { IsPrimaryKey = true, IsIdentity = true };
		[DataMember]
		public int? OptionID
		{
			get { return OptionIDColumn.GetValue(this); }
			set { OptionIDColumn.SetValue(this, value); }
		}


		public static readonly Column<Option, int> AccountIDColumn = new Column<Option, int>() ;
		[DataMember]
		public int? AccountID
		{
			get { return AccountIDColumn.GetValue(this); }
			set { AccountIDColumn.SetValue(this, value); }
		}



		public static readonly Column<Option, DayOfWeek> FirstDayOfWeekColumn = new Column<Option, DayOfWeek>();
		[DataMember]
		public DayOfWeek? FirstDayOfWeek
		{
			get { return FirstDayOfWeekColumn.GetValue(this); }
			set { FirstDayOfWeekColumn.SetValue(this, value); }
		}


		public static readonly Column<Option, CalendarWeekRule> CalendarWeekRuleColumn = new Column<Option, CalendarWeekRule>();
		[DataMember]
		public CalendarWeekRule? CalendarWeekRule
		{
			get { return CalendarWeekRuleColumn.GetValue(this); }
			set { CalendarWeekRuleColumn.SetValue(this, value); }
		}

		[Revision(5)]
		public static readonly Column<Option, bool> DisplayPhotosColumn = new Column<Option, bool>() {DefaultValue=true };
		[DataMember]
		public bool? DisplayPhotos
		{
			get { return DisplayPhotosColumn.GetValue(this); }
			set { DisplayPhotosColumn.SetValue(this, value); }
		}





		// first day of week
		// calendar week rule



	}
}
