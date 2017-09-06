using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerLib
{
	public class ePlanifServiceHost: ServiceHost
	{

		private static Uri uri = new Uri("net.tcp://localhost:8523/ePlanif");

		public ePlanifServiceHost(IDataProvider DataProvider) :base(new ePlanifService(DataProvider),uri)
		{
			Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = false });

			Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			Authorization.ExternalAuthorizationPolicies = new IAuthorizationPolicy[] { new ePlanifPolicy(DataProvider) }.ToList().AsReadOnly();

			NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

			AddServiceEndpoint(typeof(IePlanifService), binding, uri);
			AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "net.tcp://localhost:8523/ePlanif/mex/");
		}


	}
}
