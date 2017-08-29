using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public abstract class DisableViewModelCollection<ViewModelType, ModelType> : WCFViewModelCollection<ViewModelType, ModelType>
		where ViewModelType : DisableViewModel<ModelType>
		where ModelType : ePlanifModel
	{
		public new static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register("RemoveCommand", typeof(ViewModelCommand), typeof(DisableViewModelCollection<ViewModelType, ModelType>));
		public new ViewModelCommand RemoveCommand
		{
			get { return (ViewModelCommand)GetValue(RemoveCommandProperty); }
			private set { SetValue(RemoveCommandProperty, value); }
		}


		public DisableViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
		{
			RemoveCommand = new ViewModelCommand(OnDisableCommandCanExecute, OnDisableCommandExecute);

		}

		private bool OnDisableCommandCanExecute(object Parameter)
		{
			ViewModelType item;

			item = Parameter as ViewModelType;
			if (item == null) item = SelectedItem;

			return (IsLoaded) && (item != null);
		}

		private async void OnDisableCommandExecute(object Parameter)
		{
			ViewModelType item;
			ViewModelType[] items;

			item = Parameter as ViewModelType;
			if (item == null)
			{
				items = SelectedItems.ToArray();
			}
			else
			{
				items = new ViewModelType[] { item };
			}

			
			foreach (ViewModelType viewModel in items)
			{
				viewModel.IsDisabled = !viewModel.IsDisabled;
				if (!await OnEditInModelAsync(viewModel)) viewModel.IsDisabled = !viewModel.IsDisabled;
			}
			

		}

		protected override sealed Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, ViewModelType ViewModel)
		{
			throw (new NotImplementedException());
		}


	}
}
