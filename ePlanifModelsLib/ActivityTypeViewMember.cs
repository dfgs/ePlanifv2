using DatabaseModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
    public class ActivityTypeViewMember
    {

        public static readonly Column<ActivityTypeViewMember, int> ActivityTypeViewMemberIDColumn = new Column<ActivityTypeViewMember, int>() {IsPrimaryKey=true,IsIdentity=true };
		[DataMember]
		public int? ActivityTypeViewMemberID
        {
            get { return ActivityTypeViewMemberIDColumn.GetValue(this); }
            set { ActivityTypeViewMemberIDColumn.SetValue(this, value); }
        }


		public static readonly Column<ActivityTypeViewMember, int> ActivityTypeViewIDColumn = new Column<ActivityTypeViewMember, int>();
		[DataMember]
		public int? ActivityTypeViewID
		{
			get { return ActivityTypeViewIDColumn.GetValue(this); }
			set { ActivityTypeViewIDColumn.SetValue(this, value); }
		}

		public static readonly Column<ActivityTypeViewMember, int> ActivityTypeIDColumn = new Column<ActivityTypeViewMember, int>();
		[DataMember]
		public int? ActivityTypeID
		{
			get { return ActivityTypeIDColumn.GetValue(this); }
			set { ActivityTypeIDColumn.SetValue(this, value); }
		}





	}
}
