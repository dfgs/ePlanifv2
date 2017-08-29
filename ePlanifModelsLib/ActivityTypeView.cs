using DatabaseModelLib;
using ModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
    public class ActivityTypeView
    {

        public static readonly Column<ActivityTypeView, int> ActivityTypeViewIDColumn = new Column<ActivityTypeView, int>() {IsPrimaryKey=true,IsIdentity=true };
		[DataMember]
		public int? ActivityTypeViewID
        {
            get { return ActivityTypeViewIDColumn.GetValue(this); }
            set { ActivityTypeViewIDColumn.SetValue(this, value); }
        }


		public static readonly Column<ActivityTypeView, int> AccountIDColumn = new Column<ActivityTypeView, int>();
		[DataMember]
		public int? AccountID
		{
			get { return AccountIDColumn.GetValue(this); }
			set { AccountIDColumn.SetValue(this, value); }
		}



		public static readonly Column<ActivityTypeView, Text> NameColumn = new Column<ActivityTypeView, Text>();
		[DataMember]
		public Text? Name
		{
			get { return NameColumn.GetValue(this); }
			set { NameColumn.SetValue(this, value); }
		}




	}
}
