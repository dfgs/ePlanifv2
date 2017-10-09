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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ePlanifv2.Views
{
	/// <summary>
	/// Logique d'interaction pour LoadView.xaml
	/// </summary>
	public partial class LoadView : UserControl
	{
		private DoubleAnimation animation1, animation2, animation3;

		
		
		public LoadView()
		{
			InitializeComponent();


			animation1 = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(2)));
			animation1.RepeatBehavior = RepeatBehavior.Forever;

			animation2 = new DoubleAnimation(120, -240, new Duration(TimeSpan.FromSeconds(2)));
			animation2.RepeatBehavior = RepeatBehavior.Forever;

			animation3 = new DoubleAnimation(240, 600, new Duration(TimeSpan.FromSeconds(2)));
			animation3.RepeatBehavior = RepeatBehavior.Forever;

			
		}


		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (IsVisible)
			{
				transform1.BeginAnimation(RotateTransform.AngleProperty, animation1, HandoffBehavior.SnapshotAndReplace);
				transform2.BeginAnimation(RotateTransform.AngleProperty, animation2, HandoffBehavior.SnapshotAndReplace);
				transform3.BeginAnimation(RotateTransform.AngleProperty, animation3, HandoffBehavior.SnapshotAndReplace);
			}
			else
			{
				transform1.BeginAnimation(RotateTransform.AngleProperty, null, HandoffBehavior.SnapshotAndReplace);
				transform2.BeginAnimation(RotateTransform.AngleProperty, null, HandoffBehavior.SnapshotAndReplace);
				transform3.BeginAnimation(RotateTransform.AngleProperty, null, HandoffBehavior.SnapshotAndReplace);
			}
		}




	}
}
