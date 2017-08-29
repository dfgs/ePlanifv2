using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifModelsLib
{
	[DataContract]
	public abstract class ePlanifModel
	{

		[DataMember]
		public abstract bool? IsDisabled
		{
			get;
			set;
		}
	}
}
