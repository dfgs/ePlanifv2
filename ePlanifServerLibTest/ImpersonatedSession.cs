using ePlanifServerLibTest.ePlanifService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerLibTest
{
	public  class ImpersonatedSession:IDisposable
	{
		[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private extern static bool CloseHandle(IntPtr handle);

		private const int LOGON32_PROVIDER_DEFAULT = 0;
		//This parameter causes LogonUser to create a primary token.
		private const int LOGON32_LOGON_INTERACTIVE = 2;

		private SafeTokenHandle safeTokenHandle;
		private WindowsIdentity identity;
		private WindowsImpersonationContext impersonatedUser;

		private IePlanifServiceClient client;
		public IePlanifServiceClient Client
		{
			get { return client; }
		}


		[PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
		public ImpersonatedSession(string Domain, string Login, string Password)
		{
			//bool result;

			//result = LogonUser(Login, Domain, Password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out safeTokenHandle);
			//if (!result) throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

			//identity = new WindowsIdentity(safeTokenHandle.DangerousGetHandle());

			//impersonatedUser = identity.Impersonate();
			// Check the identity.
			//Console.WriteLine("After impersonation: "+ WindowsIdentity.GetCurrent().Name);

			client = new IePlanifServiceClient();

			client.ClientCredentials.UserName.UserName = Login;
			client.ClientCredentials.UserName.Password = Password;

			client.Open();
		}

		[PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
		public void Dispose()
		{
			if (client != null) client.Close();
			// Releasing the context object stops the impersonation
			if (impersonatedUser!=null) impersonatedUser.Dispose();
			if (identity != null) identity.Dispose();
			if (safeTokenHandle != null) safeTokenHandle.Close();

			impersonatedUser = null;
			identity = null;
			safeTokenHandle = null;
		}





	}
}