using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public interface IGroupViewModel
	{
		Task<bool> AddMemberAsync(object Member);
	}
}
