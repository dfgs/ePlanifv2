using DatabaseModelLib;
using ModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
    public class Group
    {

        public static readonly Column<Group, int> GroupIDColumn = new Column<Group, int>() {IsPrimaryKey=true,IsIdentity=true };
		[DataMember]
		public int? GroupID
        {
            get { return GroupIDColumn.GetValue(this); }
            set { GroupIDColumn.SetValue(this, value); }
        }


		public static readonly Column<Group, int> ParentGroupIDColumn = new Column<Group, int>() {IsNullable=true };
		[DataMember]
		public int? ParentGroupID
		{
			get { return ParentGroupIDColumn.GetValue(this); }
			set { ParentGroupIDColumn.SetValue(this, value); }
		}



		public static readonly Column<Group, Text> NameColumn = new Column<Group, Text>();
		[DataMember]
		public Text? Name
		{
			get { return NameColumn.GetValue(this); }
			set { NameColumn.SetValue(this, value); }
		}








	}
}
