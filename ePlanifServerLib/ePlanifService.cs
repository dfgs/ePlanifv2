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
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
	public class ePlanifService :Worker, IePlanifService
	{
		private IDatabase<SqlConnection,SqlCommand> database;

		private ePlanifPrincipal principal;
		private ePlanifPrincipal Principal
		{
			get { return principal ?? (ePlanifPrincipal)System.Threading.Thread.CurrentPrincipal; ; }
		}
		public ePlanifService(): base("ePlanifService")
		{
			
			database = new ePlanifDatabase("127.0.0.1");
			principal = null;
		}

		public ePlanifService(ePlanifPrincipal Principal,IDatabase<SqlConnection, SqlCommand> Database) : base("ePlanifService")
		{
			this.database = Database;
			this.principal = Principal;
		}


		private bool AssertPermission(string Role,[CallerMemberName]string CallerName=null)
		{
			if (Principal==null)
			{
				WriteLog(LogLevels.Error, $"User is not authenticated", CallerName);
				return false;
			}
			if (Principal.IsInRole(Role)) return true;
			WriteLog(LogLevels.Warning, $"User {Principal.Account.Login} has no permission to use this fonction", CallerName);
			return false;
		}


		private async Task<ValType?> ExecuteAsync<ValType>(SqlCommand Command)
			where ValType:struct
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			try
			{
				return await database.ExecuteAsync<ValType>(Command);
			}
			catch (Exception ex)
			{
				WriteLog(ex);
				return null;
			}
		}

		private async Task<IEnumerable<ItemType>> SelectAsync<ItemType>(Filter<ItemType> Filter = null, params IColumn<ItemType>[] Orders)
			where ItemType : class, new()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			try
			{
				return await database.SelectAsync<ItemType>(Filter, Orders);
			}
			catch (Exception ex)
			{
				WriteLog(ex);
				return null;
			}
		}

		private async Task<IEnumerable<ItemType>> SelectAsync<ItemType>(SqlCommand Command)
			where ItemType : class, new()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			try
			{
				return await database.SelectAsync<ItemType>(Command);
			}
			catch (Exception ex)
			{
				WriteLog(ex);
				return null;
			}
		}

		private async Task<bool> InsertAsync<ItemType>(ItemType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			try
			{
				await database.InsertAsync<ItemType>(Item);
				return true;
			}
			catch (Exception ex)
			{
				WriteLog(ex);
				return false;
			}
		}

		private async Task<bool> DeleteAsync<ItemType>(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			try
			{
				await database.DeleteAsync<ItemType,int>(ItemID);
				return true;
			}
			catch (Exception ex)
			{
				WriteLog(ex);
				return false;
			}
		}

		private async Task<bool> UpdateAsync<ItemType>(ItemType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			try
			{
				await database.UpdateAsync<ItemType>(Item);
				return true;
			}
			catch (Exception ex)
			{
				WriteLog(ex);
				return false;
			}
		}

		private async Task<bool> ExecuteAsync(SqlCommand Command)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			try
			{
				await database.ExecuteAsync(Command);
				return true;
			}
			catch (Exception ex)
			{
				WriteLog(ex);
				return false;
			}
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


		public async Task<IEnumerable<Employee>> GetEmployeesAsync()
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;

			command = new SqlCommand("select Employee.*,GrantedEmployeesPerAccount.WriteAccess from Employee inner join GrantedEmployeesPerAccount on Employee.EmployeeID=GrantedEmployeesPerAccount.EmployeeID where AccountID=@AccountID order by LastName,FirstName");
			command.Parameters.AddWithValue("@AccountID", Principal.Account.AccountID.Value);
			return await SelectAsync<Employee>(command);

		}

		public async Task<int> CreateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return -1;
			return await InsertAsync(Item) ? Item.EmployeeID.Value : -1;
		}

		public async Task<bool> UpdateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<ActivityType>> GetActivityTypesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await SelectAsync<ActivityType>(null, ActivityType.NameColumn);
		}

		public async Task<int> CreateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return -1;
			return await InsertAsync(Item) ? Item.ActivityTypeID.Value : -1;
		}

		public async Task<bool> UpdateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return false;
			return await UpdateAsync(Item);
		}


		public async Task<IEnumerable<Profile>> GetProfilesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return null;
			return await SelectAsync<Profile>(null, Profile.NameColumn);
		}

		public async Task<int> CreateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return -1;
			return await InsertAsync(Item) ? Item.ProfileID.Value : -1;
		}

		public async Task<bool> UpdateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			if (Item.ProfileID == 1) return false;	// cannot update default profile
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<Account>> GetAccountsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return null;
			return await SelectAsync<Account>(null, Account.LoginColumn);
		}

		public async Task<int> CreateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return -1;
			return await InsertAsync(Item) ? Item.AccountID.Value : -1;
		}

		public async Task<bool> UpdateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			return await UpdateAsync(Item);
		}


		public async Task<IEnumerable<Activity>> GetActivitiesAsync(DateTime Date)
		{
			SqlCommand command;
			DateTime startDate, endDate;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;

			startDate = Date.Date;
			endDate = startDate.AddDays(1);

			command = new SqlCommand("select Activity.* from Activity inner join GrantedEmployeesPerAccount on Activity.EmployeeID=GrantedEmployeesPerAccount.EmployeeID  where AccountID=@AccountID and Activity.StartDate >= @StartDate and Activity.StartDate < @EndDate");
			command.Parameters.AddWithValue("@AccountID", Principal.Account.AccountID.Value);
			command.Parameters.AddWithValue("@StartDate", startDate);
			command.Parameters.AddWithValue("@EndDate", endDate);

			return await SelectAsync<Activity>(command);
		}

		public async Task<int> CreateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			return await InsertAsync(Item) ? Item.ActivityID.Value : -1;
		}
		public async Task<bool> DeleteActivityAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await DeleteAsync<Activity>(ItemID);
		}
		public async Task<bool> BulkDeleteActivitiesAsync(DateTime StartDate,DateTime EndDate, int EmployeeID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;

			command = new SqlCommand("delete from Activity  where EmployeeID=@EmployeeID and StartDate >= @StartDate and StartDate <= @EndDate");
			command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
			command.Parameters.AddWithValue("@StartDate", StartDate);
			command.Parameters.AddWithValue("@EndDate", EndDate);

			return await ExecuteAsync(command);
		}
		public async Task<bool> UpdateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await UpdateAsync(Item);
		}

		public async Task<bool> HasWriteAccessAsync(int EmployeeID)
		{
			SqlCommand command;
			bool? result;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;

			command = new SqlCommand("SELECT [dbo].[HasWriteAccess] (@AccountID,@EmployeeID)");
			command.Parameters.AddWithValue("@AccountID", Principal.Account.AccountID.Value);
			command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

			result= await ExecuteAsync<bool>(command);
			return result == true;
		}



		public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int GroupID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return null;

			command = new SqlCommand("select GroupMembers.* from GroupMembers inner join Employee on GroupMembers.EmployeeID=Employee.EmployeeID where GroupID=@GroupID order by LastName,FirstName");
			command.Parameters.AddWithValue("@GroupID", GroupID);
			return await SelectAsync<GroupMember>(command);
		}

		public async Task<int> CreateGroupMemberAsync(GroupMember Item)
		{
			GroupMember groupMember;
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return -1;

			command = new SqlCommand("select * from GroupMembers where GroupID=@GroupID and EmployeeID=@EmployeeID");
			command.Parameters.AddWithValue("@GroupID", Item.GroupID.Value);
			command.Parameters.AddWithValue("@EmployeeID", Item.EmployeeID.Value);
			groupMember = (await SelectAsync<GroupMember>(command)).FirstOrDefault();
			if (groupMember != null) return -1;	// cannot duplicate members
			return await InsertAsync(Item) ? Item.GroupMemberID.Value : -1;
		}

		public async Task<bool> DeleteGroupMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			if (ItemID == -1) return false; // cannot delete a virtual group member
			return await DeleteAsync<GroupMember>(ItemID);
		}



		public async Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return null;

			command = new SqlCommand("select GrantsPerProfile.* from GrantsPerProfile inner join [Group] on GrantsPerProfile.GroupID=[Group].GroupID where ProfileID=@ProfileID order by [Group].Name");
			command.Parameters.AddWithValue("@ProfileID", ProfileID);
			return await SelectAsync<Grant>(command);
		}

		public async Task<int> CreateGrantAsync(Grant Item)
		{
			Grant grant;
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return -1;
			command = new SqlCommand("select * from GrantsPerProfile where ProfileID=@ProfileID and GroupID=@GroupID");
			command.Parameters.AddWithValue("@ProfileID", Item.ProfileID.Value);
			command.Parameters.AddWithValue("@GroupID", Item.GroupID.Value);
			grant = (await SelectAsync<Grant>(command)).FirstOrDefault();
			if (grant != null) return -1; // cannot duplicate grants

			return await InsertAsync(Item) ? Item.GrantID.Value : -1;
		}

		public async Task<bool> DeleteGrantAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			if (ItemID == -1) return false;	// cannot remove virtual grant
			return await DeleteAsync<Grant>(ItemID);
		}

		public async Task<bool> UpdateGrantAsync(Grant Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateAccounts)) return false;
			if (Item.GrantID == -1) return false; // cannot remove virtual grant
			return await UpdateAsync<Grant>(Item);
		}


		public async Task<IEnumerable<Group>> GetGroupsAsync()
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;

			command = new SqlCommand("select [Group].* from[Group] inner join GrantedGroupsPerAccount on[Group].GroupID = GrantedGroupsPerAccount.GroupID where AccountID = @AccountID");
			command.Parameters.AddWithValue("@AccountID", Principal.Account.AccountID.Value);

			return await SelectAsync<Group>(command);
		}

		public async Task<int> CreateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return -1;
			return await InsertAsync(Item) ? Item.GroupID.Value : -1;
		}

		public async Task<bool> DeleteGroupAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			if (ItemID == 1) return false;
			return await DeleteAsync<Group>(ItemID);
		}

		public async Task<bool> UpdateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateEmployees)) return false;
			return await UpdateAsync(Item);
		}


		public async Task<IEnumerable<Layer>> GetLayersAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await SelectAsync<Layer>(null, Layer.LayerIDColumn);
		}

		public async Task<int> CreateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return -1;
			return await InsertAsync(Item) ? Item.LayerID.Value : -1;
		}

		public async Task<bool> UpdateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.AdministrateActivityTypes)) return false;
			if ((Item.LayerID == 1) && (Item.IsDisabled == true)) return false;
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<EmployeeView>> GetEmployeeViewsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await SelectAsync<EmployeeView>(new EqualFilter<EmployeeView>(EmployeeView.AccountIDColumn, Principal.Account?.AccountID));
		}

		public async Task<int> CreateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			if (Item.AccountID != Principal.Account.AccountID)
			{
				return -1;
			}
			return await InsertAsync(Item) ? Item.EmployeeViewID.Value : -1;
		}
		public async Task<bool> DeleteEmployeeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await DeleteAsync<EmployeeView>(ItemID);
		}
		public async Task<bool> UpdateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<ActivityTypeView>> GetActivityTypeViewsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			return await SelectAsync<ActivityTypeView>(new EqualFilter<ActivityTypeView>(ActivityTypeView.AccountIDColumn, Principal.Account?.AccountID));
		}
		public async Task<int> CreateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			if (Item.AccountID!=Principal.Account.AccountID)
			{
				return -1;
			}
			return await InsertAsync(Item) ? Item.ActivityTypeViewID.Value : -1;
		}
		public async Task<bool> DeleteActivityTypeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await DeleteAsync<ActivityTypeView>(ItemID);
		}
		public async Task<bool> UpdateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await UpdateAsync(Item);
		}




		public async Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int ViewID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;

			command = new SqlCommand("select EmployeeViewMember.* from EmployeeViewMember inner join GrantedEmployeesPerAccount on EmployeeViewMember.EmployeeID = GrantedEmployeesPerAccount.EmployeeID inner join Employee on EmployeeViewMember.EmployeeID=Employee.EmployeeID where GrantedEmployeesPerAccount.AccountID = @AccountID and EmployeeViewID=@EmployeeViewID order by LastName,FirstName");
			command.Parameters.AddWithValue("@AccountID", Principal.Account.AccountID.Value);
			command.Parameters.AddWithValue("@EmployeeViewID", ViewID);

			return await SelectAsync<EmployeeViewMember>(command);
		}

		public async Task<int> CreateEmployeeViewMemberAsync(EmployeeViewMember Item)
		{
			EmployeeViewMember member;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			member = (await SelectAsync<EmployeeViewMember>(new AndFilter<EmployeeViewMember>(new EqualFilter<EmployeeViewMember>(EmployeeViewMember.EmployeeViewIDColumn, Item.EmployeeViewID.Value), new EqualFilter<EmployeeViewMember>(EmployeeViewMember.EmployeeIDColumn, Item.EmployeeID.Value) ))).FirstOrDefault();
			if (member != null) return -1;//cannot duplicate view member
			return await InsertAsync(Item) ? Item.EmployeeViewMemberID.Value : -1;
		}

		public async Task<bool> DeleteEmployeeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await DeleteAsync<EmployeeViewMember>(ItemID);
		}

		public async Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync(int ViewID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return null;
			command = new SqlCommand("select ActivityTypeViewMember.* from ActivityTypeViewMember inner join ActivityType on  ActivityTypeViewMember.ActivityTypeID=ActivityType.ActivityTypeID where ActivityTypeViewID = @ActivityTypeViewID order by Name");
			command.Parameters.AddWithValue("@ActivityTypeViewID",ViewID);

			return await SelectAsync<ActivityTypeViewMember>(command);


		}
		public async Task<int> CreateActivityTypeViewMemberAsync(ActivityTypeViewMember Item)
		{
			ActivityTypeViewMember member;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return -1;
			member = (await SelectAsync<ActivityTypeViewMember>(new AndFilter<ActivityTypeViewMember>(new EqualFilter<ActivityTypeViewMember>(ActivityTypeViewMember.ActivityTypeViewIDColumn, Item.ActivityTypeViewID.Value), new EqualFilter<ActivityTypeViewMember>(ActivityTypeViewMember.ActivityTypeIDColumn, Item.ActivityTypeID.Value)))).FirstOrDefault();
			if (member != null) return -1;//cannot duplicate view member

			return await InsertAsync(Item) ? Item.ActivityTypeViewMemberID.Value : -1;
		}

		public async Task<bool> DeleteActivityTypeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			if (!AssertPermission(Roles.ePlanifUser)) return false;
			return await DeleteAsync<ActivityTypeViewMember>(ItemID);
		}


	}
}

		