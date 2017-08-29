using DatabaseModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
    public class Grant
    {

        public static readonly Column<Grant, int> GrantIDColumn = new Column<Grant, int>() {IsPrimaryKey=true,IsIdentity=true };
		[DataMember]
		public int? GrantID
        {
            get { return GrantIDColumn.GetValue(this); }
            set { GrantIDColumn.SetValue(this, value); }
        }


		public static readonly Column<Grant, int> ProfileIDColumn = new Column<Grant, int>();
		[DataMember]
		public int? ProfileID
		{
			get { return ProfileIDColumn.GetValue(this); }
			set { ProfileIDColumn.SetValue(this, value); }
		}


		public static readonly Column<Grant, int> GroupIDColumn = new Column<Grant, int>();
		[DataMember]
		public int? GroupID
		{
			get { return GroupIDColumn.GetValue(this); }
			set { GroupIDColumn.SetValue(this, value); }
		}






	}
}
