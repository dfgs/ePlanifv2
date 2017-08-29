using ePlanifModelsLib;
using System;
using System.Security.Principal;

namespace ePlanifServerLib
{
	class ePlanifPrincipal: IPrincipal
	{
		private string[] roles;
		public string[] Roles
		{
			get { return roles; }
		}

		private IIdentity identity;
		public IIdentity Identity
		{
			get { return identity; }
		}

		private Account account;
		public Account Account
		{
			get { return account; }
		}

		private Profile profile;
		public Profile Profile
		{
			get { return profile; }
		}
		
		public ePlanifPrincipal(IIdentity Identity,Account Account, Profile Profile,params string[] Roles)
		{
			this.identity = Identity;this.account = Account; this.profile = Profile; this.roles = Roles;
		}


		public bool IsInRole(string role)
		{
			if (Roles == null) return false;
			foreach(string item in Roles)
			{
				if (item == role) return true;
			}
			return false;
		}


	}
}