using DatabaseModelLib;
using ModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
    public class EmployeeView
    {

        public static readonly Column<EmployeeView, int> EmployeeViewIDColumn = new Column<EmployeeView, int>() {IsPrimaryKey=true,IsIdentity=true };
		[DataMember]
		public int? EmployeeViewID
        {
            get { return EmployeeViewIDColumn.GetValue(this); }
            set { EmployeeViewIDColumn.SetValue(this, value); }
        }


		public static readonly Column<EmployeeView, int> AccountIDColumn = new Column<EmployeeView, int>();
		[DataMember]
		public int? AccountID
		{
			get { return AccountIDColumn.GetValue(this); }
			set { AccountIDColumn.SetValue(this, value); }
		}



		public static readonly Column<EmployeeView, Text> NameColumn = new Column<EmployeeView, Text>();
		[DataMember]
		public Text? Name
		{
			get { return NameColumn.GetValue(this); }
			set { NameColumn.SetValue(this, value); }
		}




	}
}
