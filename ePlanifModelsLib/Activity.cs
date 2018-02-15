using DatabaseModelLib;
using ModelLib;
using System;
using System.Runtime.Serialization;


namespace ePlanifModelsLib
{
	[DataContract]
	public class Activity : BaseModel
	{

		public static readonly Column<Activity, int> ActivityIDColumn = new Column<Activity, int>() { IsPrimaryKey = true, IsIdentity = true };
		[DataMember]
		public int? ActivityID
		{
			get { return ActivityIDColumn.GetValue(this); }
			set { ActivityIDColumn.SetValue(this, value); }
		}



		public static readonly Column<Activity, DateTime> StartDateColumn = new Column<Activity, DateTime>();
		[DataMember]
		public DateTime? StartDate
		{
			get { return StartDateColumn.GetValue(this); }
			set { StartDateColumn.SetValue(this, value); }
		}


		public static readonly Column<Activity, TimeSpan> DurationColumn = new Column<Activity, TimeSpan>();
		[DataMember]
		public TimeSpan? Duration
		{
			get { return DurationColumn.GetValue(this); }
			set { DurationColumn.SetValue(this, value); }
		}

		/*[Revision(2)]
		public static readonly Column<Activity, TimeSpan> TravelDurationColumn = new Column<Activity, TimeSpan>() {IsNullable=true };
		[DataMember]
		public TimeSpan? TravelDuration
		{
			get { return TravelDurationColumn.GetValue(this); }
			set { TravelDurationColumn.SetValue(this, value); }
		}*/


		public static readonly Column<Activity, TimeSpan> TrackedDurationColumn = new Column<Activity, TimeSpan>() {IsNullable=true };
		[DataMember]
		public TimeSpan? TrackedDuration
		{
			get { return TrackedDurationColumn.GetValue(this); }
			set { TrackedDurationColumn.SetValue(this, value); }
		}


		public static readonly Column<Activity, Text> CommentColumn = new Column<Activity, Text>() {IsNullable=true };
		[DataMember]
		public Text? Comment
        {
            get { return CommentColumn.GetValue(this); }
            set { CommentColumn.SetValue(this, value); }
        }



		public static readonly Column<Activity, int> ActivityTypeIDColumn = new Column<Activity, int>();
		[DataMember]
		public int? ActivityTypeID
		{
			get { return ActivityTypeIDColumn.GetValue(this); }
			set { ActivityTypeIDColumn.SetValue(this, value); }
		}



		public static readonly Column<Activity, int> EmployeeIDColumn = new Column<Activity, int>();
		[DataMember]
		public int? EmployeeID
		{
			get { return EmployeeIDColumn.GetValue(this); }
			set { EmployeeIDColumn.SetValue(this, value); }
		}


		
		public static readonly Column<Activity, int> ProjectNumberColumn = new Column<Activity, int>() { IsNullable=true };
		[DataMember]
		public int? ProjectNumber
		{
			get { return ProjectNumberColumn.GetValue(this); }
			set { ProjectNumberColumn.SetValue(this, value); }
		}


		
		public static readonly Column<Activity, Text> RemedyRefColumn = new Column<Activity, Text>() { IsNullable = false, DefaultValue="Remedy"};
		[DataMember]
		public Text? RemedyRef
		{
			get { return RemedyRefColumn.GetValue(this); }
			set { RemedyRefColumn.SetValue(this, value); }
		}

		
		public static readonly Column<Activity, bool> IsDraftColumn = new Column<Activity, bool>() { IsNullable = false, DefaultValue = false };
		[DataMember]
		public bool? IsDraft
		{
			get { return IsDraftColumn.GetValue(this); }
			set { IsDraftColumn.SetValue(this, value); }
		}

		/*
		public static readonly Column<Activity, bool> IsAllDayColumn = new Column<Activity, bool>() { DefaultValue=false };
		public bool? IsAllDay
		{
			get { return IsAllDayColumn.GetValue(this); }
			set { IsAllDayColumn.SetValue(this, value); }
		}*/








	}
}
