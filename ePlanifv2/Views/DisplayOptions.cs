using ModelLib;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ePlanifv2.Views
{
	public sealed class DisplayOptions:DependencyObject
	{
		private static DisplayOptions instance = new DisplayOptions();
		public static DisplayOptions Instance
		{
			get { return instance; }
		}

		private static BrushConverter brushConverter = new BrushConverter();


		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(double), typeof(DisplayOptions),new PropertyMetadata(200d));
		public double ColumnWidth
		{
			get { return (double)GetValue(ColumnWidthProperty); }
			set { SetValue(ColumnWidthProperty, value); }
		}


		public static SolidColorBrush TextBrush = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
		public static SolidColorBrush TextDarkBrush = new SolidColorBrush(Color.FromArgb(255, 48, 48, 48));
		public static SolidColorBrush DarkBrush = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));
		public static SolidColorBrush ShadowBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
		public static SolidColorBrush TransparentDarkBrush = new SolidColorBrush(Color.FromArgb(92, 0, 0, 0));
		public static SolidColorBrush TransparentLightBrush = new SolidColorBrush(Color.FromArgb(92, 255, 255, 255));
		public static SolidColorBrush SelectionBrush = new SolidColorBrush(Color.FromArgb(92, 0,0 , 255));
		public static LinearGradientBrush HighLightBrush = new LinearGradientBrush() { StartPoint = new Point(0.5, 0), EndPoint = new Point(0.5, 1), GradientStops = new GradientStopCollection(new GradientStop[] { new GradientStop(Color.FromArgb(136, 255, 255, 255), 0), new GradientStop(Colors.Transparent, 1) }) };
		public static LinearGradientBrush InvertedHighLightBrush = new LinearGradientBrush() { StartPoint = new Point(0.5, 0), EndPoint = new Point(0.5, 1), GradientStops = new GradientStopCollection(new GradientStop[] {  new GradientStop(Colors.Transparent, 1), new GradientStop(Color.FromArgb(136, 255, 255, 255), 0) }) };
		public static LinearGradientBrush BevelBrush = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(1, 1), GradientStops = new GradientStopCollection(new GradientStop[] { new GradientStop(TransparentLightBrush.Color, 0), new GradientStop(TransparentDarkBrush.Color, 1) }) };
		public static LinearGradientBrush InvertedBevelBrush = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(1, 1), GradientStops = new GradientStopCollection(new GradientStop[] { new GradientStop(TransparentDarkBrush.Color, 0), new GradientStop(TransparentLightBrush.Color, 1) }) };


		public static Pen TransparentDarkPen = new Pen(TransparentDarkBrush, 1);
		public static Pen TransparentLightPen = new Pen(TransparentLightBrush, 1);
		public static Pen HighLightPen = new Pen(HighLightBrush, 1);
		public static Pen SelectionPen = new Pen(SelectionBrush, 1);
		public static Pen InvertedHighLightPen = new Pen(InvertedHighLightBrush, 1);
		public static Pen PhotoPen = new Pen(Brushes.White, 2);
		public static Pen BevelPen = new Pen(BevelBrush, 1);
		public static Pen InvertedBevelPen = new Pen(InvertedBevelBrush, 1);

		public static FlowDirection FlowDirection= CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
		public static Typeface Typeface= new Typeface("Segoe UI");

		public static BitmapImage ExclamationImage =  new BitmapImage(new Uri("pack://application:,,,/Images/exclamation.png"));
		public static BitmapImage InterogationImage =  new BitmapImage(new Uri("pack://application:,,,/Images/help.png"));
		public static BitmapImage CalendarImage = new BitmapImage(new Uri("pack://application:,,,/Images/calendar.png"));
		public static BitmapImage LargeCalendarImage = new BitmapImage(new Uri("pack://application:,,,/Images/calendar-month-transparent.png"));
		public static BitmapImage DoorImage = new BitmapImage(new Uri("pack://application:,,,/Images/door--exclamation.png"));
		public static BitmapImage LockImage = new BitmapImage(new Uri("pack://application:,,,/Images/lock-warning.png"));

		private DisplayOptions()
		{

		}

		public static FormattedText FormatText(string Text, Brush Foreground ,double Size=16,double? MaxWidth=null)
		{
			FormattedText text;

			text = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection, Typeface, Size, Foreground);
			text.Trimming = TextTrimming.CharacterEllipsis;text.MaxLineCount = 1;
			if (MaxWidth.HasValue) text.SetMaxTextWidths(new double[] {MaxWidth.Value });
		
			return text;
		}
		public static Point GetTextPosition(Rect Rect, FormattedText Text, HorizontalAlignment HorizontalAlignment,VerticalAlignment VerticalAlignment)
		{
			double x, y;

			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					x = Rect.Left;
					break;
				case HorizontalAlignment.Right:
					x = Rect.Right - Text.WidthIncludingTrailingWhitespace;
					break;
				case HorizontalAlignment.Center:
					x = Rect.Left + (Rect.Width - Text.WidthIncludingTrailingWhitespace) / 2;
					break;
				default:
					x = Rect.Left;
					break;
			}
			switch (VerticalAlignment)
			{
				case VerticalAlignment.Top:
					y = Rect.Top;
					break;
				case VerticalAlignment.Bottom:
					y = Rect.Bottom - Text.Height;
					break;
				case VerticalAlignment.Center:
					y = Rect.Top + (Rect.Height- Text.Height) / 2;
					break;
				default:
					y = Rect.Top;
					break;
			}


			return new Point(x, y);
		}

		public static SolidColorBrush GetBrush(string Color)
		{
			try
			{
				return brushConverter.ConvertFromString(Color) as SolidColorBrush;
			}
			catch
			{
				return Brushes.LightGray;
			}
		}

		public static void RenderBevel(DrawingContext Context, Rect Rect)
		{
			Context.DrawLine(DisplayOptions.TransparentDarkPen, Rect.TopRight, Rect.BottomRight);
			Context.DrawLine(DisplayOptions.TransparentDarkPen, Rect.BottomLeft, Rect.BottomRight + new Vector(-1, 0));
			Context.DrawLine(DisplayOptions.TransparentLightPen, Rect.TopLeft + new Vector(1, 1), Rect.TopRight + new Vector(-1, 1));
			Context.DrawLine(DisplayOptions.TransparentLightPen, Rect.TopLeft + new Vector(1, 1), Rect.BottomLeft + new Vector(1, -1));//*/
		}
		public static void RenderInvertedBevel(DrawingContext Context, Rect Rect)
		{
			Context.DrawLine(DisplayOptions.TransparentLightPen, Rect.TopRight, Rect.BottomRight);
			Context.DrawLine(DisplayOptions.TransparentLightPen, Rect.BottomLeft, Rect.BottomRight + new Vector(-1, 0));
			Context.DrawLine(DisplayOptions.TransparentDarkPen, Rect.TopLeft + new Vector(0, 1), Rect.TopRight + new Vector(-1, 1));
			Context.DrawLine(DisplayOptions.TransparentDarkPen, Rect.TopLeft + new Vector(1, 1), Rect.BottomLeft + new Vector(1, -1));
		}
		public static void RenderHighLight(DrawingContext Context, Rect Rect)
		{
			Context.DrawRectangle(DisplayOptions.HighLightBrush, null, Rect);
		}

		

	




	}

	
}
