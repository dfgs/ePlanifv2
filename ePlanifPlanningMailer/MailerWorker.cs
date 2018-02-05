using DatabaseModelLib.Filters;
using ePlanifModelsLib;
using LogUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerLib;
using ePlanifPlanningMailer.ReportingExecutionServices;
using System.IO;
using System.Net.Mail;

namespace ePlanifPlanningMailer
{
	public class MailerWorker : Worker
	{
		private ePlanifDatabase database;

		public MailerWorker() : base("Mailer worker")
		{
			database = new ePlanifDatabase("127.0.0.1");


		}
		public DateTime FirstDayOfWeek(DateTime Date)
		{
			DayOfWeek firstDayOfWeek;

			firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			
			int diff = Date.DayOfWeek - firstDayOfWeek;
			if (diff < 0) diff += 7;
			return Date.AddDays(-1 * diff).Date;
		}


		public void Run()
		{
			IEnumerable<Employee> employees;
			byte[] planning;
			DateTime nextWeekDate;
			int week;
			int year;

			nextWeekDate = FirstDayOfWeek(DateTime.Now.AddDays(7));
			week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(nextWeekDate, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
			year =  nextWeekDate.Year;

			WriteLog(LogLevels.Information, $"Next week is {week}");

			WriteLog(LogLevels.Information, "Try to get registered employees");
			try
			{
				employees = database.SelectAsync<Employee>(
					new AndFilter<Employee>(
						new EqualFilter<Employee>(Employee.IsRegisteredToMailPlannningColumn, true),
						new EqualFilter<Employee>(Employee.IsDisabledColumn, false)
						)
					).Result;
			}
			catch(Exception ex)
			{
				WriteLog(ex);
				return;
			}

			WriteLog(LogLevels.Information, "Parsing registered employees");
			foreach (Employee employee in employees.Where(item=>item.eMail!=null))
			{
				WriteLog(LogLevels.Information, $"Generating report for employee {employee.LastName} {employee.FirstName}");
				planning = GenerateReport(employee,week,year);
				if (planning == null)
				{
					WriteLog(LogLevels.Warning, "Failed to generate report");
					continue;
				}
				if (Properties.Settings.Default.SendMail) SendMail(employee.eMail.Value.Value, $"Your planning for week {week}", planning);
				if (Properties.Settings.Default.SaveToFile) SaveToFile(employee.eMail.Value.Value, $"Your planning for week {week}", planning);
			}


		}

		private void SendMail(string To,string Subject,byte[] Body)
		{
			SmtpClient client = new SmtpClient(Properties.Settings.Default.SMTPServer,Properties.Settings.Default.SMTPPort);
			client.EnableSsl = Properties.Settings.Default.SMTPSSL;
			client.Credentials= new System.Net.NetworkCredential(Properties.Settings.Default.SMTPUser, Properties.Settings.Default.SMTPPassword);
			MailAddress from = new MailAddress(Properties.Settings.Default.SMTPSender);
			MailAddress to = new MailAddress(To);

			MailMessage message = new MailMessage(from, to);
			message.IsBodyHtml = true;
			message.Body = Encoding.UTF8.GetString(Body);

			message.BodyEncoding = System.Text.Encoding.UTF8;

			message.Subject = Subject;
			message.SubjectEncoding = System.Text.Encoding.UTF8;

			WriteLog(LogLevels.Information, $"Sending email to {To}");
			try
			{
				client.Send(message);
			}
			catch(Exception ex)
			{
				WriteLog(ex);
			}

			message.Dispose();

		}

		private void SaveToFile(string To, string Subject, byte[] Body)
		{
			WriteLog(LogLevels.Information, $"Saving message to {To}");
			try
			{
				using (FileStream stream = new FileStream($"{To}.html", FileMode.Create))
				{
					stream.Write(Body, 0, Body.Length);
					stream.Flush();
				}
			}
			catch (Exception ex)
			{
				WriteLog(ex);
			}
		}

		private byte[] GenerateReport(Employee Employee,int Week,int Year)
		{

			ReportExecutionService rs = new ReportExecutionService();
			rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
			rs.Url = "http://localhost/reportserver/ReportExecution2005.asmx";

			// Render arguments
			byte[] result = null;
			string reportPath = Properties.Settings.Default.ReportName;
			string format = "HTML4.0";
			string historyID = null;
			string devInfo = @"<DeviceInfo><JavaScript>False</JavaScript><ExpandContent>True</ExpandContent></DeviceInfo>";//@"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

			// Prepare report parameter.
			ParameterValue[] parameters = new ParameterValue[5];
			parameters[0] = new ParameterValue();
			parameters[0].Name = "EmployeeID";
			parameters[0].Value = Employee.EmployeeID.ToString();
			parameters[1] = new ParameterValue();
			parameters[1].Name = "Week";
			parameters[1].Value = Week.ToString(); 
			parameters[2] = new ParameterValue();
			parameters[2].Name = "Year";
			parameters[2].Value = Year.ToString();
			parameters[3] = new ParameterValue();
			parameters[3].Name = "FirstName";
			parameters[3].Value = Employee.FirstName.ToString();
			parameters[4] = new ParameterValue();
			parameters[4].Name = "LastName";
			parameters[4].Value = Employee.LastName.ToString();

			string encoding;
			string mimeType;
			string extension;
			Warning[] warnings = null;
			string[] streamIDs = null;

			ExecutionInfo execInfo = new ExecutionInfo();
			//ExecutionHeader execHeader = new ExecutionHeader();
			//rs.ExecutionHeaderValue = execHeader;

			execInfo = rs.LoadReport(reportPath, historyID);

			rs.SetExecutionParameters(parameters, "en-us");
			
			try
			{
				result = rs.Render(format, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
				//execInfo = rs.GetExecutionInfo();
				//Console.WriteLine("Execution date and time: {0}", execInfo.ExecutionDateTime);
			}
			catch (System.Web.Services.Protocols.SoapException ex)
			{
				WriteLog(ex);
				return null;
			}
			return result;
		}




	}
}
