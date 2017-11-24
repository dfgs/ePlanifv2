using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifv2
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
	{
		private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			//e.Handled = true;
			string path;

			StreamWriter writer;
			path=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ePlanif");

			Directory.CreateDirectory(path);
			using (FileStream stream = new FileStream(Path.Combine(path,"dump.txt"), FileMode.Create))
			{
				writer = new StreamWriter(stream);
				writer.Write(e.Exception.ToString());
				writer.Flush();
			}
		}
	}
}
