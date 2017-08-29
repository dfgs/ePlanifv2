using ModelLib;
using System;
using System.Collections.Generic;
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
	/// Logique d'interaction pour ConnectWindow.xaml
	/// </summary>
	public partial class ConnectWindow : Window
	{

		public static readonly DependencyProperty LoginProperty = DependencyProperty.Register("Login", typeof(string), typeof(ConnectWindow));
		public string Login
		{
			get { return (string)GetValue(LoginProperty); }
			set { SetValue(LoginProperty, value); }
		}

		public static readonly DependencyProperty ServerProperty = DependencyProperty.Register("Server", typeof(string), typeof(ConnectWindow));
		public string Server
		{
			get { return (string)GetValue(ServerProperty); }
			set { SetValue(ServerProperty, value); }
		}


		
		public ConnectWindow()
		{
			InitializeComponent();
		}

		private void OKCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !(string.IsNullOrWhiteSpace(Server) );
			e.Handled = true;
		}
		private void OKCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = true;
		}
		private void CancelCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true; e.Handled = true;
		}
		private void CancelCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			textBoxServer.Focus();
		}

	}
}
