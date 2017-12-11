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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ePlanifv2.Views
{
	/// <summary>
	/// Logique d'interaction pour BulletView.xaml
	/// </summary>
	public partial class BulletView : UserControl
	{

		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(BulletView));
		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}


		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(BulletView));
		public object Value
		{
			get { return (object)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}



		public BulletView()
		{
			InitializeComponent();
		}
	}
}
