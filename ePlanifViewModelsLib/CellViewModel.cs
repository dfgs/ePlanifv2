using ePlanifModelsLib;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class CellViewModel : DependencyObject
	{

		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CellViewModel));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.Register("HasError", typeof(bool), typeof(CellViewModel));
        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }


        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(string), typeof(CellViewModel));
		public string Background
		{
			get { return (string)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}


		public static readonly DependencyProperty IsPublicHolidayProperty = DependencyProperty.Register("IsPublicHoliday", typeof(bool), typeof(CellViewModel));
		public bool IsPublicHoliday
		{
			get { return (bool)GetValue(IsPublicHolidayProperty); }
			set { SetValue(IsPublicHolidayProperty, value); }
		}




		private Dictionary<int,ObservableCollection<ActivityViewModel>> dictionary;
				

		private DateTime date;
		public DateTime Date
		{
			get { return date; }
		}

		private int rowID;
		public int RowID
		{
			get { return rowID; }
		}

		private int row;
		public int Row
		{
			get { return row; }
		}

		private int column;
		public int Column
		{
			get { return column; }
		}
		public CellViewModel(int Column,int Row, DateTime Date,int RowID,bool IsPublicHoliday )
		{
			this.column = Column;this.row = Row;
			this.date = Date;this.rowID = RowID;this.IsPublicHoliday = IsPublicHoliday;
			if ((Date.DayOfWeek == DayOfWeek.Saturday) || (Date.DayOfWeek == DayOfWeek.Sunday)) Background = "DarkGray";
			else Background = "LightGray";

			dictionary = new Dictionary<int, ObservableCollection<ActivityViewModel>>();
		}

		//protected abstract Task<ActivityViewModel> OnCreateActivityAsync();

		public string GetBackground(int LayerID)
		{
			ObservableCollection<ActivityViewModel> items;
			if (IsPublicHoliday) return "DarkGray";
			items = GetActivities(LayerID);
			return items.FirstOrDefault()?.Color?.ToString()??Background;
		}

		/*public object[] GetActivities()
		{
			return activities.ToArray();
		}*/
		public ObservableCollection<ActivityViewModel> GetActivities(int LayerID)
		{
			ObservableCollection<ActivityViewModel> items;

			if (!dictionary.TryGetValue(LayerID, out items))
			{
				items = new ObservableCollection<ActivityViewModel>();
				dictionary.Add(LayerID, items);
			}
			return items;
		}

		

		public ActivityViewModel GetActivity(int LayerID,int Index)
		{
			ObservableCollection<ActivityViewModel> items;
			items = GetActivities(LayerID);
			return items[Index];
		}

		public bool IsActivitySelected(int LayerID,int Index)
		{
			ObservableCollection<ActivityViewModel> items;
			items = GetActivities(LayerID);
			return items[Index].IsSelected;
		}

		public void InsertActivity(int LayerID, ActivityViewModel Activity)
		{
			int index ;
			ObservableCollection<ActivityViewModel> items;
			items = GetActivities(LayerID);

			index = items.Count;
			for (int t = 0; t < items.Count; t++)
			{
				//if (activities[t].IsAllDay==true) continue;
				if (Activity.Compare(items[t]) <= 0)
				{
					index = t;
					break;
				}
			}
			items.Insert(index, Activity);
			//Validate();
		}
		public void RemoveActivity(int LayerID,ActivityViewModel Activity)
		{
			ObservableCollection<ActivityViewModel> items;
			items = GetActivities(LayerID);

			items.Remove(Activity);
			//Validate();
		}
		public int FindActivity(ActivityViewModel Activity)
		{
			foreach(KeyValuePair<int,ObservableCollection<ActivityViewModel>> keyValuePair in dictionary)
			{
				if (keyValuePair.Value.Contains(Activity)) return keyValuePair.Key;
			}
			return -1;
		}

		public void ClearActivities()
		{
			dictionary.Clear();
		}


		
		public void SelectAll(int LayerID)
		{
			ObservableCollection<ActivityViewModel> items;
			items = GetActivities(LayerID);
			foreach (ActivityViewModel activity in items)
			{
				activity.IsSelected = true;
			}
		}

		public void UnSelectAll(int LayerID)
		{
			ObservableCollection<ActivityViewModel> items;
			items = GetActivities(LayerID);
			foreach (ActivityViewModel activity in items)
			{
				activity.IsSelected = false;
			}
		}

		
	}
}
