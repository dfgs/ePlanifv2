using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public abstract class WCFViewModelCollection<ViewModelType, ModelType> : ViewModelCollection<IEnumerable<ModelType>,ViewModelType,ModelType>
		where ViewModelType:WCFViewModel<ModelType>
	{
		private ePlanifServiceViewModel service;
		public ePlanifServiceViewModel Service
		{
			get { return service; }
		}

		public WCFViewModelCollection(ePlanifServiceViewModel Service)
		{
			this.service = Service;
		}

		protected abstract Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, ViewModelType ViewModel);
		protected abstract Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, ViewModelType ViewModel);
		protected abstract Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, ViewModelType ViewModel);
		protected abstract Task<IEnumerable<ModelType>> OnLoadModelAsync(IePlanifServiceClient Client);

		protected sealed override async Task<bool> OnAddInModelAsync(ViewModelType ViewModel)
		{
			IePlanifServiceClient client = Service.CreateClient();
			if (client == null) return false;
			using (client)
			{
				return await OnAddInModelAsync(client, ViewModel);
			}
		}
		protected sealed override async Task<bool> OnEditInModelAsync(ViewModelType ViewModel)
		{
			IePlanifServiceClient client = Service.CreateClient();
			if (client == null) return false;
			using (client)
			{
				return await OnEditInModelAsync(client, ViewModel);
			}
		}
		protected sealed override async Task<bool> OnRemoveFromModelAsync(ViewModelType ViewModel)
		{
			IePlanifServiceClient client = Service.CreateClient();
			if (client == null) return false;
			using (client)
			{
				return await OnRemoveFromModelAsync(client, ViewModel);
			}
		}
		protected sealed override async Task<IEnumerable<ModelType>> OnLoadModelAsync()
		{
			IePlanifServiceClient client = Service.CreateClient();
			if (client == null) return null;
			using (client)
			{
				return await OnLoadModelAsync(client);
			}
		}


	}
}
