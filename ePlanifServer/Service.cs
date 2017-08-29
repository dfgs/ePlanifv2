using ePlanifServerLib;
using LogUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServer
{
	public partial class Service : ServiceBase
	{
		private const string source = "ePlanifServer";

		private ServiceHost serviceHost;

		public Service()
		{
			ServiceName = "ePlanifServer";
			
			InitializeComponent();

			if (!EventLog.SourceExists(source)) EventLog.CreateEventSource(source, "Application");

		}

		protected override void OnStart(string[] args)
		{
			try
			{
				string path = System.IO.Path.Combine(@"C:\ProgramData", "ePlanifServer");
				Logger.StartLogToFile(path);
				if (serviceHost != null) serviceHost.Close();
				serviceHost = new ServiceHost(typeof(ePlanifService));
				serviceHost.Open();
				EventLog.WriteEntry(source, "ePlanif server started successfully", EventLogEntryType.Information);
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(source, "Failed to start ePlanif server (" + ex.Message + ")", EventLogEntryType.Error);
			}
		}

		protected override void OnStop()
		{
			try
			{
				if (serviceHost != null)
				{
					serviceHost.Close();
					serviceHost = null;
					Logger.StopLogToFile();
					EventLog.WriteEntry(source, "ePlanif server stopped successfully", EventLogEntryType.Information);
				}
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(source, "Failed to stop ePlanif server (" + ex.Message + ")", EventLogEntryType.Error);
			}
		}

	}
	
}
