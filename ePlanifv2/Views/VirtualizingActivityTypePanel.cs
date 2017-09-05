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
	public class VirtualizingActivityTypePanel:VirtualizingGridPanel<ActivityTypeViewMemberViewModel>
	{

		protected override void OnRenderVerticalHeaderContent(DrawingContext Context, Rect Rect, ActivityTypeViewMemberViewModel Content)
		{
			FormattedText text;
			Point pos;
			ActivityTypeViewModel activityType;
			Layout layout;

			layout = new Layout(Rect);
			layout.Trim(8);
			activityType = Content.ActivityType;

			text = DisplayOptions.FormatText(activityType.Name.ToString(), DisplayOptions.TextDarkBrush, 16);
			pos = DisplayOptions.GetTextPosition(layout.FreeRect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			Context.DrawText(text, pos);
		}

		protected override void OnRenderActivityContent(DrawingContext Context, Rect Rect, ActivityViewModel Content)
		{
			FormattedText text;
			Point pos;
			Brush textBrush;
			Layout layout;
			Rect rect;

			layout = new Layout(Rect);

			if (Content.Employee.WriteAccess!=true)
			{
				rect = layout.DockRight(Rect.Height);
				Context.DrawImage(DisplayOptions.LockImage, rect);
			}

			textBrush = DisplayOptions.GetBrush(Content.TextColor.ToString());
			text = DisplayOptions.FormatText(Content.Employee.FullName, textBrush, 12, layout.FreeRect.Width);
			if (Content.Employee.IsDisabled.Value) text.SetTextDecorations(TextDecorations.Strikethrough);
			pos = DisplayOptions.GetTextPosition(layout.FreeRect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			Context.DrawText(text, pos);
		}

		


	}
}
