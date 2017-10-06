using DatabaseModelLib;
using ModelLib;
using System.Runtime.Serialization;

namespace ePlanifModelsLib
{
	[DataContract]
	public class Photo
	{


		public static readonly Column<Photo, int> PhotoIDColumn = new Column<Photo, int>() { IsPrimaryKey = true, IsIdentity = true };
		[DataMember]
		public int? PhotoID
		{
			get { return PhotoIDColumn.GetValue(this); }
			set { PhotoIDColumn.SetValue(this, value); }
		}
				
		public static readonly Column<Photo, int> EmployeeIDColumn = new Column<Photo, int>() ;
		[DataMember]
		public int? EmployeeID
		{
			get { return EmployeeIDColumn.GetValue(this); }
			set { EmployeeIDColumn.SetValue(this, value); }
		}


		public static readonly BlobColumn<Photo> DataColumn = new BlobColumn<Photo>();
		[DataMember]
		public byte[] Data
		{
			get { return DataColumn.GetValue(this); }
			set { DataColumn.SetValue(this, value); }
		}






	}
}
