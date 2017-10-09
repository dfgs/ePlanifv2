using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class PhotoViewModelCollection : WCFViewModelCollection<PhotoViewModel, Photo>
    {
        public PhotoViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}
		protected override Task<Photo> OnCreateEmptyModelAsync()
		{
			return Task.FromResult(new Photo() { });
		}

		protected override Task<PhotoViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return Task.FromResult(new PhotoViewModel(Service));
		}
		

		protected override async Task<IEnumerable<Photo>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			List<Photo> results;

			if (IsLoaded) return await System.Threading.Tasks.Task.FromResult(Model); //.Select(item=>item.Model)

			results = new List<Photo>();
			foreach(EmployeeViewModel employee in Service.Employees)
			{
				results.AddRange( await Client.GetPhotosAsync(employee.EmployeeID.Value));
			}

			return results;
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client,PhotoViewModel ViewModel)
		{
			ViewModel.PhotoID= await Client.CreatePhotoAsync(ViewModel.Model);
			return ViewModel.PhotoID > 0;
		}
		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, PhotoViewModel ViewModel)
		{
			return await Client.DeletePhotoAsync(ViewModel.Model.PhotoID.Value);
		}
		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client,PhotoViewModel ViewModel)
		{
			return await Client.UpdatePhotoAsync(ViewModel.Model);
		}



	}
}
