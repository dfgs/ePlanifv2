using DatabaseModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
    public class EmployeeViewMember
    {

        public static readonly Column<EmployeeViewMember, int> EmployeeViewMemberIDColumn = new Column<EmployeeViewMember, int>() {IsPrimaryKey=true,IsIdentity=true };
		[DataMember]
		public int? EmployeeViewMemberID
        {
            get { return EmployeeViewMemberIDColumn.GetValue(this); }
            set { EmployeeViewMemberIDColumn.SetValue(this, value); }
        }


		public static readonly Column<EmployeeViewMember, int> EmployeeViewIDColumn = new Column<EmployeeViewMember, int>();
		[DataMember]
		public int? EmployeeViewID
		{
			get { return EmployeeViewIDColumn.GetValue(this); }
			set { EmployeeViewIDColumn.SetValue(this, value); }
		}

		public static readonly Column<EmployeeViewMember, int> EmployeeIDColumn = new Column<EmployeeViewMember, int>();
		[DataMember]
		public int? EmployeeID
		{
			get { return EmployeeIDColumn.GetValue(this); }
			set { EmployeeIDColumn.SetValue(this, value); }
		}





	}
}
