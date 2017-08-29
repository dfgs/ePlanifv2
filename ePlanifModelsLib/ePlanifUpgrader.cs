using DatabaseModelLib;
using DatabaseModelLib.Filters;
using DatabaseUpgraderLib;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifModelsLib
{
	public class ePlanifUpgrader : DatabaseUpgrader
	{
		public ePlanifUpgrader(IDatabase Database) : base(Database)
		{
		}

		protected override async Task OnUpgradedTo(int Revision)
		{
			await Task.Yield();

		}


	}
}
