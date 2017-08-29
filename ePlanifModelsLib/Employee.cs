using DatabaseModelLib;
using ModelLib;
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

		public static readonly Column<Employee, bool> IsDisabledColumn = new Column<Employee, bool>() { DefaultValue = false };
		[DataMember]
		public override bool? IsDisabled
		{
			get { return IsDisabledColumn.GetValue(this); }
			set { IsDisabledColumn.SetValue(this, value); }
		}


		
		public static readonly Column<Employee, byte> MaxWorkingHoursPerWeekColumn = new Column<Employee, byte>() { IsNullable = true };
		[DataMember]
		public byte? MaxWorkingHoursPerWeek
		{
			get { return MaxWorkingHoursPerWeekColumn.GetValue(this); }
			set { MaxWorkingHoursPerWeekColumn.SetValue(this, value); }
		}





	}
}
