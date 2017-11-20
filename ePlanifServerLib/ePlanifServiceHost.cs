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

		//private static Uri uri = new Uri("net.tcp://localhost:8523/ePlanif");

		public ePlanifServiceHost(IDataProvider DataProvider,int Port=8523) :base(new ePlanifService(DataProvider),new Uri($"net.tcp://localhost:{Port}/ePlanif"))
		{
			Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true, HttpGetUrl= new Uri($"http://localhost:{Port + 1}/ePlanif") });

			Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			Authorization.ExternalAuthorizationPolicies = new IAuthorizationPolicy[] { new ePlanifPolicy(DataProvider) }.ToList().AsReadOnly();

			NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.Transport);
			tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			AddServiceEndpoint(typeof(IePlanifService), tcpBinding, new Uri($"net.tcp://localhost:{Port}/ePlanif"));

			//BasicHttpBinding httpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
			//httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
			//AddServiceEndpoint(typeof(IePlanifService), httpBinding, new Uri($"https://localhost:{Port+1}/ePlanif"));

			AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), $"net.tcp://localhost:{Port}/ePlanif/mex");
			AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), $"http://localhost:{Port+1}/ePlanif/mex");
		}


	}
}
