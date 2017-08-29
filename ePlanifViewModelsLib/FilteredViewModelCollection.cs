using ModelLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class FilteredViewModelCollection<ViewModelType,ModelType>:ViewModel<List<ViewModelType>>, IViewModelCollection<ViewModelType>
		where ViewModelType:WCFViewModel<ModelType>
	{

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(ViewModelType), typeof(FilteredViewModelCollection<ViewModelType, ModelType>));
		public ViewModelType SelectedItem
		{
			get { return (ViewModelType)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		private ViewModelType savedSelectedItem;

		private WCFViewModelCollection<ViewModelType, ModelType> collection;
		private Func<ViewModelType, bool> predicate;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public int Count
		{
			get
			{
				if (Model == null) return 0;
				else return Model.Count;
			}
		}

		public ViewModelType this[int Index]
		{
			get
			{
				return Model[Index];
			}
		}

		public FilteredViewModelCollection(WCFViewModelCollection<ViewModelType, ModelType> Collection,Func<ViewModelType,bool> Predicate)
		{
			this.collection = Collection;this.predicate = Predicate;
			
		}

		public int IndexOf(ViewModelType Item)
		{
			if (Model == null) return -1;
			return Model.IndexOf(Item);
		}

		public IEnumerator<ViewModelType> GetEnumerator()
		{
			if (Model == null) yield break;
			foreach(ViewModelType item in Model) yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		protected override Task<bool> OnLoadingAsync()
		{
			savedSelectedItem = SelectedItem;
			return base.OnLoadingAsync();
		}
		protected override Task<List<ViewModelType>> OnLoadModelAsync()
		{
			List<ViewModelType> items;
			items = new List<ViewModelType>();
			items.AddRange(collection.Where(predicate));
			return Task.FromResult(items);
		}
		protected override Task OnLoadedAsync()
		{
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			OnPropertyChanged("Count");
			if (savedSelectedItem == null) SelectedItem = this.FirstOrDefault();
			else SelectedItem = this.FirstOrDefault(item => item.IsModelEqualTo(savedSelectedItem.Model));

			return base.OnLoadedAsync();
		}
	}
}
