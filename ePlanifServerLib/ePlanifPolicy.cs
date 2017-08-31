using DatabaseModelLib.Filters;
using ePlanifModelsLib;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerLib
{
	class ePlanifPolicy : IAuthorizationPolicy
	{
		private string id;
		public string Id
		{
			get { return id; }
		}

		public ClaimSet Issuer
		{
			get { return ClaimSet.System; }
		}

		// this method gets called after the authentication stage
		public bool Evaluate(EvaluationContext evaluationContext, ref object state)
		{
			Account account;
			Profile profile;
			ePlanifDatabase db;

			id = Guid.NewGuid().ToString();

			// will hold the combined roles
			List<string> roles = new List<string>();
			account = null;
			profile = null;

			// get the authenticated client identity
			IIdentity client = GetClientIdentity(evaluationContext);
			//if (client == null) return false;

			// this policy is intended for Windows accounts only
			WindowsIdentity windowsClient = client as WindowsIdentity;
			if (windowsClient != null)
			{
				// add Windows groups
				//roles.AddRange(GetWindowsRoles(windowsClient)); // keep in case....

				// add application defined roles
				db = new ePlanifDatabase("localhost");
				try
				{
					account = db.SelectAsync<Account>(new EqualFilter<Account>(Account.LoginColumn, windowsClient.Name.ToLower())).Result.FirstOrDefault();
					if (account != null) profile = db.SelectAsync<Profile>(new EqualFilter<Profile>(Profile.ProfileIDColumn, account.ProfileID)).Result.FirstOrDefault();
				}
				catch
				{
					// pb occured, so no role available
				}
			}

			roles.AddRange(GetAppRoles(account, profile));
			// set a new principal holding the combined roles
			// this could be your own IPrincipal implementation
			evaluationContext.Properties["Principal"] = new ePlanifPrincipal(windowsClient, account, profile, roles.ToArray());


			return true;
		}


		private IIdentity GetClientIdentity(EvaluationContext evaluationContext)
		{
			object obj;
			if (!evaluationContext.Properties.TryGetValue("Identities", out obj)) return null;
			//throw new Exception("No Identity found");

			IList<IIdentity> identities = obj as IList<IIdentity>;
			if (identities == null || identities.Count <= 0) return null;
			//throw new Exception("No Identity found");

			return identities[0];
		}

		private IEnumerable<string> GetWindowsRoles(WindowsIdentity windowsClient)
		{
			List<string> roles = new List<string>();

			IdentityReferenceCollection groups =
			  windowsClient.Groups.Translate(typeof(NTAccount));

			foreach (IdentityReference group in groups)
			{
				roles.Add(group.Value);
			}

			return roles;
		}

		private IEnumerable<string> GetAppRoles(Account Account, Profile Profile)
		{
			if ((Account == null) || (Profile == null)) yield break;
			if (Profile.IsDisabled.Value) yield break;
			yield return Roles.ePlanifUser;
			if (Profile.AdministrateEmployees.Value) yield return Roles.AdministrateEmployees;
			if (Profile.AdministrateAccounts.Value) yield return Roles.AdministrateAccounts;
			if (Profile.AdministrateActivityTypes.Value) yield return Roles.AdministrateActivityTypes;
			if (Profile.CanRunReports.Value) yield return Roles.CanRunReports;
			if (Profile.AdministrateEmployees.Value) yield return Roles.AdministrateEmployees;
		}



	}
}
