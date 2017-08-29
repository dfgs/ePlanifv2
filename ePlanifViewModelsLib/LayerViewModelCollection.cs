using ePlanifModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;
using ModelLib;
using ePlanifViewModelsLib.ePlanifService;

namespace ePlanifViewModelsLib
{
    public class LayerViewModelCollection : DisableViewModelCollection<LayerViewModel, Layer>
    {
        public LayerViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}
		protected override Task<Layer> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new Layer() { });
		}

		protected override Task<LayerViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new LayerViewModel(Service));
		}

		protected override async Task<IEnumerable<Layer>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			return await Client.GetLayersAsync();
			
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, LayerViewModel ViewModel)
		{
			ViewModel.LayerID = await Client.CreateLayerAsync(ViewModel.Model);
			return ViewModel.LayerID > 0;
			
		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, LayerViewModel ViewModel)
		{
			return await Client.UpdateLayerAsync(ViewModel.Model);
			
			

		}




	}
}
