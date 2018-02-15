using DatabaseModelLib;
using ModelLib;
using System.Runtime.Serialization;

namespace ePlanifModelsLib
{
	[DataContract]
	public class Account:ePlanifModel
	{
		

		public static readonly Column<Account, int> AccountIDColumn = new Column<Account, int>() { IsPrimaryKey = true, IsIdentity = true };
		[DataMember]
		public int? AccountID
		{
			get { return AccountIDColumn.GetValue(this); }
			set { AccountIDColumn.SetValue(this, value); }
		}


		public static readonly Column<Account, Text> LoginColumn = new Column<Account, Text>();
		[DataMember]
		public Text? Login
		{
			get { return LoginColumn.GetValue(this); }
			set { LoginColumn.SetValue(this, value); }
		}

		
		public static readonly Column<Account, int> ProfileIDColumn = new Column<Account, int>() ;
		[DataMember]
		public int? ProfileID
		{
			get { return ProfileIDColumn.GetValue(this); }
			set { ProfileIDColumn.SetValue(this, value); }
		}



		public static readonly Column<Account, int> EmployeeIDColumn = new Column<Account, int>() { IsNullable = true };
		[DataMember]
		public int? EmployeeID
		{
			get { return EmployeeIDColumn.GetValue(this); }
			set { EmployeeIDColumn.SetValue(this, value); }
		}


		public static readonly Column<Account, bool> IsDisabledColumn = new Column<Account, bool>() { DefaultValue = false };
		[DataMember]
		public override bool? IsDisabled
		{
			get { return IsDisabledColumn.GetValue(this); }
			set { IsDisabledColumn.SetValue(this, value); }
		}

	}
}
