using ePlanifViewModelsLib;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelLib;

namespace ePlanifv2.Views
{
	//public delegate void RenderVerticalHeaderHandler<RowViewModelType>(DrawingContext Context, Rect Rect, RowViewModelType Content);
	//public delegate void RenderActivityHandler(DrawingContext Context, Rect Rect, ActivityViewModel Content);

	public enum SelectionModes {None,Click,Drag };

	public abstract class VirtualizingGridPanel<RowViewModelType> : FrameworkElement, IScrollInfo
		where RowViewModelType: class,IRowViewModel
	{
		#region columns and rows properties

		public static readonly DependencyProperty TableViewModelProperty = DependencyProperty.Register("TableViewModel", typeof(IViewViewModel), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, TablePropertyChangedCallBack, TablePropertyCoerceCallBack));
		public IViewViewModel TableViewModel
		{
			get { return (IViewViewModel)GetValue(TableViewModelProperty); }
			set { SetValue(TableViewModelProperty, value); }
		}


		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(IViewModelCollection<DayViewModel>), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
		public IViewModelCollection<DayViewModel> Columns
		{
			get { return (IViewModelCollection<DayViewModel>)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}


		public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register("ColumnCount", typeof(int), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public int ColumnCount
		{
			get { return (int)GetValue(ColumnCountProperty); }
			set { SetValue(ColumnCountProperty, value); }
		}

		public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(IViewModelCollection<RowViewModelType>), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
		public IViewModelCollection<RowViewModelType> Rows
		{
			get { return (IViewModelCollection<RowViewModelType>)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}


		public static readonly DependencyProperty RowCountProperty = DependencyProperty.Register("RowCount", typeof(int), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public int RowCount
		{
			get { return (int)GetValue(RowCountProperty); }
			set { SetValue(RowCountProperty, value); }
		}


		public static readonly DependencyProperty LayersProperty = DependencyProperty.Register("Layers", typeof(IEnumerable<LayerViewModel>), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
		public IEnumerable<LayerViewModel> Layers
		{
			get { return (IEnumerable<LayerViewModel>)GetValue(LayersProperty); }
			set { SetValue(LayersProperty, value); }
		}
		


		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double ColumnWidth
		{
			get { return (double)GetValue(ColumnWidthProperty); }
			set { SetValue(ColumnWidthProperty, value); }
		}
		

		public static readonly DependencyProperty ActivityHeightProperty = DependencyProperty.Register("ActivityHeight", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(32d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double ActivityHeight
		{
			get { return (double)GetValue(ActivityHeightProperty); }
			set { SetValue(ActivityHeightProperty, value); }
		}

		public static readonly DependencyProperty ActivityMarginProperty = DependencyProperty.Register("ActivityMargin", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(4d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double ActivityMargin
		{
			get { return (double)GetValue(ActivityMarginProperty); }
			set { SetValue(ActivityMarginProperty, value); }
		}

		

		public static readonly DependencyProperty AddButtonMarginProperty = DependencyProperty.Register("AddButtonMargin", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double AddButtonMargin
		{
			get { return (double)GetValue(AddButtonMarginProperty); }
			set { SetValue(AddButtonMarginProperty, value); }
		}

		public static readonly DependencyProperty CellHeaderMarginProperty = DependencyProperty.Register("CellHeaderMargin", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double CellHeaderMargin
		{
			get { return (double)GetValue(CellHeaderMarginProperty); }
			set { SetValue(CellHeaderMarginProperty, value); }
		}

		public static readonly DependencyProperty MinRowHeightProperty = DependencyProperty.Register("MinRowHeight", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(80d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double MinRowHeight
		{
			get { return (double)GetValue(MinRowHeightProperty); }
			set { SetValue(MinRowHeightProperty, value); }
		}

		public static readonly DependencyProperty VerticalHeaderWidthProperty = DependencyProperty.Register("VerticalHeaderWidth", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double VerticalHeaderWidth
		{
			get { return (double)GetValue(VerticalHeaderWidthProperty); }
			set { SetValue(VerticalHeaderWidthProperty, value); }
		}

		public static readonly DependencyProperty HorizontalHeaderHeightProperty = DependencyProperty.Register("HorizontalHeaderHeight", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.AffectsRender, IScrollPropertyChangedCallBack));
		public double HorizontalHeaderHeight
		{
			get { return (double)GetValue(HorizontalHeaderHeightProperty); }
			set { SetValue(HorizontalHeaderHeightProperty, value); }
		}


		public static readonly DependencyProperty CornerContentProperty = DependencyProperty.Register("CornerContent", typeof(object), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
		public object CornerContent
		{
			get { return (object)GetValue(CornerContentProperty); }
			set { SetValue(CornerContentProperty, value); }
		}


		#endregion

		private SelectionModes selectionMode;
		private Point clickedPoint;
		private SelectionAdorner adorner;

		public static readonly DependencyProperty LayerIDProperty = DependencyProperty.Register("LayerID", typeof(int), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));
		public int LayerID
		{
			get { return (int)GetValue(LayerIDProperty); }
			set { SetValue(LayerIDProperty, value); }
		}

		#region IScrollInfo
		private bool canVerticallyScroll;
		public bool CanVerticallyScroll
		{
			get { return canVerticallyScroll; }
			set { canVerticallyScroll = value; }
		}

		private bool canHorizontallyScroll;
		public bool CanHorizontallyScroll
		{
			get { return canHorizontallyScroll; }
			set { canHorizontallyScroll = value; }
		}

		public double ExtentWidth
		{

			get { return  VerticalHeaderWidth+ ColumnCount * ColumnWidth; }
		}

		//private Dictionary<int, double> rowHeights;
		public double ExtentHeight
		{
			get
			{
				double result;
				result = HorizontalHeaderHeight;
				for (int t = 0; t < RowCount; t++)
				{
					result += GetRowHeight(t);
				}
				return result;
			}
		}

		private double viewportWidth;
		public double ViewportWidth
		{
			get { return viewportWidth; }
		}

		private double viewportHeight;
		public double ViewportHeight
		{
			get { return viewportHeight; }
		}


		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>),new FrameworkPropertyMetadata(0d,FrameworkPropertyMetadataOptions.AffectsRender));
		public double HorizontalOffset
		{
			get { return (double)GetValue(HorizontalOffsetProperty); }
			private set { SetValue(HorizontalOffsetProperty, value); }
		}


		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(VirtualizingGridPanel<RowViewModelType>), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));
		public double VerticalOffset
		{
			get { return (double)GetValue(VerticalOffsetProperty); }
			private set { SetValue(VerticalOffsetProperty, value); }
		}

		private ScrollViewer scrollOwner;
		public ScrollViewer ScrollOwner
		{
			get { return scrollOwner; }
			set { scrollOwner = value; }
		}
		#endregion



		

		public static readonly DependencyProperty IsToolTipVisibleProperty = DependencyProperty.Register("IsToolTipVisible", typeof(bool), typeof(VirtualizingGridPanel<RowViewModelType>));
		public bool IsToolTipVisible
		{
			get { return (bool)GetValue(IsToolTipVisibleProperty); }
			private set { SetValue(IsToolTipVisibleProperty, value); }
		}


		public static readonly DependencyProperty ToolTipContentProperty = DependencyProperty.Register("ToolTipContent", typeof(ActivityViewModel), typeof(VirtualizingGridPanel<RowViewModelType>));
		public ActivityViewModel ToolTipContent
		{
			get { return (ActivityViewModel)GetValue(ToolTipContentProperty); }
			private set { SetValue(ToolTipContentProperty, value); }
		}

		private Timer timer;

		public VirtualizingGridPanel()
		{
			//this.SnapsToDevicePixels = true;
			//SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
			Focusable = true;
			timer = new Timer(500) { AutoReset=false };
			timer.Elapsed += Timer_Elapsed;
		}
		protected virtual void OnTableChanging()
		{
			int t = 0;
		}

		protected virtual void OnTableChanged(IViewViewModel OldValue,IViewViewModel NewValue)
		{
			if (OldValue != null)
			{
				OldValue.Updated -= Table_Updated;
				OldValue.CellFocused -= Table_CellFocused;
			}
			if (NewValue != null)
			{
				NewValue.Updated += Table_Updated;
				NewValue.CellFocused += Table_CellFocused;
			}

			if (ScrollOwner != null) ScrollOwner.InvalidateScrollInfo();
		}

		private void Table_CellFocused(DependencyObject sender, int Column,int Row)
		{
			double x, y;
			x = ColumnWidth * Column-VerticalHeaderWidth;
			y = GetRowPosition(Row)-HorizontalHeaderHeight;
			SetHorizontalOffset(x);
			SetVerticalOffset(y);
			InvalidateVisual();
		}

		private void Table_Updated(object sender, EventArgs e)
		{
			SetHorizontalOffset(HorizontalOffset);SetVerticalOffset(VerticalOffset);
			InvalidateVisual();
		}
		
		private static void TablePropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			VirtualizingGridPanel<RowViewModelType> panel;
			panel = d as VirtualizingGridPanel<RowViewModelType>;
			if (panel == null) return;
			panel.OnTableChanged((IViewViewModel)e.OldValue, (IViewViewModel)e.NewValue);
		}
		private static object TablePropertyCoerceCallBack(DependencyObject d, object BaseValue)
		{
			VirtualizingGridPanel<RowViewModelType> panel;
			panel = d as VirtualizingGridPanel<RowViewModelType>;
			if (panel == null) return BaseValue;
			panel.OnTableChanging();
			return BaseValue;
		}
		private static void IScrollPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			VirtualizingGridPanel<RowViewModelType> panel;
			panel = d as VirtualizingGridPanel<RowViewModelType>;
			if (panel == null) return;
			if (panel.ScrollOwner != null) panel.ScrollOwner.InvalidateScrollInfo();
		}


		#region IScrollInfo

		public void LineUp()
		{
			double delta;
			SetVerticalOffset(VerticalOffset - GetRowHeight(GetFirstRow(out delta)));
		}

		public void LineDown()
		{
			double delta;
			SetVerticalOffset(VerticalOffset + GetRowHeight(GetFirstRow(out delta)));
		}

		public void LineLeft()
		{
			SetHorizontalOffset(HorizontalOffset - ColumnWidth);
		}

		public void LineRight()
		{
			SetHorizontalOffset(HorizontalOffset + ColumnWidth);
		}

		public void PageUp()
		{
			SetVerticalOffset(VerticalOffset - viewportHeight);
		}

		public void PageDown()
		{
			SetVerticalOffset(VerticalOffset + viewportHeight);
		}

		public void PageLeft()
		{
			SetHorizontalOffset(HorizontalOffset - viewportWidth);
		}

		public void PageRight()
		{
			SetHorizontalOffset(HorizontalOffset + viewportWidth);
		}

		public void MouseWheelUp()
		{
			SetVerticalOffset(VerticalOffset - ActivityHeight+ActivityMargin);
		}

		public void MouseWheelDown()
		{
			SetVerticalOffset(VerticalOffset + ActivityHeight + ActivityMargin);
		}

		public void MouseWheelLeft()
		{
			SetHorizontalOffset(HorizontalOffset - ActivityHeight + ActivityMargin);
		}

		public void MouseWheelRight()
		{
			SetHorizontalOffset(HorizontalOffset + ActivityHeight + ActivityMargin);
		}

		public void SetHorizontalOffset(double offset)
		{
			if (offset < 0 || ViewportWidth >= ExtentWidth)
			{
				offset = 0;
			}
			else
			{
				if (offset + ViewportWidth >= ExtentWidth)
				{
					offset = ExtentWidth - ViewportWidth;
				}
			}

			HorizontalOffset = (int)offset;

			if (scrollOwner != null) scrollOwner.InvalidateScrollInfo();
		}

		public void SetVerticalOffset(double offset)
		{
			if (offset < 0 || ViewportHeight >= ExtentHeight)
			{
				offset = 0;
			}
			else
			{
				if (offset + ViewportHeight >= ExtentHeight)
				{
					offset = ExtentHeight - ViewportHeight;
				}
			}

			VerticalOffset = (int)offset;

			if (scrollOwner != null) scrollOwner.InvalidateScrollInfo();
		}

		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			//throw new NotImplementedException();
			return Rect.Empty;
		}
		#endregion

		#region sizing
		private double GetCellHeight(CellViewModel Cell)
		{
			if (Cell == null) return 0;	
			return (Cell.GetActivities(LayerID)?.Count??0) * (ActivityHeight+ActivityMargin) + ActivityMargin+AddButtonMargin + CellHeaderMargin;
		}
		private double GetRowHeight(int Row)
		{
			double height,max;

			if (TableViewModel == null) return MinRowHeight;

			height = 0; max = MinRowHeight;
			for (int c = 0; c < ColumnCount; c++)
			{
				height = GetCellHeight(TableViewModel.GetCellContent(c, Row));
				if (height > max) max = height;
			}
			return max;
		}

		private double GetRowPosition(int Row)
		{
			double pos;

			pos = HorizontalHeaderHeight;
			for (int t = 0; t < Row; t++)
			{
				pos += GetRowHeight(t);
			}

			return pos;
		}
		private int GetFirstColumn(out double delta)
		{
			delta = HorizontalOffset % ColumnWidth;
			return (int)(HorizontalOffset / ColumnWidth); 
		}

		private int GetFirstRow(out double Delta)
		{
			double y = 0, rowHeight = 0;

			for (int row = 0; row < RowCount; row++)
			{
				rowHeight = GetRowHeight(row);
				if (y + rowHeight >= VerticalOffset)
				{
					Delta = VerticalOffset - y;
					return row;
				}
				y += rowHeight;
			}
			Delta = VerticalOffset - y;
			return RowCount - 1;
		}

		private ActivityViewModel GetActivityAtPos(Point Position)
		{
			int column, row;
			int index;
			CellViewModel cell;
			double rowPos;
			double relativePos;

			column = GetColumnAtPos(Position);
			row = GetRowAtPos(Position);
			if ((column < 0) || (column >= ColumnCount) || (row < 0) || (row > RowCount)) return null;

			cell = TableViewModel.GetCellContent(column, row);

			rowPos = GetRowPosition(row) - VerticalOffset;
			relativePos = (Position.Y - rowPos);
			if (relativePos <= CellHeaderMargin) return null;

			index = (int)((relativePos - CellHeaderMargin) / (ActivityHeight + ActivityMargin));
			if ((index >= 0) && (index < cell.GetActivities(LayerID).Count)) return cell.GetActivity(LayerID, index);

			return null;

		}
		private int GetColumnAtPos(Point Position)
		{
			int first;
			double delta;

			first = GetFirstColumn(out delta);
			return first+(int)((Position.X + delta-VerticalHeaderWidth) / ColumnWidth);
		}

		private int GetRowAtPos(Point Position)
		{
			double delta;
			double y;
			int row;

			row = GetFirstRow(out delta);
			y = HorizontalHeaderHeight - delta + GetRowHeight(row);
			while(y<Position.Y)
			{
				row++;
				y += GetRowHeight(row);
			}

			return row;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			viewportWidth = finalSize.Width; viewportHeight = finalSize.Height;
			SetHorizontalOffset(HorizontalOffset);SetVerticalOffset(VerticalOffset);
			
			return base.ArrangeOverride(finalSize);
		}
		#endregion

		#region render
		protected abstract void OnRenderVerticalHeaderContent(DrawingContext Context, Rect Rect, RowViewModelType Content);
		protected abstract void OnRenderActivityContent(DrawingContext Context, Rect Rect, ActivityViewModel Content);

		//
		private void OnRenderActivity(DrawingContext Context, Rect Rect, ActivityViewModel Activity)
		{
			FormattedText text;
			Point pos;
			Rect rect;
			Brush brush;
			Brush textBrush;
			Pen textPen;
			Layout layout;
			Layout timeLayout;

			if (Activity.IsSelected)
			{
				brush = DisplayOptions.SelectionBrush;
			}
			else
			{
				brush = DisplayOptions.GetBrush(Activity.Color.ToString());
			}

			textBrush = DisplayOptions.GetBrush(Activity.TextColor.ToString());
			textPen = new Pen(textBrush, 1);

			//DisplayOptions.RenderBevel(Context, Rect);
			Context.DrawRectangle(brush, null, Rect);
			Context.DrawRectangle(DisplayOptions.TransparentLightBrush, DisplayOptions.TransparentDarkPen, Rect);

			layout = new Layout(Rect);
			layout.Trim(4);

			if (Activity.IsDraft == true)
			{
				rect = Layout.Center( layout.DockRight(16),16,16);
				Context.DrawImage(DisplayOptions.InterogationImage, rect);
			}
			// time
			text = DisplayOptions.FormatText(String.Format("{0:HH:mm}", Activity.StartTime), textBrush, 12);
			timeLayout=new Layout( layout.DockLeft(text.WidthIncludingTrailingWhitespace+4));
			rect = timeLayout.SplitTop();
			pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Bottom);
			Context.DrawText(text, pos);
			text = DisplayOptions.FormatText(String.Format("{0:HH:mm}", Activity.StopTime), textBrush, 12);
			pos = DisplayOptions.GetTextPosition(timeLayout.FreeRect, text, HorizontalAlignment.Left, VerticalAlignment.Top);
			Context.DrawText(text, pos);

			// separator
			rect = layout.DockLeft(4);
			Context.DrawLine(textPen, rect.TopLeft, rect.BottomLeft);

			// activity type
			OnRenderActivityContent(Context, layout.FreeRect, Activity);
			



		}

		//
		private void OnRenderCell(DrawingContext Context, Rect Rect, CellViewModel Cell)
		{
			FormattedText text,iconText;
			Point pos;
			IEnumerable<ActivityViewModel> activities;
			Rect rect;
			Brush layerBrush,cellBrush;
			int count;
			Layout layout;
			Layout iconLayout;

			Context.PushClip(new RectangleGeometry(Rect));

			if (Cell != null)
			{
			
				cellBrush = DisplayOptions.GetBrush(Cell.GetBackground(LayerID));
				Context.DrawRectangle(cellBrush, null, Rect);

				layout = new Layout(Rect);
				layout.Trim(2);
				iconLayout = new Layout( layout.DockTop(CellHeaderMargin)) ;

				activities = Cell.GetActivities(LayerID);
				if (Cell.IsPublicHoliday)
				{
					rect = iconLayout.DockLeft(16);
					Context.DrawImage(DisplayOptions.DoorImage, rect);
				}

				if (Cell.HasError)
				{
					rect = iconLayout.DockLeft(16);
					Context.DrawImage(DisplayOptions.ExclamationImage, rect);
				}
				if (Layers != null)
				{
					foreach (LayerViewModel layer in Layers)
					{
						if (layer.LayerID == LayerID) continue;
						count = Cell.GetActivities(layer.LayerID.Value).Count;
						if (count == 0) continue;

						rect = iconLayout.DockLeft(16);
						pos = Layout.GetCenter(rect);
						layerBrush = DisplayOptions.GetBrush(layer.Color.Value.Value);
						Context.DrawEllipse(layerBrush, null, pos, 7, 7);
						Context.DrawEllipse(DisplayOptions.HighLightBrush, DisplayOptions.InvertedHighLightPen, pos, 7, 7);

						iconText = DisplayOptions.FormatText(count.ToString(), Brushes.White, 10);
						pos = DisplayOptions.GetTextPosition(rect, iconText, HorizontalAlignment.Center, VerticalAlignment.Center);
						Context.DrawText(iconText, pos);
					}
				}

				if (Cell.IsSelected) Context.DrawRectangle(DisplayOptions.SelectionBrush, null, Rect);

				DisplayOptions.RenderBevel(Context, Rect);
				for (int t = 0; t < Cell.GetActivities(LayerID)?.Count; t++)
				{
					rect = Layout.Trim( layout.DockTop(ActivityHeight),ActivityMargin,0,ActivityMargin,0);
					layout.DockTop(ActivityMargin);
					Context.PushClip(new RectangleGeometry(rect));
					OnRenderActivity(Context, rect, Cell.GetActivity(LayerID, t));
					//RenderActivity?.Invoke(Context, activityRect, Cell.GetActivity(t));
					Context.Pop();
				}

				if (Cell.IsSelected)
				{
					rect = layout.DockBottom(AddButtonMargin);
					text = DisplayOptions.FormatText("...", DisplayOptions.TextDarkBrush, AddButtonMargin);
					pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Center, VerticalAlignment.Top);
					Context.DrawText(text, pos);
				}
			}
			else
			{
				Context.DrawRectangle(Brushes.LightGray, null, Rect);
			}

			Context.Pop();
		}

		//
		private void OnRenderCornerHeader(DrawingContext Context, Rect Rect,object Corner)
		{
			FormattedText text;
			Point pos;
			Layout layout;
			Rect rect;

			Context.PushClip(new RectangleGeometry(Rect));		
			Context.DrawRectangle(Brushes.LightSteelBlue, null, Rect);
			DisplayOptions.RenderHighLight(Context, Rect);
			DisplayOptions.RenderBevel(Context, Rect);

			layout = new Layout(Rect);
			layout.Trim(8);

			rect = layout.DockLeft(32);
			Context.DrawImage(DisplayOptions.LargeCalendarImage, rect);
			layout.DockLeft(8);

			if (Corner != null)
			{
				text = DisplayOptions.FormatText(Corner.ToString(), DisplayOptions.TextDarkBrush, 32);
				pos = DisplayOptions.GetTextPosition(layout.FreeRect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
				Context.DrawText(text, pos);
			}
			Context.Pop();
		}
		//
		private void OnRenderHorizontalHeader(DrawingContext Context, Rect Rect, DayViewModel Day)
		{
			FormattedText text;
			Point pos;
			Rect rect;
			Layout layout;

			Context.PushClip(new RectangleGeometry(Rect));
			Context.DrawRectangle(Brushes.LightSteelBlue, null, Rect);
			DisplayOptions.RenderHighLight(Context, Rect);
			DisplayOptions.RenderBevel(Context, Rect);

			layout = new Layout(Rect);
			layout.Trim(8);

			if (Day != null)
			{

				rect = layout.DockLeft(40);
				text = DisplayOptions.FormatText(Day.DayOfMonth, DisplayOptions.TextDarkBrush, 32);
				pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
				Context.DrawText(text, pos);

				if (Day.Date == DateTime.Today)
				{
					rect = Layout.Center( layout.DockRight(16),16,16);
					Context.DrawImage(DisplayOptions.CalendarImage, rect);
				}

				rect = layout.SplitTop();
				text = DisplayOptions.FormatText(Day.DayOfWeek, DisplayOptions.TextDarkBrush, 16);
				pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
				Context.DrawText(text, pos);

				rect = layout.FreeRect;
				text = DisplayOptions.FormatText(Day.MonthWithYear, DisplayOptions.TextBrush, 12);
				pos = DisplayOptions.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
				Context.DrawText(text, pos);

				
			}
			Context.Pop();
		}
		
		//
		private void OnRenderVerticalHeader(DrawingContext Context, Rect Rect, RowViewModelType Row)
		{
			Context.PushClip(new RectangleGeometry(Rect));
			Context.DrawRectangle(Brushes.LightSteelBlue, null, Rect);
			DisplayOptions.RenderHighLight(Context, Rect);
			DisplayOptions.RenderBevel(Context, Rect);
			if (Row != null) OnRenderVerticalHeaderContent(Context, Rect,Row);
			Context.Pop();
		}

		//
		protected override void OnRender(DrawingContext drawingContext)
		{
			double rowHeight;
			int visibleColumns,visibleRows;
			int firstRow,firstColumn;
			double deltaX,deltaY;
			double x,y;
			int row;

			//if (TableViewModel == null) return;

			firstColumn = GetFirstColumn(out deltaX);
			firstRow = GetFirstRow(out deltaY);
			visibleColumns = (int)Math.Min(ColumnCount, Math.Ceiling((viewportWidth + deltaX - VerticalHeaderWidth) / ColumnWidth));
			visibleRows = 0;

			if ((firstColumn < 0) || (firstRow < 0)) return;

			#region cells
			y = HorizontalHeaderHeight - deltaY; row = firstRow;
			while ((y < viewportHeight) && (row < RowCount))
			{
				rowHeight = GetRowHeight(row);
				x = VerticalHeaderWidth - deltaX;
				for (int col = 0; col < visibleColumns; col++)
				{
					OnRenderCell(drawingContext, new Rect(x, y, ColumnWidth, rowHeight), TableViewModel?.GetCellContent(col + firstColumn, row));
					x += ColumnWidth;
				}
				row++; y += rowHeight; visibleRows++;
			}
			#endregion

			#region horizontal header
			x = VerticalHeaderWidth - deltaX;
			for (int col = 0; col < visibleColumns; col++)
			{
				
				OnRenderHorizontalHeader(drawingContext, new Rect(x, 0, ColumnWidth, HorizontalHeaderHeight), Columns?[col+firstColumn]);
				x += ColumnWidth;
			}
			#endregion

			#region vertical header
			y = HorizontalHeaderHeight - deltaY; row = firstRow;
			for (int t = 0; t < visibleRows; t++)
			{
				rowHeight = GetRowHeight(row);
				OnRenderVerticalHeader(drawingContext, new Rect(0,y, VerticalHeaderWidth,rowHeight), Rows?[row]);
				row++; y += rowHeight;
			}
			#endregion

			#region corner header
			OnRenderCornerHeader(drawingContext, new Rect(0, 0, VerticalHeaderWidth, HorizontalHeaderHeight), CornerContent);
			#endregion


		}
		#endregion

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(UpdateToolTip);
		}
		private void UpdateToolTip()
		{
			ToolTipContent= GetActivityAtPos(Mouse.GetPosition(this));
			IsToolTipVisible = ToolTipContent!=null;
		}

		#region click
		private async Task OnCornerHeaderClick(Point Position,bool ControlKey)
		{
			await Task.Yield();
		}
		private async Task OnVerticalHeaderClick(Point Position, bool ControlKey)
		{
			int index;
			CellViewModel cell;

			index = GetRowAtPos(Position);

			TableViewModel.UnSelectCells();
			if (!ControlKey) TableViewModel.UnSelectActivities();
			for(int t=0;t<ColumnCount;t++)
			{
				cell = TableViewModel.GetCellContent(t, index);
				cell.SelectAll(LayerID);
			}
			InvalidateVisual();
			await Task.Yield();
		}
		
		private async Task OnHorizontalHeaderClick(Point Position, bool ControlKey)
		{
			int index;
			CellViewModel cell;

			index = GetColumnAtPos(Position);

			TableViewModel.UnSelectCells();
			if (!ControlKey) TableViewModel.UnSelectActivities();
			for (int t = 0; t < RowCount; t++)
			{
				cell = TableViewModel.GetCellContent(index,t);
				cell.SelectAll(LayerID);
			}
			InvalidateVisual();
			await Task.Yield();
		}

		private async Task OnCellsClick(Point Position, bool ControlKey)
		{
			int column,row;
			int index;
			CellViewModel cell;
			double rowPos;
			double relativePos;

			column = GetColumnAtPos(Position);
			row = GetRowAtPos(Position);

			cell = TableViewModel.GetCellContent(column, row);

			rowPos = GetRowPosition(row) - VerticalOffset  ;
			relativePos = (Position.Y - rowPos );
			if (relativePos <= CellHeaderMargin)
			{
				await OnCellClick(cell, ControlKey);
			}
			else
			{
				index = (int)((relativePos - CellHeaderMargin) / (ActivityHeight + ActivityMargin));
				if ((index >= 0) && (index < cell.GetActivities(LayerID).Count)) await OnActivityClick(cell, index, ControlKey);
				else await OnCellClick(cell, ControlKey);
			}
		}
		private async Task OnCellClick(CellViewModel Cell, bool ControlKey)
		{
			if ((Cell.IsSelected) && (!ControlKey))
			{
				await TableViewModel.Add(Cell);
			}
			else
			{
				TableViewModel.UnSelectActivities();
				if (!ControlKey) TableViewModel.UnSelectCells();
				TableViewModel.Select(Cell);
			}
			InvalidateVisual();
		}
		private async Task OnActivityClick(CellViewModel Cell,int Index,bool ControlKey)
		{
			if (Cell.IsActivitySelected(LayerID,Index) && (!ControlKey))
			{
				await TableViewModel.Edit();
			}
			else
			{
				TableViewModel.UnSelectCells();
				if (!ControlKey) TableViewModel.UnSelectActivities();
				TableViewModel.SelectActivity(Cell, Index);
			}
			InvalidateVisual();
		}

	

		private async Task OnSelectionBeginDrag(Point Position,bool ControlKey)
		{
			if (!Mouse.Capture(this)) return;

			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
			adorner = new SelectionAdorner(this);
			adorner.PointA = clickedPoint;
			adorner.PointB = Position;
			adornerLayer.Add(adorner);
			await Task.Yield();
		}
		private async Task OnSelectionDrag(Point Position, bool ControlKey)
		{
			adorner.PointB = Position;
			await Task.Yield();
		}
		private async Task OnSelectionEndDrag(Point Position, bool ControlKey)
		{
			int column1, row1, column2, row2;
			int index1,index2;
			double x1, y1, x2, y2;
			Point pt1, pt2;
			CellViewModel cell;
			double rowPos;
			double relativePos1,relativePos2;

			Mouse.Capture(null);

			if (!ControlKey)
			{
				TableViewModel.UnSelectActivities();
			}

			adorner.GetSelectionPoints(out x1, out y1, out x2, out y2);
			pt1 = new Point(x1, y1);
			pt2 = new Point(x2, y2);

			column1 = GetColumnAtPos(pt1);
			row1 = Math.Max(0, GetRowAtPos(pt1));
			column2 = GetColumnAtPos(pt2);
			row2 = Math.Min(RowCount-1, GetRowAtPos(pt2));

			for (int row = row1; row <= row2; row++)
			{
				for (int column = column1; column <= column2; column++)
				{
					cell = TableViewModel.GetCellContent(column, row);
					rowPos = GetRowPosition(row) - VerticalOffset;
					relativePos1 = (pt1.Y - rowPos);
					relativePos2 = (pt2.Y - rowPos);

					index1 = Math.Max(0, (int)((relativePos1 - CellHeaderMargin) / (ActivityHeight + ActivityMargin)));
					index2 = Math.Min(cell.GetActivities(LayerID).Count-1,(int)((relativePos2 - CellHeaderMargin) / (ActivityHeight + ActivityMargin)));

					for (int index = index1; index <= index2; index++)
					{
						await OnActivityClick(cell, index, true);
					}
				}
			}

			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
			adornerLayer.Remove(adorner);
			adorner = null;

		}

		protected override async void OnMouseDown(MouseButtonEventArgs e)
		{
			//Point position;
			bool controlKey;

			base.OnMouseDown(e);
			if (e.LeftButton != MouseButtonState.Pressed)
			{
				selectionMode = SelectionModes.None;
				return;
			}

			Keyboard.Focus(this);

			controlKey = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

			clickedPoint = e.MouseDevice.GetPosition(this);
			if (clickedPoint.X < VerticalHeaderWidth)
			{
				if (clickedPoint.Y < HorizontalHeaderHeight) await OnCornerHeaderClick(clickedPoint, controlKey);
				else await OnVerticalHeaderClick(clickedPoint, controlKey);
			}
			else
			{
				if (clickedPoint.Y < HorizontalHeaderHeight) await OnHorizontalHeaderClick(clickedPoint, controlKey);
				else selectionMode = SelectionModes.Click;
			}
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			IsToolTipVisible = false;
			timer.Stop();
		}
		protected override async void OnMouseMove(MouseEventArgs e)
		{
			Point position;
			bool controlKey;

			base.OnMouseMove(e);

			position = e.MouseDevice.GetPosition(this);
			controlKey = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

			switch (selectionMode)
			{
				case SelectionModes.None:
					IsToolTipVisible = false;
					
					timer.Stop();
					timer.Start();

					break;
				case SelectionModes.Click:
					selectionMode = SelectionModes.Drag;
					await OnSelectionBeginDrag(position, controlKey);
					break;
				case SelectionModes.Drag:
					await OnSelectionDrag(position, controlKey);
					break;
			}

		}

		

		protected override async void OnMouseUp(MouseButtonEventArgs e)
		{
			Point position;
			bool controlKey;

			base.OnMouseDown(e);

			controlKey = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			position = e.MouseDevice.GetPosition(this);

			switch (selectionMode)
			{
				case SelectionModes.None:
					break;
				case SelectionModes.Click:
					selectionMode = SelectionModes.None;	// must be before edit in order to avoid capturing mouse during edit
					await OnCellsClick(position, controlKey);
					break;
				case SelectionModes.Drag:
					selectionMode = SelectionModes.None;
					await OnSelectionEndDrag(position, controlKey);
					break;

			}

			
		}
		#endregion

		#region mouse
		

		#endregion

		#region keyboad



		protected override void OnKeyDown(KeyEventArgs e)
		{
			RowViewModelType row;
			base.OnKeyDown(e);
			e.Handled = false;

			
			if (!e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && !e.KeyboardDevice.IsKeyDown(Key.RightCtrl) &&  (e.Key>=Key.A) && (e.Key<=Key.Z))
			{
				row=Rows.FirstOrDefault(item => item.StartsWith((char)(e.Key + 21)));
				if (row==null) return;
				SetVerticalOffset(GetRowPosition(Rows.IndexOf(row))-HorizontalHeaderHeight);
				e.Handled = true;
			}
			
				
		}
		#endregion
	}

}
