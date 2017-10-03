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

		protected override void OnRenderVerticalHeaderContent(DrawingContext Context, Rect Rect, EmployeeViewMemberViewModel Content)
		{
			FormattedText text;
			Point pos;
			EmployeeViewModel employee;
			Rect rect;
			TimeSpan total;
			Layout layout;

			layout = new Layout(Rect);
			layout.Trim(8);

			employee = Content.Employee;


			total = employee.GetTotalHours();
			text = DisplayOptions.FormatText(((int)(total.TotalMinutes / 60)).ToString("00") + ":" + (total.TotalMinutes % 60).ToString("00"), DisplayOptions.TextBrush, 16); //String.Format("{0:00}:{1:00}", total.H,total.Minutes)
			if ((employee.MaxWorkingHoursPerWeek.HasValue) && (total.TotalMinutes > employee.MaxWorkingHoursPerWeek.Value * 60))
			{
				text.SetForegroundBrush(Brushes.IndianRed);
				text.SetFontWeight(FontWeights.Bold);
			}
			rect = layout.DockRight(text.WidthIncludingTrailingWhitespace);
			pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Center, VerticalAlignment.Center);
			Context.DrawText(text, pos);
			layout.DockRight(8);

			
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

			textBrush = DisplayOptions.GetBrush(Content.TextColor.ToString());
			text = DisplayOptions.FormatText(Content.ActivityType?.Name?.Value ?? "NA", textBrush, 12, Rect.Width);
			if (Content.ActivityType?.IsDisabled == true) text.SetTextDecorations(TextDecorations.Strikethrough);
			pos = DisplayOptions.GetTextPosition(Rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			Context.DrawText(text, pos);
		}

	}
}
