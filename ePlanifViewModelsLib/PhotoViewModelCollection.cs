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
			Photo photo;

			if (IsLoaded) return await System.Threading.Tasks.Task.FromResult(Model); //.Select(item=>item.Model)

			results = new List<Photo>();
			foreach(EmployeeViewModel employee in Service.Employees)
			{
				photo = await Client.GetPhotoAsync(employee.EmployeeID.Value);
				if (photo != null) results.Add(photo);
			}

			return results;
		}

		protected override Task<bool> OnAddInModelAsync(IePlanifServiceClient Client,PhotoViewModel ViewModel)
		{
			throw (new NotImplementedException());
		}
		protected override Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, PhotoViewModel ViewModel)
		{
			throw new NotImplementedException();
		}
		protected override Task<bool> OnEditInModelAsync(IePlanifServiceClient Client,PhotoViewModel ViewModel)
		{
			throw (new NotImplementedException());
		}



	}
}
