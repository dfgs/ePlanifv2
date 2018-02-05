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

namespace ePlanifViewModelsLib
{
	public delegate void CellFocusedEventHandler(DependencyObject sender, int Column,int Row);

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


		public event EventHandler Updated;
		public event CellFocusedEventHandler CellFocused;

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

		}

		protected abstract bool IsActivityBindedTo (ActivityViewModel Activity, MemberViewModelType Row);
		protected abstract void SetRowID(ActivityViewModel Activity, int RowID);
		protected abstract bool OnValidateCell(CellViewModel Cell);
		protected abstract int GetLayerID(ActivityViewModel Activity);
		protected abstract bool GetIsPublicHolyday(DateTime Data, int Row);
		protected abstract bool HasWriteAccessOnRow(int Row);

		protected abstract int CompareActivities(ActivityViewModel A, ActivityViewModel B);

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
			if (sender == this) return;
			Tuple<CellViewModel, int> pair = FindCellForActivity(Activity);
			if ((pair == null) || (pair.Item2 != this.LayerID)) return;
			OnCellFocused(pair.Item1.Column, pair.Item1.Row);
		}

		public void Select(CellViewModel Cell)
		{
			Cell.IsSelected = !Cell.IsSelected;
		}
		public void UnSelectCells()
		{
			this.SelectedCell = null;
		}
		public void SelectActivity(CellViewModel Cell,int ActivityIndex)
		{
			CellViewModel cell;
			ActivityViewModel activity;

			cell = Cell as CellViewModel;
			activity = cell.GetActivity(LayerID,ActivityIndex);
			activity.IsSelected = !activity.IsSelected;
			if (activity.IsSelected) ActivitySelectionManager.OnActivitySelected(this, activity);
		}
		public void UnSelectActivities()
		{
			Service.Activities.SelectedItem = null;
		}
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
			OnUpdated();
		}
		private void Activities_ActivityRemoved(object sender, ActivityViewModel Activity)
		{
			RemoveActivity(Activity);
			OnUpdated();
		}
		private void Activities_ActivityEdited(object sender, ActivityViewModel Activity)
		{
			MoveActivity(Activity);
			OnUpdated();
		}



		protected virtual void OnUpdated()
		{
			if (Updated != null) Updated(this, EventArgs.Empty);
		}
		protected virtual void OnCellFocused(int Column,int Row)
		{
			if (CellFocused != null) CellFocused(this,Column,Row);
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
