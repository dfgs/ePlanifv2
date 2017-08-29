using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public interface IRowViewModel
	{
		int RowID
		{
			get;
		}

		bool? IsDisabled
		{
			get;
		}
		bool StartsWith(char Key);
	}
}
