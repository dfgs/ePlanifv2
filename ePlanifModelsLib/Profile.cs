using DatabaseModelLib;
using ModelLib;
using System.Runtime.Serialization;

namespace ePlanifModelsLib
{
	[DataContract]
	public class Profile:ePlanifModel
	{


		public static readonly Column<Profile, int> ProfileIDColumn = new Column<Profile, int>() { IsPrimaryKey = true, IsIdentity = true };
		[DataMember]
		public int? ProfileID
		{
			get { return ProfileIDColumn.GetValue(this); }
			set { ProfileIDColumn.SetValue(this, value); }
		}


		public static readonly Column<Profile, Text> NameColumn = new Column<Profile, Text>();
		[DataMember]
		public Text? Name
		{
			get { return NameColumn.GetValue(this); }
			set { NameColumn.SetValue(this, value); }
		}


		
		public static readonly Column<Profile, bool> AdministrateEmployeesColumn = new Column<Profile, bool>() { DefaultValue = false };
		[DataMember]
		public bool? AdministrateEmployees
		{
			get { return AdministrateEmployeesColumn.GetValue(this); }
			set { AdministrateEmployeesColumn.SetValue(this, value); }
		}

		
		public static readonly Column<Profile, bool> AdministrateActivityTypesColumn = new Column<Profile, bool>() { DefaultValue = false };
		[DataMember]
		public bool? AdministrateActivityTypes
		{
			get { return AdministrateActivityTypesColumn.GetValue(this); }
			set { AdministrateActivityTypesColumn.SetValue(this, value); }
		}

		
		public static readonly Column<Profile, bool> CanRunReportsColumn = new Column<Profile, bool>() { DefaultValue = false };
		[DataMember]
		public bool? CanRunReports
		{
			get { return CanRunReportsColumn.GetValue(this); }
			set { CanRunReportsColumn.SetValue(this, value); }
		}

		
		public static readonly Column<Profile, bool> AdministrateAccountsColumn = new Column<Profile, bool>() { DefaultValue = false };
		[DataMember]
		public bool? AdministrateAccounts
		{
			get { return AdministrateAccountsColumn.GetValue(this); }
			set { AdministrateAccountsColumn.SetValue(this, value); }
		}

		public static readonly Column<Profile, bool> SelfWriteAccessColumn = new Column<Profile, bool>() { DefaultValue = true };
		[DataMember]
		public bool? SelfWriteAccess
		{
			get { return SelfWriteAccessColumn.GetValue(this); }
			set { SelfWriteAccessColumn.SetValue(this, value); }
		}


		public static readonly Column<Profile, bool> IsDisabledColumn = new Column<Profile, bool>() { DefaultValue = false };
		[DataMember]
		public override bool? IsDisabled
		{
			get { return IsDisabledColumn.GetValue(this); }
			set { IsDisabledColumn.SetValue(this, value); }
		}



	}
}
