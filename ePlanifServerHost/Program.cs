using ePlanifServerLib;
using LogUtils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerHost
{
	class Program
	{
		private static string locker = "locker";

		static void Main(string[] args)
		{
			ServiceHost serviceHost;
			IDataProvider dataProvider;

			Logger.DebugLog += Logger_DebugLog;
			Logger.InformationLog += Logger_InformationLog;
			Logger.WarningLog += Logger_WarningLog;
			Logger.ErrorLog += Logger_ErrorLog;
			Logger.FatalLog += Logger_FatalLog;

			try
			{
				string path = System.IO.Path.Combine(@"C:\ProgramData", "ePlanifServer");
				Logger.StartLogToFile(path);

				//dataProvider = new SqlDataProvider();
				dataProvider = new TestDataProvider("liza");
				serviceHost = new ePlanifServiceHost(dataProvider);
				serviceHost.Open();
				Logger.WriteLog(LogLevels.Debug,"main",0,"ePlanif server started successfully");
			}
			catch (Exception ex)
			{
				Logger.WriteLog(LogLevels.Fatal,"main",0, "Failed to start ePlanif server (" + ex.Message + ")");
			}
			Console.ReadLine();
		}

		private static void Logger_FatalLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine(e.Log);
			}
		}

		private static void Logger_ErrorLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(e.Log);
			}
		}

		private static void Logger_WarningLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(e.Log);
			}
		}

		private static void Logger_InformationLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(e.Log);
			}
		}

		private static void Logger_DebugLog(object sender, LogEventArgs e)
		{
			lock(locker)
			{
				Console.ForegroundColor = ConsoleColor.Gray;Console.WriteLine(e.Log);
			}
		}



	}
}
