using ePlanifViewModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ePlanifv2.Views
{
	public class VirtualizingEmployeePanel:VirtualizingGridPanel<EmployeeViewMemberViewModel>
	{

		private Geometry CreatePieGeometry(Point Center, double InRadiusX, double InRadiusY, double OutRadiusX, double OutRadiusY, double StartAngle, double EndAngle)
		{


			double outXStart = Center.X + (OutRadiusX * Math.Cos(StartAngle * Math.PI / 180.0));
			double outYStart = Center.Y - (OutRadiusY * Math.Sin(StartAngle * Math.PI / 180.0));
			double outXEnd = Center.X + (OutRadiusX * Math.Cos(EndAngle * Math.PI / 180.0));
			double outYEnd = Center.Y - (OutRadiusY * Math.Sin(EndAngle * Math.PI / 180.0));

			double inXStart = Center.X + (InRadiusX * Math.Cos(StartAngle * Math.PI / 180.0));
			double inYStart = Center.Y - (InRadiusY * Math.Sin(StartAngle * Math.PI / 180.0));
			double inXEnd = Center.X + (InRadiusX * Math.Cos(EndAngle * Math.PI / 180.0));
			double inYEnd = Center.Y - (InRadiusY * Math.Sin(EndAngle * Math.PI / 180.0));


			StreamGeometry geom = new StreamGeometry();
			using (StreamGeometryContext ctx = geom.Open())
			{
				ctx.BeginFigure(new Point(inXStart, inYStart), true, true);  // Closed
				ctx.LineTo(new Point(outXStart, outYStart), true, false);
				ctx.ArcTo(new Point(outXEnd, outYEnd), new Size(OutRadiusX, OutRadiusY), 0.0, (EndAngle - StartAngle) > 180, SweepDirection.Counterclockwise, true, false);
				ctx.LineTo(new Point(inXEnd, inYEnd), true, false);
				ctx.ArcTo(new Point(inXStart, inYStart), new Size(InRadiusX, InRadiusY), 0.0, (EndAngle - StartAngle) > 180, SweepDirection.Clockwise, true, false);
			}

			return geom;
		}

		private float Lerp(float v0,float v1, float t)
		{
			return v0 + t * (v1 - v0);
		}

		protected override void OnRenderVerticalHeaderContent(DrawingContext Context, Rect Rect, EmployeeViewMemberViewModel Content)
		{
			FormattedText text;
			Point pos;
			EmployeeViewModel employee;
			Rect rect;
			Layout layout,totalTimeLayout;
			Point center;
			double angle;
			Brush pieBrush;
			byte pieAlpha = 92;
			double percent;
			double totalTime,workingTime, maxWorkingTime;

			layout = new Layout(Rect);
			layout.Trim(8);

			employee = Content.Employee;

			rect = layout.DockRight(32);
			totalTimeLayout = new Layout(rect);

			totalTime = employee.GetTotalHours().TotalMinutes;
			workingTime = employee.WorkingTimePerWeek.HasValue ? employee.WorkingTimePerWeek.Value.TotalMinutes : totalTime;
			maxWorkingTime = employee.MaxWorkingTimePerWeek.HasValue ? employee.MaxWorkingTimePerWeek.Value.TotalMinutes : totalTime;
			if (workingTime == 0) workingTime = totalTime;


			percent = totalTime / workingTime;
			angle = Math.Min(359.9, 359.9 *percent );
			text = DisplayOptions.FormatText(((int)(totalTime / 60)).ToString("00") + ":" + (totalTime % 60).ToString("00"), DisplayOptions.TextBrush, 16); //String.Format("{0:00}:{1:00}", total.H,total.Minutes)
			if (totalTime > maxWorkingTime)
			{
				pieBrush = new SolidColorBrush(Color.FromArgb(pieAlpha, 255, 0, 0));
				text.SetForegroundBrush(Brushes.IndianRed);
				text.SetFontWeight(FontWeights.Bold);
			}
			else if (totalTime> workingTime)
			{
				pieBrush = new SolidColorBrush(Color.FromArgb(pieAlpha, 255, 128, 0));
			}
			else
			{
				pieBrush = new SolidColorBrush(Color.FromArgb(pieAlpha, 0, 255, 0));
			}


			rect = totalTimeLayout.SplitTop();
			center = new Point(rect.Left + rect.Width / 2, rect.Bottom);
			Context.DrawGeometry(pieBrush, null, CreatePieGeometry(center, 8, 8, 16, 16, 0, angle));
			Context.DrawEllipse(null, DisplayOptions.InvertedBevelPen, center, 16, 16);
			Context.DrawEllipse(null, DisplayOptions.BevelPen, center, 8, 8);

			totalTimeLayout.DockTop(18);
			rect = totalTimeLayout.FreeRect;
			pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Center, VerticalAlignment.Top);
			Context.DrawText(text, pos);


			#region photo
			if (Content.Service.Option.DisplayPhotos == true)
			{
				layout.DockRight(8);
				rect = Layout.Center(layout.DockLeft(64), 64, 64);
				center = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);

				Geometry geometry;
				geometry = new EllipseGeometry(rect);
				Context.PushClip(geometry);
				if (employee.Photo == null)
				{
					Context.DrawEllipse(DisplayOptions.ShadowBrush, DisplayOptions.InvertedBevelPen, center, 32, 32);
					text = DisplayOptions.FormatText(String.Concat(employee.LastName.Value.Value[0], employee.FirstName.Value.Value[0]), Brushes.White, 24, 64);
					Context.DrawText(text, DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Center, VerticalAlignment.Center));
				}
				else
				{
					Context.DrawImage(employee.Photo.Image, rect);
					Context.DrawEllipse(null, DisplayOptions.InvertedBevelPen, center, 32, 32);
				}
				Context.Pop();
				layout.DockLeft(8);
			}
			#endregion

			rect = layout.SplitTop();
			text = DisplayOptions.FormatText(employee.LastName.ToString(), DisplayOptions.TextDarkBrush, 16);
			pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Bottom);
			Context.DrawText(text, pos);

			text = DisplayOptions.FormatText(employee.FirstName.ToString(), DisplayOptions.TextBrush, 12);
			pos = DisplayOptions.GetTextPosition(layout.FreeRect, text, HorizontalAlignment.Left, VerticalAlignment.Top);
			Context.DrawText(text, pos);

			if (employee.WriteAccess!=true)
			{
				Context.DrawImage(DisplayOptions.LockImage, new Rect(Rect.Right - 20, Rect.Top+4, 16, 16));
			}

		}

		protected override void OnRenderActivityContent(DrawingContext Context, Rect Rect, ActivityViewModel Content)
		{
			FormattedText text;
			Point pos;
			Brush textBrush;
			Rect rect;
			Layout layout;

			layout = new Layout(Rect);
			
			rect = layout.SplitTop();
			// activity type
			textBrush = DisplayOptions.GetBrush(Content.TextColor.ToString());
			text = DisplayOptions.FormatText(Content.ActivityType?.Name?.Value ?? "NA", textBrush, 12, rect.Width);
			if (Content.ActivityType?.IsDisabled == true) text.SetTextDecorations(TextDecorations.Strikethrough);
			pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			Context.DrawText(text, pos);

			
			// comment
			text = DisplayOptions.FormatText(Content.Comment.ToString(), textBrush, 12, layout.FreeRect.Width); text.SetFontStyle(FontStyles.Italic); text.SetFontWeight(FontWeights.Bold);
			pos = DisplayOptions.GetTextPosition(layout.FreeRect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			Context.DrawText(text, pos);



		}

	}
}
