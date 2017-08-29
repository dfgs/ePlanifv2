using ePlanifViewModelsLib.ePlanifService;
using ePlanifViewModelsLib.ReportingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib
{
	public class ReportViewModelCollection : WCFViewModelCollection<ReportViewModel, string>
	{

		public ReportViewModelCollection(ePlanifServiceViewModel Service):base(Service)
		{
			
		}

		protected override async Task<string> OnCreateEmptyModelAsync()
		{
			return await System.Threading.Tasks.Task.FromResult("New report");
		}

		protected override async Task<ReportViewModel> OnCreateViewModelItem(Type ModelType)
		{
			return await System.Threading.Tasks.Task.FromResult(new ReportViewModel(Service));
		}


		protected override async Task<IEnumerable<string>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			//List<string> results;

			if (Service.UserProfile.CanRunReports == false) return new string[0];
			if (IsLoaded) return await System.Threading.Tasks.Task.FromResult(Model); //.Select(item=>item.Model)

			return await System.Threading.Tasks.Task.FromResult(new string[0]);
			/*return await System.Threading.Tasks.Task.Run<IEnumerable<string>>(() =>
			{
				results = new List<string>();
				ePlanifViewModelsLib.ReportingServices.ReportingService2010 rs;
				rs = new ReportingService2010();
				rs.Url = "http://" + Service.Server + "/ReportServer/ReportService2010.asmx";
				rs.Credentials = System.Net.CredentialCache.DefaultCredentials;

				CatalogItem[] items;
				try
				{
					items = rs.ListChildren("/ePlanif_Reports", false);
				}
				catch
				{
					return results;
				}
				foreach (CatalogItem item in items)
				{
					if (item.TypeName != "Report") continue;
					Dispatcher.Invoke(() => { results.Add(item.Name); });
				}
				return results;
			});*/
		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, ReportViewModel ViewModel)
		{
			return await System.Threading.Tasks.Task.FromResult(false);
			
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, ReportViewModel ViewModel)
		{
			return await System.Threading.Tasks.Task.FromResult(false);
			
		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, ReportViewModel ViewModel)
		{
			return await System.Threading.Tasks.Task.FromResult(false);
			

		}

	}
}
