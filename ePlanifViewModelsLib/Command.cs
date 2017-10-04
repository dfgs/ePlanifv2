using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public abstract class Command :IDisposable
	{
		public virtual string Description
		{
			get { return GetType().Name; }
		}



		public virtual void Dispose()
		{

		}
		public abstract Task<bool> ExecuteAsync();
		public abstract Task<bool> CancelAsync();

		

		


		public override string ToString()
		{
			return Description;
		}



	}
}
