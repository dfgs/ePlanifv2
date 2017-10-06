using DatabaseModelLib;
using DatabaseModelLib.Filters;
using ePlanifModelsLib;
using LogUtils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.ServiceModel;
using System.Threading.Tasks;
using WorkerLib;

namespace ePlanifServerLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // necessary for self host
	public class ePlanifService : Worker, IePlanifService
	{
		private IDataProvider dataProvider;

		private ePlanifPrincipal Principal
		{
			get { return (ePlanifPrincipal)System.Threading.Thread.CurrentPrincipal; ; }
		}
		public ePlanifService(IDataProvider DataProvider) : base("ePlanifService")
		{
			dataProvider = DataProvider;
		}


		private bool AssertPermission(string Role, [CallerMemberName]string CallerName = null)
		{
			if (Principal == null)
			{
				WriteLog(LogLevels.Error, $"User is not authenticated", CallerName);
				return false;
			}
			if (Principal.IsInRole(Role)) return true;
			WriteLog(LogLevels.Warning, $"User {Principal.Account.Login} has no permission to use this fonction", CallerName);
			return false;
		}


		public async Task<Account> GetCurrentAccountAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await Task.FromResult(Principal.Account);

		}

		public async Task<Profile> GetCurrentProfileAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await Task.FromResult(Principal.Profile);
		}


		public async Task<Option> GetOptionAsync()
		{
			Option option;
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			option = await dataProvider.GetOptionAsync(Principal.Account.AccountID.Value);
			if (option==null)
			{
				WriteLog(LogLevels.Information, $"User {Principal.Account.Login} has no option yet, creating new one");
				option = new Option() { AccountID=Principal.Account.AccountID, CalendarWeekRule=System.Globalization.CalendarWeekRule.FirstFourDayWeek, FirstDayOfWeek=DayOfWeek.Monday };
				if (!await dataProvider.CreateOptionAsync(option)) return null;
			}
			return option;
		}
		public async Task<bool> UpdateOptionAsync(Option Option)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false ;
			Option existing = (await dataProvider.GetOptionAsync(Principal.Account.AccountID.Value));
			if ((existing.AccountID != Option.AccountID) || (existing.OptionID!=Option.OptionID)) return false; // trying to hack an existing view
			return await dataProvider.UpdateOptionAsync(Option);
		}


		public async Task<IEnumerable<Employee>> GetEmployeesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await dataProvider.GetEmployeesAsync(Principal.Account.AccountID.Value);
		}

		public async Task<int> CreateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return -1;
			return await dataProvider.CreateEmployeeAsync(Item);
		}

		public async Task<bool> UpdateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			return await dataProvider.UpdateEmployeeAsync(Item);
		}

		public async Task<IEnumerable<ActivityType>> GetActivityTypesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await dataProvider.GetActivityTypesAsync();
		}

		public async Task<int> CreateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return -1;
			return await dataProvider.CreateActivityTypeAsync(Item);
		}

		public async Task<bool> UpdateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return false;
			return await dataProvider.UpdateActivityTypeAsync(Item);
		}


		public async Task<IEnumerable<Profile>> GetProfilesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return null;
			return await dataProvider.GetProfilesAsync();
		}

		public async Task<int> CreateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return -1;
			return await dataProvider.CreateProfileAsync(Item);
		}

		public async Task<bool> UpdateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			if (Item.ProfileID == 1) return false;  // cannot update default profile
			return await dataProvider.UpdateProfileAsync(Item);
		}

		public async Task<IEnumerable<Account>> GetAccountsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return null;
			return await dataProvider.GetAccountsAsync();
		}

		public async Task<int> CreateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return -1;
			return await dataProvider.CreateAccountAsync(Item);
		}

		public async Task<bool> UpdateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			return await dataProvider.UpdateAccountAsync(Item);
		}


		public async Task<IEnumerable<Activity>> GetActivitiesAsync(DateTime Date)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await dataProvider.GetActivitiesAsync(Principal.Account.AccountID.Value, Date);
		}

		public async Task<int> CreateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			if (await dataProvider.HasWriteAccessToEmployeeAsync(Principal.Account.AccountID.Value, Item.EmployeeID.Value) == false)
			{
				WriteLog(LogLevels.Warning, $"User {Principal.Account.Login} has no permission to perform this action");
				return -1;
			}
			return await dataProvider.CreateActivityAsync(Item);
		}
		public async Task<bool> DeleteActivityAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			if (await dataProvider.HasWriteAccessToActivityAsync(Principal.Account.AccountID.Value, ItemID) == false)
			{
				WriteLog(LogLevels.Warning, $"User {Principal.Account.Login} has no permission to perform this action");
				return false;
			}
			return await dataProvider.DeleteActivityAsync(ItemID);
		}
		public async Task<bool> BulkDeleteActivitiesAsync(DateTime StartDate,DateTime EndDate, int EmployeeID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			if (await dataProvider.HasWriteAccessToEmployeeAsync(Principal.Account.AccountID.Value, EmployeeID) == false)
			{
				WriteLog(LogLevels.Warning, $"User {Principal.Account.Login} has no permission to perform this action");
				return false;
			}
			return await dataProvider.BulkDeleteActivitiesAsync(StartDate, EndDate, EmployeeID);
		}
		public async Task<bool> UpdateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			if ((await dataProvider.HasWriteAccessToActivityAsync(Principal.Account.AccountID.Value,Item.ActivityID.Value) == false) || (await dataProvider.HasWriteAccessToEmployeeAsync(Principal.Account.AccountID.Value, Item.EmployeeID.Value) == false))
			{
				WriteLog(LogLevels.Warning, $"User {Principal.Account.Login} has no permission to perform this action");
				return false;
			}
			return await dataProvider.UpdateActivityAsync(Item);
		}
		

		public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int GroupID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return null;
			return await dataProvider.GetGroupMembersAsync(GroupID);
		}

		public async Task<int> CreateGroupMemberAsync(GroupMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return -1;
			if (null != (await dataProvider.GetGroupMembersAsync(Item.GroupID.Value)).FirstOrDefault(item => item.EmployeeID == Item.EmployeeID)) return -1;// cannot duplicate members
			return await dataProvider.CreateGroupMemberAsync(Item);
		}

		public async Task<bool> DeleteGroupMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			if (ItemID == -1) return false; // cannot delete a virtual group member
			return await dataProvider.DeleteGroupMemberAsync(ItemID);
		}



		public async Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return null;
			return await dataProvider.GetGrantsAsync(ProfileID);
		}

		public async Task<int> CreateGrantAsync(Grant Item)
		{
			
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return -1;
			if (null != (await dataProvider.GetGrantsAsync(Item.ProfileID.Value)).FirstOrDefault(item => item.GroupID == Item.GroupID)) return -1;// cannot duplicate members
			return await dataProvider.CreateGrantAsync(Item);
		}

		public async Task<bool> DeleteGrantAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			if (ItemID == -1) return false; // cannot remove virtual grant
			return await dataProvider.DeleteGrantAsync(ItemID);
		}

		public async Task<bool> UpdateGrantAsync(Grant Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			if (Item.GrantID == -1) return false; // cannot remove virtual grant
			return await dataProvider.UpdateGrantAsync(Item);
		}


		public async Task<IEnumerable<Group>> GetGroupsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await dataProvider.GetGroupsAsync(Principal.Account.AccountID.Value);
		}

		public async Task<int> CreateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return -1;
			return await dataProvider.CreateGroupAsync(Item);
		}

		public async Task<bool> DeleteGroupAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			if (ItemID == 1) return false; // cannot delete root group
			return await dataProvider.DeleteGroupAsync(ItemID);
		}

		public async Task<bool> UpdateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			return await dataProvider.UpdateGroupAsync(Item);
		}


		public async Task<IEnumerable<Layer>> GetLayersAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await dataProvider.GetLayersAsync();
		}

		public async Task<int> CreateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return -1;
			return await dataProvider.CreateLayerAsync(Item);
		}

		public async Task<bool> UpdateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return false;
			if ((Item.LayerID == 1) && (Item.IsDisabled==true))  return false; // cannot delete default layer
			return await dataProvider.UpdateLayerAsync(Item);
		}

		public async Task<IEnumerable<EmployeeView>> GetEmployeeViewsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await dataProvider.GetEmployeeViewsAsync(Principal.Account.AccountID.Value);
		}

		public async Task<int> CreateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			if (Item.AccountID != Principal.Account.AccountID) return -1; // cannot create view for someone else
			return await dataProvider.CreateEmployeeViewAsync(Item);
		}
		public async Task<bool> DeleteEmployeeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			EmployeeView existing = (await dataProvider.GetEmployeeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.EmployeeViewID == ItemID);
			if (existing == null) return false; // trying to hack an existing view
			return await dataProvider.DeleteEmployeeViewAsync(ItemID);
		}
		public async Task<bool> UpdateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			EmployeeView existing = (await dataProvider.GetEmployeeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.EmployeeViewID == Item.EmployeeViewID);
			if (existing == null) return false; // trying to hack an existing view
			return await dataProvider.UpdateEmployeeViewAsync(Item);
		}

		public async Task<IEnumerable<ActivityTypeView>> GetActivityTypeViewsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await dataProvider.GetActivityTypeViewsAsync(Principal.Account.AccountID.Value);
		}
		public async Task<int> CreateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			if (Item.AccountID != Principal.Account.AccountID) return -1; // cannot create view for someone else
			return await dataProvider.CreateActivityTypeViewAsync(Item);
		}
		public async Task<bool> DeleteActivityTypeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			ActivityTypeView existing = (await dataProvider.GetActivityTypeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.ActivityTypeViewID == ItemID);
			if (existing == null) return false; // trying to hack an existing view
			return await dataProvider.DeleteActivityTypeViewAsync(ItemID);
		}
		public async Task<bool> UpdateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			ActivityTypeView existing = (await dataProvider.GetActivityTypeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.ActivityTypeViewID == Item.ActivityTypeViewID);
			if (existing == null) return false; // trying to hack an existing view
			return await dataProvider.UpdateActivityTypeViewAsync(Item);
		}




		public async Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int ViewID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			EmployeeView view = (await dataProvider.GetEmployeeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.EmployeeViewID == ViewID);
			if (view == null) return null; // trying to hack an existing view
			return await dataProvider.GetEmployeeViewMembersAsync(Principal.Account.AccountID.Value, ViewID);
		}

		public async Task<int> CreateEmployeeViewMemberAsync(EmployeeViewMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			EmployeeView view = (await dataProvider.GetEmployeeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.EmployeeViewID == Item.EmployeeViewID);
			if (view == null) return -1; // trying to hack an existing view
			if (null != (await dataProvider.GetEmployeeViewMembersAsync(Principal.Account.AccountID.Value, Item.EmployeeViewID.Value)).FirstOrDefault(item => item.EmployeeID == Item.EmployeeID)) return -1;// cannot duplicate members
			return await dataProvider.CreateEmployeeViewMemberAsync(Item);
		}

		public async Task<bool> DeleteEmployeeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			EmployeeViewMember member = (await dataProvider.GetEmployeeViewMembersAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.EmployeeViewMemberID == ItemID);
			if (member == null) return false;
			EmployeeView view = (await dataProvider.GetEmployeeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.EmployeeViewID == member.EmployeeViewID);
			if (view == null) return false; // trying to hack an existing view
			return await dataProvider.DeleteEmployeeViewMemberAsync(ItemID);
		}

		public async Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync(int ViewID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			ActivityTypeView view = (await dataProvider.GetActivityTypeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.ActivityTypeViewID == ViewID);
			if (view == null) return null; // trying to hack an existing view
			return await dataProvider.GetActivityTypeViewMembersAsync(ViewID);
		}

		public async Task<int> CreateActivityTypeViewMemberAsync(ActivityTypeViewMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			ActivityTypeView view = (await dataProvider.GetActivityTypeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.ActivityTypeViewID == Item.ActivityTypeViewID);
			if (view == null) return -1; // trying to hack an existing view
			if (null != (await dataProvider.GetActivityTypeViewMembersAsync(Item.ActivityTypeViewID.Value)).FirstOrDefault(item => item.ActivityTypeID == Item.ActivityTypeID)) return -1;// cannot duplicate members
			return await dataProvider.CreateActivityTypeViewMemberAsync(Item);
		}

		public async Task<bool> DeleteActivityTypeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			ActivityTypeViewMember member= (await dataProvider.GetActivityTypeViewMembersAsync()).FirstOrDefault(item => item.ActivityTypeViewMemberID == ItemID);
			if (member == null) return false;
			ActivityTypeView view = (await dataProvider.GetActivityTypeViewsAsync(Principal.Account.AccountID.Value)).FirstOrDefault(item => item.ActivityTypeViewID == member.ActivityTypeViewID.Value);
			if (view == null) return false; // trying to hack an existing view
			return await dataProvider.DeleteActivityTypeViewMemberAsync(ItemID);
		}


		public async Task<bool> HasWriteAccessToEmployeeAsync(int EmployeeID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await dataProvider.HasWriteAccessToEmployeeAsync(Principal.Account.AccountID.Value, EmployeeID);
		}

		public async Task<bool> HasWriteAccessToActivityAsync(int ActivityID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await dataProvider.HasWriteAccessToActivityAsync(Principal.Account.AccountID.Value, ActivityID);
		}



	}
}

		