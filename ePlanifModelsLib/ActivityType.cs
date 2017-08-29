using DatabaseModelLib;
using ModelLib;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
    public class ActivityType:ePlanifModel
    {

        public static readonly Column<ActivityType, int> ActivityTypeIDColumn = new Column<ActivityType, int>() {IsPrimaryKey=true,IsIdentity=true };
		[DataMember]
		public int? ActivityTypeID
        {
            get { return ActivityTypeIDColumn.GetValue(this); }
            set { ActivityTypeIDColumn.SetValue(this, value); }
        }
		
        public static readonly Column<ActivityType, Text> NameColumn = new Column<ActivityType, Text>() ;
		[DataMember]
		public Text? Name
        {
            get { return NameColumn.GetValue(this); }
            set { NameColumn.SetValue(this, value); }
        }


		public static readonly Column<ActivityType, Text> BackgroundColorColumn = new Column<ActivityType, Text>() { DefaultValue = "LightGreen" };
		[DataMember]
		public Text? BackgroundColor
		{
			get { return BackgroundColorColumn.GetValue(this); }
			set { BackgroundColorColumn.SetValue(this, value); }
		}

		
		public static readonly Column<ActivityType, Text> TextColorColumn = new Column<ActivityType, Text>() { DefaultValue = "Black" };
		[DataMember]
		public Text? TextColor
		{
			get { return TextColorColumn.GetValue(this); }
			set { TextColorColumn.SetValue(this, value); }
		}

		
		public static readonly Column<ActivityType, int> LayerIDColumn = new Column<ActivityType, int>() {DefaultValue=1 };
		[DataMember]
		public int? LayerID
		{
			get { return LayerIDColumn.GetValue(this); }
			set { LayerIDColumn.SetValue(this, value); }
		}



		public static readonly Column<ActivityType, bool> IsDisabledColumn = new Column<ActivityType, bool>() { DefaultValue = false };
		[DataMember]
		public override bool? IsDisabled
		{
			get { return IsDisabledColumn.GetValue(this); }
			set { IsDisabledColumn.SetValue(this, value); }
		}

		
		public static readonly Column<ActivityType, int> MinEmployeesColumn = new Column<ActivityType, int>() { IsNullable=true};
		[DataMember]
		public int? MinEmployees
		{
			get { return MinEmployeesColumn.GetValue(this); }
			set { MinEmployeesColumn.SetValue(this, value); }
		}











	}
}
