using ePlanifModelsLib;
using ePlanifViewModelsLib.ePlanifService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ePlanifViewModelsLib
{
	public delegate void ActivityEditedHandler(DependencyObject sender,ActivityViewModel Activity);

    public class ActivityViewModelCollection : WCFViewModelCollection<ActivityViewModel, Activity>
    {

		public override bool UseDiff
		{
			get { return false; }
		}

		public event ActivityEditedHandler ActivityAdded;
		public event ActivityEditedHandler ActivityRemoved;
		public event ActivityEditedHandler ActivityEdited;
		//public event ActivityEditedHandler ActivityFocused;

		public ActivityViewModelCollection(ePlanifServiceViewModel Service) : base(Service)
        {
			
		}

		public async Task<ActivityViewModel> CreateActivityAsync(DateTime Date,int RowID)
		{
			ActivityViewModel result;
			result=await OnCreateViewModelItem(typeof(Activity));
			await result.LoadAsync( await OnCreateEmptyModelAsync());
			
			return result;
		}
		protected override Task<Activity> OnCreateEmptyModelAsync()
        {
            return Task.FromResult(new Activity() { StartDate=Service.StartDate.AddHours(8),Duration=TimeSpan.FromHours(1) });
        }

        protected override Task<ActivityViewModel> OnCreateViewModelItem(Type ModelType)
        {
            return Task.FromResult(new ActivityViewModel(Service));
        }


		protected override async Task<IEnumerable<Activity>> OnLoadModelAsync(IePlanifServiceClient Client)
		{
			List<Activity> result;
			result = new List<Activity>();
			for(int t=0;t<Service.DaysCount;t++)
			{
				result.AddRange( await Client.GetActivitiesAsync(Service.StartDate.AddDays(t)) );
			}
			return result;
		}

		protected override async Task<bool> OnAddInModelAsync(IePlanifServiceClient Client, ActivityViewModel ViewModel)
		{
			ViewModel.ActivityID= await Client.CreateActivityAsync(ViewModel.Model);
			return ViewModel.ActivityID > 0;
		}

		protected override async Task<bool> OnRemoveFromModelAsync(IePlanifServiceClient Client, ActivityViewModel ViewModel)
		{
			return await Client.DeleteActivityAsync(ViewModel.ActivityID.Value);
		}

		protected override async Task<bool> OnEditInModelAsync(IePlanifServiceClient Client, ActivityViewModel ViewModel)
		{
			return await Client.UpdateActivityAsync(ViewModel.Model);
		}

		public async Task<bool> BulkDeleteAsync(DateTime StartDate, DateTime EndDate, int EmployeeID)
		{
			using (IePlanifServiceClient client = Service.CreateClient())
			{
				return await client.BulkDeleteActivitiesAsync(StartDate, EndDate, EmployeeID);	
			}
		}



		protected override Task OnItemAddedAsync(ActivityViewModel Item, int Index)
		{
			if (ActivityAdded != null) ActivityAdded(this, Item);
			return base.OnItemAddedAsync(Item, Index);
		}

		

		protected override Task OnItemRemovedAsync(ActivityViewModel Item, int Index)
		{
			if (ActivityRemoved != null) ActivityRemoved(this, Item);
			return base.OnItemRemovedAsync(Item, Index);
		}
		

		public override async Task OnEditCommandExecuted(ActivityViewModel ViewModel)
		{
			if (ActivityEdited != null) ActivityEdited(this, ViewModel);
				await Task.Yield();
		}

		public async Task<bool> UpdateAsync(ActivityViewModel Activity)
		{
			bool result;
			result= await OnEditInModelAsync(Activity);
			if (ActivityEdited != null) ActivityEdited(this, Activity);
			return result;
		}

		public async Task<bool> HasWriteAccessAsync()
		{
			IePlanifServiceClient client = Service.CreateClient();
			if (client == null) return false;
			using (client)
			{
				foreach (ActivityViewModel activity in Service.Activities.SelectedItems)
				{
					if (!((await client.HasWriteAccessToActivityAsync(activity.ActivityID.Value)))) return false;
				}
			}
			return true;
		}

		protected override Window OnCreateEditWindow()
		{
			EditActivityWindow window= new EditActivityWindow();
			window.ActivityTypes = Service.ActivityTypes;
			return  window;
		}



	}
}
