using Microsoft.Reporting.WinForms;
using ModelLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ePlanifv2
{
	/// <summary>
	/// Logique d'interaction pour ReportWindow.xaml
	/// </summary>
	public partial class ReportWindow : Window
	{

		public static readonly DependencyProperty ReportPathProperty = DependencyProperty.Register("ReportPath", typeof(string), typeof(ReportWindow));
		public string ReportPath
		{
			get { return (string)GetValue(ReportPathProperty); }
			set { SetValue(ReportPathProperty, value); }
		}


		public static readonly DependencyProperty ReportServerURLProperty = DependencyProperty.Register("ReportServerURL", typeof(string), typeof(ReportWindow));
		public string ReportServerURL
		{
			get { return (string)GetValue(ReportServerURLProperty); }
			set { SetValue(ReportServerURLProperty, value); }
		}

		public ReportWindow()
		{
			InitializeComponent();

		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServerURL); //@"http://localhost/ReportServer"
			reportViewer.ServerReport.ReportPath =  ReportPath; //@"/Activities"

			//reportViewer.ServerReport.SetParameters(new ReportParameter("StartDate", DateTime.Now.ToString()));
			//reportViewer.ServerReport.SetParameters(new ReportParameter("EndDate", DateTime.Now.AddDays(7).ToString()));
			//reportViewer.ServerReport.Refresh();

			reportViewer.RefreshReport();
		}


	}
}
