using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ePlanifv2.Views
{
	public class SelectionAdorner : Adorner
	{
		private static Pen pen = new Pen(Brushes.Gray, 3) ;

		public static readonly DependencyProperty PointAProperty = DependencyProperty.Register("PointA", typeof(Point), typeof(SelectionAdorner),new FrameworkPropertyMetadata(default(Point),FrameworkPropertyMetadataOptions.AffectsRender));
		public Point PointA
		{
			get { return (Point)GetValue(PointAProperty); }
			set { SetValue(PointAProperty, value); }
		}


		public static readonly DependencyProperty PointBProperty = DependencyProperty.Register("PointB", typeof(Point), typeof(SelectionAdorner), new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender));
		public Point PointB
		{
			get { return (Point)GetValue(PointBProperty); }
			set { SetValue(PointBProperty, value); }
		}


		public SelectionAdorner(UIElement adornedElement) : base(adornedElement)
		{
			IsHitTestVisible = false;
		}

		public void GetSelectionPoints(out double x1, out double y1, out double x2, out double y2)
		{

			if (PointA.X < PointB.X)
			{
				x1 = PointA.X; x2 = PointB.X ;
			}
			else
			{
				x1 = PointB.X; x2 = PointA.X ;

			}
			if (PointA.Y < PointB.Y)
			{
				y1 = PointA.Y; y2 = PointB.Y ;
			}
			else
			{
				y1 = PointB.Y; y2 = PointA.Y ;
			}

		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			double x1, y1,x2,y2, w, h;

			base.OnRender(drawingContext);

			GetSelectionPoints(out x1, out y1, out x2, out y2);
			w = x2 - x1;h = y2 - y1;

			
			drawingContext.DrawRectangle(Brushes.Transparent, pen, new Rect(x1,y1,w,h));
		}
	}
}
