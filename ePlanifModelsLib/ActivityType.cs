using DatabaseModelLib;
using ModelLib;
using System;
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

		[Revision(6)]
		public static readonly Column<ActivityType, DateTime> DefaultStartTimeAMColumn = new Column<ActivityType, DateTime>() { IsNullable=true};
		[DataMember]
		public DateTime? DefaultStartTimeAM
		{
			get { return DefaultStartTimeAMColumn.GetValue(this); }
			set { DefaultStartTimeAMColumn.SetValue(this, value); }
		}
		[Revision(6)]
		public static readonly Column<ActivityType, TimeSpan> DefaultDurationAMColumn = new Column<ActivityType, TimeSpan>() { IsNullable = true };
		[DataMember]
		public TimeSpan? DefaultDurationAM
		{
			get { return DefaultDurationAMColumn.GetValue(this); }
			set { DefaultDurationAMColumn.SetValue(this, value); }
		}

		[Revision(6)]
		public static readonly Column<ActivityType, DateTime> DefaultStartTimePMColumn = new Column<ActivityType, DateTime>() { IsNullable = true };
		[DataMember]
		public DateTime? DefaultStartTimePM
		{
			get { return DefaultStartTimePMColumn.GetValue(this); }
			set { DefaultStartTimePMColumn.SetValue(this, value); }
		}

		[Revision(6)]
		public static readonly Column<ActivityType, TimeSpan> DefaultDurationPMColumn = new Column<ActivityType, TimeSpan>() { IsNullable = true };
		[DataMember]
		public TimeSpan? DefaultDurationPM
		{
			get { return DefaultDurationPMColumn.GetValue(this); }
			set { DefaultDurationPMColumn.SetValue(this, value); }
		}

		[Revision(9)]
		public static readonly Column<ActivityType, TimeSpan> DefaultTrackedDurationColumn = new Column<ActivityType, TimeSpan>() { IsNullable = true };
		[DataMember]
		public TimeSpan? DefaultTrackedDuration
		{
			get { return DefaultTrackedDurationColumn.GetValue(this); }
			set { DefaultTrackedDurationColumn.SetValue(this, value); }
		}





	}
}
