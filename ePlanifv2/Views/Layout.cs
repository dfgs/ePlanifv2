using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifv2.Views
{
	public class Layout
	{
		private Rect freeRect;
		public Rect FreeRect
		{
			get { return freeRect; }
		}

		public Layout(Rect Rect)
		{
			freeRect = Rect;
			
		}

		public void Trim(double Size)
		{
			freeRect = Trim(freeRect, Size);
		}
		public void Trim(double Left,double Top,double Right,double Bottom)
		{
			freeRect = Trim(freeRect, Left, Top, Right, Bottom);
		}

		public static Rect Trim(Rect Source,double Size)
		{
			return  new Rect(Source.Left + Size, Source.Top + Size, Source.Width - 2 * Size, Source.Height - 2 * Size);
		}
		public static Rect Trim(Rect Source, double Left, double Top, double Right, double Bottom)
		{
			return new Rect(Source.Left + Left, Source.Top + Top, Source.Width - Left - Right, Source.Height - Top - Bottom);
		}

		public Rect DockTop(double Height)
		{
			Rect result;

			result = new Rect(freeRect.Left, freeRect.Top, freeRect.Width, Height);
			freeRect = new Rect(freeRect.Left, freeRect.Top + Height, freeRect.Width, freeRect.Height - Height);

			return result;
		}
		public Rect DockBottom(double Height)
		{
			Rect result;

			result = new Rect(freeRect.Left, freeRect.Top+freeRect.Height-1-Height, freeRect.Width, Height);
			freeRect = new Rect(freeRect.Left, freeRect.Top , freeRect.Width, freeRect.Height - Height);

			return result;
		}

		public Rect DockLeft(double Width)
		{
			Rect result;

			result = new Rect(freeRect.Left, freeRect.Top, Width, freeRect.Height);
			freeRect = new Rect(freeRect.Left+Width, freeRect.Top , freeRect.Width-Width, freeRect.Height );

			return result;
		}
		public Rect DockRight(double Width)
		{
			Rect result;

			result = new Rect(freeRect.Left+freeRect.Width-1-Width, freeRect.Top, Width, freeRect.Height);
			freeRect = new Rect(freeRect.Left , freeRect.Top, freeRect.Width - Width, freeRect.Height);

			return result;
		}

		public Rect SplitTop()
		{
			return DockTop(freeRect.Height / 2.0d);
		}
		public Rect SplitBottom()
		{
			return DockBottom(freeRect.Height / 2.0d);
		}
		public Rect SplitLeft()
		{
			return DockLeft(freeRect.Width / 2.0d);
		}
		public Rect SplitRight()
		{
			return DockRight(freeRect.Width / 2.0d);
		}

		public static Rect Center(Rect Source,double Width,double Height)
		{
			return new Rect(Source.Left + (Source.Width - Width) / 2, Source.Top + (Source.Height - Height) / 2, Width, Height);
		}

		public static Point GetCenter(Rect Source)
		{
			return new Point(Source.Left + Source.Width / 2, Source.Top + Source.Height / 2);
		}

	}
}
