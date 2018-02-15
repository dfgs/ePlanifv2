using DatabaseModelLib;
using ModelLib;
using System;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
	public class Employee : ePlanifModel
	{
		

		public static readonly Column<Employee, int> EmployeeIDColumn = new Column<Employee, int>() { IsPrimaryKey = true, IsIdentity = true };
		[DataMember]
		public int? EmployeeID
		{
			get { return EmployeeIDColumn.GetValue(this); }
			set { EmployeeIDColumn.SetValue(this, value); }
		}

		public static readonly Column<Employee, Text> FirstNameColumn = new Column<Employee, Text>();
		[DataMember]
		public Text? FirstName
		{
			get { return FirstNameColumn.GetValue(this); }
			set { FirstNameColumn.SetValue(this, value); }
		}


		public static readonly Column<Employee, Text> LastNameColumn = new Column<Employee, Text>();
		[DataMember]
		public Text? LastName
		{
			get { return LastNameColumn.GetValue(this); }
			set { LastNameColumn.SetValue(this, value); }
		}

		[Revision(2)]
		public static readonly Column<Employee, Text> eMailColumn = new Column<Employee, Text>() { IsNullable = true };
		[DataMember]
		public Text? eMail
		{
			get { return eMailColumn.GetValue(this); }
			set { eMailColumn.SetValue(this, value); }
		}

		public static readonly Column<Employee, bool> IsDisabledColumn = new Column<Employee, bool>() { DefaultValue = false };
		[DataMember]
		public override bool? IsDisabled
		{
			get { return IsDisabledColumn.GetValue(this); }
			set { IsDisabledColumn.SetValue(this, value); }
		}


		public static readonly Column<Employee, bool> WriteAccessColumn = new Column<Employee, bool>() {IsVirtual=true };
		[DataMember]
		public bool? WriteAccess
		{
			get { return WriteAccessColumn.GetValue(this); }
			set { WriteAccessColumn.SetValue(this, value); }
		}

		public static readonly Column<Employee, int> WorkingTimePerWeekColumn = new Column<Employee, int>() { IsNullable = true };
		[DataMember]
		public int? WorkingTimePerWeek
		{
			get { return WorkingTimePerWeekColumn.GetValue(this); }
			set { WorkingTimePerWeekColumn.SetValue(this, value); }
		}

		public static readonly Column<Employee, int> MaxWorkingTimePerWeekColumn = new Column<Employee, int>() { IsNullable = true };
		[DataMember]
		public int? MaxWorkingTimePerWeek
		{
			get { return MaxWorkingTimePerWeekColumn.GetValue(this); }
			set { MaxWorkingTimePerWeekColumn.SetValue(this, value); }
		}

		[Revision(7)]
		public static readonly Column<Employee, int> MaxWorkingTimePerDayColumn = new Column<Employee, int>() { IsNullable = true };
		[DataMember]
		public int? MaxWorkingTimePerDay
		{
			get { return MaxWorkingTimePerDayColumn.GetValue(this); }
			set { MaxWorkingTimePerDayColumn.SetValue(this, value); }
		}


		public static readonly Column<Employee, Text> CountryCodeColumn = new Column<Employee, Text>();
		[DataMember]
		public Text? CountryCode
		{
			get { return CountryCodeColumn.GetValue(this); }
			set { CountryCodeColumn.SetValue(this, value); }
		}


		[Revision(8)]
		public static readonly Column<Employee, bool> IsRegisteredToMailPlannningColumn = new Column<Employee, bool>() {DefaultValue=true };
		[DataMember]
		public bool? IsRegisteredToMailPlannning
		{
			get { return IsRegisteredToMailPlannningColumn.GetValue(this); }
			set { IsRegisteredToMailPlannningColumn.SetValue(this, value); }
		}

		public Employee()
		{

		}
		public Employee(Employee Model)
		{
			
			
		}





	}
}
