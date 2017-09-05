using DatabaseModelLib;
using DatabaseModelLib.Filters;
using ePlanifModelsLib;
using LogUtils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerLib;

namespace ePlanifServerLib
{
	public class SqlDataProvider : Worker, IDataProvider
	{
		private ePlanifDatabase database;

		public SqlDataProvider() : base("SqlDataProvider")
		{
			database = new ePlanifDatabase("127.0.0.1");
		}

		#region safe database operations
		private async Task<ValType?> ExecuteAsync<ValType>(SqlCommand Command)
			where ValType : struct
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
				await database.DeleteAsync<ItemType, int>(ItemID);
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

		#endregion

		public async Task<IEnumerable<Employee>> GetEmployeesAsync(int AccountID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			command = new SqlCommand("select Employee.*,GrantedEmployeesPerAccount.WriteAccess from Employee inner join GrantedEmployeesPerAccount on Employee.EmployeeID=GrantedEmployeesPerAccount.EmployeeID where AccountID=@AccountID order by LastName,FirstName");
			command.Parameters.AddWithValue("@AccountID", AccountID);
			return await SelectAsync<Employee>(command);

		}

		public async Task<int> CreateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.EmployeeID.Value : -1;
		}

		public async Task<bool> UpdateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<ActivityType>> GetActivityTypesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await SelectAsync<ActivityType>(null, ActivityType.NameColumn);
		}

		public async Task<int> CreateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.ActivityTypeID.Value : -1;
		}

		public async Task<bool> UpdateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}


		public async Task<IEnumerable<Profile>> GetProfilesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await SelectAsync<Profile>(null, Profile.NameColumn);
		}

		public async Task<int> CreateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.ProfileID.Value : -1;
		}

		public async Task<bool> UpdateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<Account>> GetAccountsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await SelectAsync<Account>(null, Account.LoginColumn);
		}

		public async Task<int> CreateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.AccountID.Value : -1;
		}

		public async Task<bool> UpdateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}


		public async Task<IEnumerable<Activity>> GetActivitiesAsync(int AccountID,DateTime Date)
		{
			SqlCommand command;
			DateTime startDate, endDate;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			startDate = Date.Date;
			endDate = startDate.AddDays(1);

			command = new SqlCommand("select Activity.* from Activity inner join GrantedEmployeesPerAccount on Activity.EmployeeID=GrantedEmployeesPerAccount.EmployeeID  where AccountID=@AccountID and Activity.StartDate >= @StartDate and Activity.StartDate < @EndDate");
			command.Parameters.AddWithValue("@AccountID", AccountID);
			command.Parameters.AddWithValue("@StartDate", startDate);
			command.Parameters.AddWithValue("@EndDate", endDate);

			return await SelectAsync<Activity>(command);
		}

		public async Task<int> CreateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.ActivityID.Value : -1;
		}
		public async Task<bool> DeleteActivityAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<Activity>(ItemID);
		}
		public async Task<bool> BulkDeleteActivitiesAsync(DateTime StartDate, DateTime EndDate, int EmployeeID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("delete from Activity  where EmployeeID=@EmployeeID and StartDate >= @StartDate and StartDate <= @EndDate");
			command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
			command.Parameters.AddWithValue("@StartDate", StartDate);
			command.Parameters.AddWithValue("@EndDate", EndDate);

			return await ExecuteAsync(command);
		}
		public async Task<bool> UpdateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}





		public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int GroupID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("select GroupMembers.* from GroupMembers inner join Employee on GroupMembers.EmployeeID=Employee.EmployeeID where GroupID=@GroupID order by LastName,FirstName");
			command.Parameters.AddWithValue("@GroupID", GroupID);
			return await SelectAsync<GroupMember>(command);
		}

		public async Task<int> CreateGroupMemberAsync(GroupMember Item)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("select * from GroupMembers where GroupID=@GroupID and EmployeeID=@EmployeeID");
			command.Parameters.AddWithValue("@GroupID", Item.GroupID.Value);
			command.Parameters.AddWithValue("@EmployeeID", Item.EmployeeID.Value);
			return await InsertAsync(Item) ? Item.GroupMemberID.Value : -1;
		}

		public async Task<bool> DeleteGroupMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<GroupMember>(ItemID);
		}



		public async Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("select GrantsPerProfile.* from GrantsPerProfile inner join [Group] on GrantsPerProfile.GroupID=[Group].GroupID where ProfileID=@ProfileID order by [Group].Name");
			command.Parameters.AddWithValue("@ProfileID", ProfileID);
			return await SelectAsync<Grant>(command);
		}

		public async Task<int> CreateGrantAsync(Grant Item)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			command = new SqlCommand("select * from GrantsPerProfile where ProfileID=@ProfileID and GroupID=@GroupID");
			command.Parameters.AddWithValue("@ProfileID", Item.ProfileID.Value);
			command.Parameters.AddWithValue("@GroupID", Item.GroupID.Value);

			return await InsertAsync(Item) ? Item.GrantID.Value : -1;
		}

		public async Task<bool> DeleteGrantAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<Grant>(ItemID);
		}

		public async Task<bool> UpdateGrantAsync(Grant Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync<Grant>(Item);
		}


		public async Task<IEnumerable<Group>> GetGroupsAsync(int AccountID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("select [Group].* from[Group] inner join GrantedGroupsPerAccount on[Group].GroupID = GrantedGroupsPerAccount.GroupID where AccountID = @AccountID");
			command.Parameters.AddWithValue("@AccountID", AccountID);

			return await SelectAsync<Group>(command);
		}

		public async Task<int> CreateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.GroupID.Value : -1;
		}

		public async Task<bool> DeleteGroupAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<Group>(ItemID);
		}

		public async Task<bool> UpdateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}


		public async Task<IEnumerable<Layer>> GetLayersAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await SelectAsync<Layer>(null, Layer.LayerIDColumn);
		}

		public async Task<int> CreateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.LayerID.Value : -1;
		}

		public async Task<bool> UpdateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<EmployeeView>> GetEmployeeViewsAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await SelectAsync<EmployeeView>(new EqualFilter<EmployeeView>(EmployeeView.AccountIDColumn, AccountID));
		}

		public async Task<int> CreateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.EmployeeViewID.Value : -1;
		}
		public async Task<bool> DeleteEmployeeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<EmployeeView>(ItemID);
		}
		public async Task<bool> UpdateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}

		public async Task<IEnumerable<ActivityTypeView>> GetActivityTypeViewsAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await SelectAsync<ActivityTypeView>(new EqualFilter<ActivityTypeView>(ActivityTypeView.AccountIDColumn, AccountID));
		}
		public async Task<int> CreateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.ActivityTypeViewID.Value : -1;
		}
		public async Task<bool> DeleteActivityTypeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<ActivityTypeView>(ItemID);
		}
		public async Task<bool> UpdateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await UpdateAsync(Item);
		}




		public async Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int AccountID,int ViewID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("select EmployeeViewMember.* from EmployeeViewMember inner join GrantedEmployeesPerAccount on EmployeeViewMember.EmployeeID = GrantedEmployeesPerAccount.EmployeeID inner join Employee on EmployeeViewMember.EmployeeID=Employee.EmployeeID where GrantedEmployeesPerAccount.AccountID = @AccountID and EmployeeViewID=@EmployeeViewID order by LastName,FirstName");
			command.Parameters.AddWithValue("@AccountID", AccountID);
			command.Parameters.AddWithValue("@EmployeeViewID", ViewID);

			return await SelectAsync<EmployeeViewMember>(command);
		}

		public async Task<int> CreateEmployeeViewMemberAsync(EmployeeViewMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.EmployeeViewMemberID.Value : -1;
		}

		public async Task<bool> DeleteEmployeeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<EmployeeViewMember>(ItemID);
		}

		public async Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync(int ViewID)
		{
			SqlCommand command;

			WriteLog(LogLevels.Debug, LogActions.Enter);
			
			command = new SqlCommand("select ActivityTypeViewMember.* from ActivityTypeViewMember inner join ActivityType on  ActivityTypeViewMember.ActivityTypeID=ActivityType.ActivityTypeID where ActivityTypeViewID = @ActivityTypeViewID order by Name");
			command.Parameters.AddWithValue("@ActivityTypeViewID", ViewID);

			return await SelectAsync<ActivityTypeViewMember>(command);


		}
		public async Task<int> CreateActivityTypeViewMemberAsync(ActivityTypeViewMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await InsertAsync(Item) ? Item.ActivityTypeViewMemberID.Value : -1;
		}

		public async Task<bool> DeleteActivityTypeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await DeleteAsync<ActivityTypeViewMember>(ItemID);
		}


		public async Task<bool> HasWriteAccessToEmployeeAsync(int AccountID,int EmployeeID)
		{
			SqlCommand command;
			bool? result;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("SELECT [dbo].[HasWriteAccessToEmployee] (@AccountID,@EmployeeID)");
			command.Parameters.AddWithValue("@AccountID", AccountID);
			command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

			result = await ExecuteAsync<bool>(command);
			return result == true;
		}

		public async Task<bool> HasWriteAccessToActivityAsync(int AccountID,int ActivityID)
		{
			SqlCommand command;
			bool? result;

			WriteLog(LogLevels.Debug, LogActions.Enter);

			command = new SqlCommand("SELECT [dbo].[HasWriteAccessToActivity] (@AccountID,@ActivityID)");
			command.Parameters.AddWithValue("@AccountID", AccountID);
			command.Parameters.AddWithValue("@ActivityID", ActivityID);

			result = await ExecuteAsync<bool>(command);
			return result == true;
		}

	}
}
