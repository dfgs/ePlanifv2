using ePlanifModelsLib;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;
using System.Collections;
using Nager.Date;
using ePlanifViewModelsLib.Commands;
using ClosedXML.Excel;
using System.Collections.ObjectModel;

namespace ePlanifViewModelsLib
{
	public delegate void CellEventHandler(DependencyObject sender, int Column,int Row);

	public abstract class ViewViewModel<ViewModelType,MemberViewModelType> : WCFViewModel<ViewModelType>, IEnumerable<CellViewModel>,IViewViewModel
		where MemberViewModelType:IRowViewModel
	{
		#region commands
		public static readonly DependencyProperty CopyCommandProperty = DependencyProperty.Register("CopyCommand", typeof(ViewModelCommand), typeof(ViewViewModel<ViewModelType, MemberViewModelType>));
		public ViewModelCommand CopyCommand
		{
			get { return (ViewModelCommand)GetValue(CopyCommandProperty); }
			private set { SetValue(CopyCommandProperty, value); }
		}
		public static readonly DependencyProperty CutCommandProperty = DependencyProperty.Register("CutCommand", typeof(ViewModelCommand), typeof(ViewViewModel<ViewModelType, MemberViewModelType>));
		public ViewModelCommand CutCommand
		{
			get { return (ViewModelCommand)GetValue(CutCommandProperty); }
			private set { SetValue(CutCommandProperty, value); }
		}
		public static readonly DependencyProperty PasteCommandProperty = DependencyProperty.Register("PasteCommand", typeof(ViewModelCommand), typeof(ViewViewModel<ViewModelType, MemberViewModelType>));
		public ViewModelCommand PasteCommand
		{
			get { return (ViewModelCommand)GetValue(PasteCommandProperty); }
			private set { SetValue(PasteCommandProperty, value); }
		}
		public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register("DeleteCommand", typeof(ViewModelCommand), typeof(ViewViewModel<ViewModelType, MemberViewModelType>));
		public ViewModelCommand DeleteCommand
		{
			get { return (ViewModelCommand)GetValue(DeleteCommandProperty); }
			private set { SetValue(DeleteCommandProperty, value); }
		}
		#endregion


		public event CellEventHandler Updated;
		public event CellEventHandler CellFocused;
		public event CellEventHandler CellSelectionChanged;
		public event CellEventHandler ActivitySelectionChanged;

		private CellViewModel[,] cells;

		public abstract IViewModelCollection<MemberViewModelType> VisibleMembers
		{
			get;
		}

		private int columnCount;
		
		private int rowCount;
	


		public CellViewModel SelectedCell
		{
			get
			{
				return this.FirstOrDefault(item => item.IsSelected);
			}
			set
			{
				foreach (CellViewModel cell in this)
				{
					cell.IsSelected = false;
				}
				if (value!=null) value.IsSelected = true;
			}
		}

		public bool HasActivitySelected
		{
			get { return Service.Activities.SelectedItem != null; }
		}

		public abstract int LayerID
		{
			get;
		}

	
	


		public ViewViewModel(ePlanifServiceViewModel Service):base(Service)
		{			

			CopyCommand = new ViewModelCommand(OnCopyCommandCanExecute, OnCopyCommandExecute);
			CutCommand = new ViewModelCommand(OnCutCommandCanExecute, OnCutCommandExecute);
			PasteCommand = new ViewModelCommand(OnPasteCommandCanExecute, OnPasteCommandExecute);
			DeleteCommand = new ViewModelCommand(OnDeleteCommandCanExecute, OnDeleteCommandExecute);


			Service.Activities.ActivityAdded+= Activities_ActivityAdded;
			Service.Activities.ActivityRemoved += Activities_ActivityRemoved;
			Service.Activities.ActivityEdited += Activities_ActivityEdited;
			//Service.Activities.ActivityFocused += Activities_ActivityFocused;

			ActivitySelectionManager.ActivitySelected += ViewViewModel_ActivitySelected;
			ActivitySelectionManager.ActivityUnselected += ViewViewModel_ActivityUnSelected;
		}

		protected abstract bool IsActivityBindedTo (ActivityViewModel Activity, MemberViewModelType Row);
		protected abstract void SetRowID(ActivityViewModel Activity, int RowID);
		protected abstract bool OnValidateCell(CellViewModel Cell);
		protected abstract int GetLayerID(ActivityViewModel Activity);
		protected abstract bool GetIsPublicHolyday(DateTime Data, int Row);
		protected abstract bool HasWriteAccessOnRow(int Row);

		protected abstract int CompareActivities(ActivityViewModel A, ActivityViewModel B);
		protected abstract string OnGetActivityDisplay(ActivityViewModel Activity);

		public void ExportToExcel(string FileName)
		{
			IXLWorksheet worksheet;
			XLWorkbook workbook;
			IXLStyle xlStyle;
			int maxLines;
			int line;
			ObservableCollection<ActivityViewModel> activities;
			
			xlStyle =  XLWorkbook.DefaultStyle;
			//xlStyle.Font.FontColor = XLColor.FromArgb(255, 0, 0, 0);
			//xlStyle.Font.FontName = "Arial";
			//xlStyle.Font.FontSize = 12;
			//xlStyle.Font.Bold = false;
			//xlStyle.Font.Italic = false;
			
			workbook = new XLWorkbook();
			worksheet = workbook.Worksheets.Add("Activities");

			// Corner header
			worksheet.Column(1).Width = 25;
			worksheet.Cell(1, 1).SetValue(Service.WeekName)
				.Style
				.Border.SetBottomBorder( XLBorderStyleValues.Thin)
				.Border.SetTopBorder(XLBorderStyleValues.Thin)
				.Border.SetLeftBorder(XLBorderStyleValues.Thin)
				.Border.SetRightBorder(XLBorderStyleValues.Thin)
				.Fill.SetBackgroundColor(XLColor.LightSteelBlue);

			// horizontal header
			for (int col = 0; col < this.columnCount; col++)
			{
				worksheet.Column(col + 2).Width = 25;
				worksheet.Cell(1, col + 2).SetValue(cells[col,0].Date)
					.Style
					.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
					.Border.SetBottomBorder(XLBorderStyleValues.Thin)
					.Border.SetTopBorder(XLBorderStyleValues.Thin)
					.Border.SetLeftBorder(XLBorderStyleValues.Thin)
					.Border.SetRightBorder(XLBorderStyleValues.Thin)
					.Fill.SetBackgroundColor(XLColor.LightSteelBlue);

			}




			line = 2;
			for (int row = 0; row < rowCount; row++)
			{
				#region get max number of line
				maxLines = 1;
				for (int col = 0; col < this.columnCount; col++)
				{
					activities = cells[col, row].GetActivities(LayerID);
					if (activities.Count > maxLines) maxLines = activities.Count;
				}
				#endregion

				#region vertical header
				worksheet.Cell(line, 1).SetValue(VisibleMembers[row].ToString())
					.Style
					.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
					.Border.SetBottomBorder(XLBorderStyleValues.Thin)
					.Border.SetTopBorder(XLBorderStyleValues.Thin)
					.Border.SetLeftBorder(XLBorderStyleValues.Thin)
					.Border.SetRightBorder(XLBorderStyleValues.Thin)
					.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
				worksheet.Range(line, 1, line + maxLines - 1, 1).Merge();
				#endregion

				for (int col = 0; col < this.columnCount; col++)
				{
					activities = cells[col, row].GetActivities(LayerID);
					
					for (int t = 0; t < activities.Count; t++)
					{
						worksheet.Cell(line + t, col + 2).SetValue(OnGetActivityDisplay(activities[t]))
							.Style
							.Fill.SetBackgroundColor(XLColor.FromName(activities[t].ActivityType.BackgroundColor.Value.Value ))
							.Font.SetFontColor(XLColor.FromName(activities[t].ActivityType.TextColor.Value.Value))
							.Border.SetTopBorder(XLBorderStyleValues.Thin)
							.Border.SetBottomBorder(XLBorderStyleValues.Thin)
							.Border.SetLeftBorder(XLBorderStyleValues.Thin)
							.Border.SetRightBorder(XLBorderStyleValues.Thin);
					}
					if (activities.Count < maxLines)
					{
						worksheet.Range(line+activities.Count, col+2, line + maxLines - 1, col+2).Merge()
							.Style.Fill.SetBackgroundColor(XLColor.FromName(cells[col, row].Background))
							.Border.SetTopBorder(XLBorderStyleValues.Thin)
							.Border.SetBottomBorder(XLBorderStyleValues.Thin)
							.Border.SetLeftBorder(XLBorderStyleValues.Thin)
							.Border.SetRightBorder(XLBorderStyleValues.Thin);
						
					}

				}
				
				line += maxLines;
			}


			workbook.SaveAs(FileName);
		}



		protected bool Overlap(ActivityViewModel A, ActivityViewModel B)
		{
			if ((A.StartTime >= B.StartTime) && (A.StartTime < B.StopTime) ) return true;
			if ((B.StartTime >= A.StartTime) && (B.StartTime < A.StopTime) ) return true;
			return false;
		}

		protected override async Task OnLoadedAsync()
		{
			bool writeAccess;
			await base.OnLoadedAsync();


			columnCount = Service.Days.Count;rowCount = VisibleMembers.Count;
			this.cells = new CellViewModel[columnCount, rowCount];
			for(int r=0;r<rowCount;r++)
			{
				writeAccess = HasWriteAccessOnRow(r);
				for(int c=0;c<columnCount;c++)
				{
					cells[c, r] = new CellViewModel(c,r, Service.Days[c].Date,VisibleMembers[r].RowID,GetIsPublicHolyday(Service.Days[c].Date,r),writeAccess);
				}
			}
			OnPropertyChanged("Cells");

			foreach (ActivityViewModel activity in Service.Activities)
			{
				AddActivity(activity);
			}
			foreach(CellViewModel cell in cells)
			{
				ValidateCell(cell);
			}

		}

		public IEnumerator<CellViewModel> GetEnumerator()
		{
			for(int r=0;r<rowCount;r++)
			{
				for(int c=0;c<columnCount;c++)
				{
					yield return cells[c, r];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void ValidateCell(CellViewModel Cell)
		{
			Cell.HasError = OnValidateCell(Cell);
			
		}


		public CellViewModel GetCellContent(int Col, int Row)
		{
			if ((Col >= columnCount) || (Row >= rowCount) || (Col <0) || (Row <0)) return null;
			return cells[Col, Row];
		}

		/*public IRowViewModel GetRowContent(int Index)
		{
			return VisibleMembers[Index];
		}
		public DayViewModel GetColumnContent(int Index)
		{
			return Service.Days[Index];
		}


		public object GetCornerContent()
		{
			return Service.WeekName;
		}*/

		private void ViewViewModel_ActivitySelected(DependencyObject sender, ActivityViewModel Activity)
		{

			Tuple<CellViewModel, int> pair = FindCellForActivity(Activity);
			if ((pair == null) || (pair.Item2 != this.LayerID)) return;
			if (sender == this) OnActivitySelected(GetColumnIndex(Activity), GetRowIndex(Activity));
			else OnCellFocused(pair.Item1.Column, pair.Item1.Row);
		}
		private void ViewViewModel_ActivityUnSelected(DependencyObject sender, ActivityViewModel Activity)
		{
			Tuple<CellViewModel, int> pair = FindCellForActivity(Activity);
			if ((pair == null) || (pair.Item2 != this.LayerID)) return;
			OnActivitySelected(GetColumnIndex(Activity), GetRowIndex(Activity));
		}

		public void Select(CellViewModel Cell)
		{
			Cell.IsSelected = !Cell.IsSelected;
			OnCellSelected(Cell.Column, Cell.Row);
		}

		/*public IEnumerable<int> GetSelectedCellRows()
		{
			foreach(CellViewModel cell in cells)
			{
				if (cell.IsSelected) yield return cell.Row;
			}
		}*/

		public void UnSelectCells()
		{
			if (this.SelectedCell != null) OnCellSelected(SelectedCell.Column, SelectedCell.Row);
			this.SelectedCell = null;
		}
		public void SelectActivity(CellViewModel Cell,int ActivityIndex)
		{
			CellViewModel cell;
			ActivityViewModel activity;

			cell = Cell as CellViewModel;
			activity = cell.GetActivity(LayerID,ActivityIndex);
			activity.IsSelected = !activity.IsSelected;
			if (activity.IsSelected) ActivitySelectionManager.OnActivitySelected(this,activity);
			else ActivitySelectionManager.OnActivityUnselected(this,activity);
		}
		public void UnSelectActivities()
		{
			foreach (ActivityViewModel activity in Service.Activities.SelectedItems)
			{
				activity.IsSelected = false;
				ActivitySelectionManager.OnActivityUnselected(this, activity);
			}
			Service.Activities.SelectedItem = null;
		}

		
		/*public IEnumerable<int> GetSelectedActivitiesRows()
		{
			return Service.Activities.SelectedItems.Select(item => GetRowIndex(item));
		}*/


		public async Task Edit()
		{
			foreach(ActivityViewModel activity in Service.Activities.SelectedItems)
			{
				if (activity.Employee.WriteAccess != true) return;
			}
			
			await Service.CommandManager.ExecuteAsync(new EditCommand<ActivityViewModel, Activity>(Service.Activities, Service.Activities.SelectedItems.ToArray()));
			//await Service.Activities.EditAsync();	
		}
		public async Task Add(CellViewModel Cell)
		{
			ActivityViewModel vm;
			
			if (HasWriteAccessOnRow(Cell.Row))
			{
				vm = await Service.Activities.CreateActivityAsync(Cell.Date, Cell.RowID);
				vm.Date = Cell.Date;
				SetRowID(vm, Cell.RowID);

				await Service.CommandManager.ExecuteAsync(new AddCommand<ActivityViewModel, Activity>(Service.Activities, vm));
				//await Service.Activities.AddAsync(vm, true);
			}
		}

		public int GetColumnIndex(ActivityViewModel Activity)
		{
			return (Activity.Date.Value.Date - Service.StartDate).Days;
		}


		/*public int GetColumn(CellViewModel Cell)
		{
			return (Cell.StartDate.Date - database.StartDate).Days;
		}*/
		private  int GetRowIndex(ActivityViewModel Activity)
		{
			
			for(int t=0;t<rowCount;t++)
			{
				if (IsActivityBindedTo(Activity,VisibleMembers[t])) return t;
			}
			return -1;
		}

		public CellViewModel GetCell(ActivityViewModel Activity)
		{
			int col, row;
			col = GetColumnIndex(Activity); row = GetRowIndex(Activity);
			if ((col < 0) || (row < 0) || (col >= columnCount) || (row >= rowCount)) return null;
			return cells[col, row]; 
		}

		public Tuple<CellViewModel,int> FindCellForActivity(ActivityViewModel Activity)
		{
			int layerID;
			foreach (CellViewModel cell in this.cells)
			{
				layerID = cell.FindActivity(Activity);
				if (layerID>=0) return new Tuple<CellViewModel,int>( cell,layerID);
			}
			return null;
		}


		public void AddActivity(ActivityViewModel Activity)
		{
			CellViewModel cell;
			cell = GetCell(Activity);
			if (cell != null)
			{
				cell.InsertActivity(GetLayerID(Activity), Activity,CompareActivities);
				ValidateCell(cell);
			}
		}
		public void RemoveActivity(ActivityViewModel Activity)
		{
			CellViewModel cell;
			cell = GetCell(Activity);
			if (cell != null)
			{
				cell.RemoveActivity(GetLayerID(Activity), Activity);
				ValidateCell(cell);
			}
		}

		public void MoveActivity(ActivityViewModel Activity)
		{
			//CellViewModel currentCell;
			CellViewModel newCell;
			Tuple<CellViewModel, int> t;

			t = FindCellForActivity(Activity);
			if (t != null)
			{
				t.Item1.RemoveActivity(t.Item2,Activity);
				ValidateCell(t.Item1);
			}

			newCell = GetCell(Activity);
			if (newCell != null)
			{
				newCell.InsertActivity(GetLayerID(Activity), Activity,CompareActivities);
				ValidateCell(newCell);
			}
		}


		public async Task<bool> ReplicateActivitiesAsync(DateTime EndDate,bool SkipPublicHolidays)
		{
			int counter;
			DateTime currentDate;
			ActivityViewModel newActivity;
			DateTime newDate;
			ActivityViewModel[] activities;
			bool result;

			activities = Service.Activities.SelectedItems.ToArray();
			currentDate = activities.Min(item => item.Date).Value;
			counter = 7;
			while (currentDate <= EndDate)
			{
				foreach (ActivityViewModel activity in activities)
				{
					newDate = activity.Date.Value.AddDays(counter);
					if (newDate > EndDate) continue;

					if ((SkipPublicHolidays) && (DateSystem.IsPublicHoliday(newDate, activity.Employee.GetCountryCode()))) continue;
					

					newActivity = await activity.CloneAsync();
					newActivity.Date = newDate;

					result=await Service.Activities.AddAsync(newActivity,false);
				}

				counter += 7;
				currentDate = currentDate.AddDays(7);
			}
			//Update();
			return true;
		}




		private void Activities_ActivityAdded(object sender, ActivityViewModel Activity)
		{
			AddActivity(Activity);
			OnUpdated(GetColumnIndex(Activity),  GetRowIndex(Activity) );
		}
		private void Activities_ActivityRemoved(object sender, ActivityViewModel Activity)
		{
			RemoveActivity(Activity);
			OnUpdated(GetColumnIndex(Activity), GetRowIndex(Activity));
		}
		private void Activities_ActivityEdited(object sender, ActivityViewModel Activity)
		{
			MoveActivity(Activity);
			OnUpdated(GetColumnIndex(Activity), GetRowIndex(Activity));
		}



		protected virtual void OnUpdated(int Column, int Row)
		{
			if (Updated != null) Updated(this, Column,Row);
		}
		protected virtual void OnCellFocused(int Column,int Row)
		{
			if (CellFocused != null) CellFocused(this,Column,Row);
		}
		protected virtual void OnCellSelected(int Column, int Row)
		{
			if (CellSelectionChanged != null) CellSelectionChanged(this, Column, Row);
		}
		
		protected virtual void OnActivitySelected(int Column, int Row)
		{
			if (ActivitySelectionChanged != null) ActivitySelectionChanged(this, Column, Row);
		}
		

		private bool OnCopyCommandCanExecute(object Parameter)
		{
			return Service.Activities.SelectedItem != null;
		}
		private void OnCopyCommandExecute(object Parameter)
		{
			Clipboard.Copy(Service.Activities.SelectedItems);
		}
		private bool OnCutCommandCanExecute(object Parameter)
		{
			return (Service.Activities.SelectedItem != null) && (Service.Activities.Select(item=>item.Employee).All(item=>item?.WriteAccess==true));
		}
		private void OnCutCommandExecute(object Parameter)
		{
			Clipboard.Cut(Service.Activities.SelectedItems);
		}
		private bool OnPasteCommandCanExecute(object Parameter)
		{
			return (Clipboard.CanPaste) && (SelectedCell!=null) && (SelectedCell.WriteAccess);
		}
		private async void OnPasteCommandExecute(object Parameter)
		{
			TimeSpan delta;
			ActivityViewModel newActivity;
			DateTime minItemsDate;
			CellViewModel cell;

			cell = SelectedCell;

			minItemsDate= Clipboard.Items.Min(item => item.Date.Value);
			delta = cell.Date - minItemsDate;

			if (Clipboard.IsCutting)
			{
				foreach (ActivityViewModel activity in Clipboard.Items)
				{
					activity.IsSelected = false;

					newActivity = await activity.CloneAsync();
					newActivity.IsSelected = true;
					newActivity.Date = activity.Date + delta;
					SetRowID(newActivity, cell.RowID);

					await Service.CommandManager.ExecuteAsync(new CutPasteCommand<ActivityViewModel, Activity>(Service.Activities,activity,newActivity));
				}
				Clipboard.Clear();
			}
			else
			{
				foreach (ActivityViewModel activity in Clipboard.Items)
				{
					activity.IsSelected = false;

					newActivity = await activity.CloneAsync();
					newActivity.IsSelected = true;
					newActivity.Date = activity.Date+delta;
					SetRowID(newActivity, cell.RowID);

					await Service.CommandManager.ExecuteAsync(new CopyPasteCommand<ActivityViewModel,Activity>(Service.Activities, newActivity));
					//await Service.Activities.AddAsync(newActivity,false);
				}
			}
			//OnUpdated();

		}
		private bool OnDeleteCommandCanExecute(object Parameter)
		{
			return (Service.Activities.SelectedItem != null) && (Service.Activities.SelectedItem?.Employee?.WriteAccess==true);
		}
		private async void OnDeleteCommandExecute(object Parameter)
		{
			foreach (ActivityViewModel activity in Service.Activities.SelectedItems.ToArray())
			{
				//await Service.Activities.RemoveAsync(activity);
				await Service.CommandManager.ExecuteAsync(new RemoveCommand<ActivityViewModel, Activity>(Service.Activities, activity));
			}
		}

		public ActivityViewModel SearchProject(string Reference, ActivityViewModel CurrentActivity)
		{

			foreach(CellViewModel cell in cells)
			{
				foreach(ActivityViewModel activity in cell.GetActivities(LayerID))
				{
					if ((activity.ProjectNumber.ToString() == Reference) || (activity.RemedyRef.ToString() == Reference) || (activity.Comment.ToString().Contains(Reference)))
					{
						if (CurrentActivity==null) return activity;
						if (CurrentActivity==activity) CurrentActivity = null;	// skip current activity
					}

				}
			}

			return null;
		}
		public void Focus(ActivityViewModel Activity)
		{
			SelectedCell = null;
			Service.Activities.SelectedItem = Activity;
			
			OnCellFocused(GetColumnIndex(Activity), GetRowIndex(Activity));
		}

		
	}
}
