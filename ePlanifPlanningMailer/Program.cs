﻿using LogUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifPlanningMailer
{
	class Program
	{
		private static MailerWorker worker;
		private static string locker = "locker";

		static void Main(string[] args)
		{
			string path = System.IO.Path.Combine(@"C:\ProgramData", "ePlanifMailer");
			Logger.StartLogToFile(path);

			Logger.DebugLog += Logger_DebugLog;
			Logger.InformationLog += Logger_InformationLog;
			Logger.WarningLog += Logger_WarningLog;
			Logger.ErrorLog += Logger_ErrorLog;
			Logger.FatalLog += Logger_FatalLog;

			worker = new MailerWorker();
			worker.Run();
		}

		private static void Logger_FatalLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine(e.Log);
			}
		}

		private static void Logger_ErrorLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Log);
			}
		}

		private static void Logger_WarningLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(e.Log);
			}
		}

		private static void Logger_InformationLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(e.Log);
			}
		}

		private static void Logger_DebugLog(object sender, LogEventArgs e)
		{
			lock (locker)
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine(e.Log);
			}
		}
	}
}
