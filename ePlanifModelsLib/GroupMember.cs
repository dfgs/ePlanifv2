using DatabaseModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
	public class GroupMember : BaseModel
	{

		public static readonly Column<GroupMember, int> GroupMemberIDColumn = new Column<GroupMember, int>() { IsPrimaryKey = true, IsIdentity = true };
		[DataMember]
		public int? GroupMemberID
		{
			get { return GroupMemberIDColumn.GetValue(this); }
			set { GroupMemberIDColumn.SetValue(this, value); }
		}


		public static readonly Column<GroupMember, int> GroupIDColumn = new Column<GroupMember, int>();
		[DataMember]
		public int? GroupID
		{
			get { return GroupIDColumn.GetValue(this); }
			set { GroupIDColumn.SetValue(this, value); }
		}


		public static readonly Column<GroupMember, int> EmployeeIDColumn = new Column<GroupMember, int>();
		[DataMember]
		public int? EmployeeID
		{
			get { return EmployeeIDColumn.GetValue(this); }
			set { EmployeeIDColumn.SetValue(this, value); }
		}






	}
}
