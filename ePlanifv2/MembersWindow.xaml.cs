using System;
using System.Collections;
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
	/// Logique d'interaction pour MembersWindow.xaml
	/// </summary>
	public partial class MembersWindow : Window
	{
		public IEnumerable SelectedItems
		{
			get { return listBox.SelectedItems; }
		}

		public MembersWindow()
		{
			InitializeComponent();
		}

		

		private void ButtonOK_Click(object sender,RoutedEventArgs e)
		{
			DialogResult = true;
		}
		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}


	}
}
