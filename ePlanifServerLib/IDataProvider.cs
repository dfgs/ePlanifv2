using ePlanifModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerLib
{
	public interface IDataProvider
	{
		Account GetAccount(string Login);
		Profile GetProfile(int ProfileID);

		Task<Option> GetOptionAsync(int AccountID);
		Task<bool> CreateOptionAsync(Option Option);
		Task<bool> UpdateOptionAsync(Option Option);

		Task<IEnumerable<Employee>> GetEmployeesAsync(int AccountID);
		Task<int> CreateEmployeeAsync(Employee Item);
		Task<bool> UpdateEmployeeAsync(Employee Item);

		Task<IEnumerable<Photo>> GetPhotosAsync(int EmployeeID);
		Task<int> CreatePhotoAsync(Photo Item);
		Task<bool> DeletePhotoAsync(int ItemID);
		Task<bool> UpdatePhotoAsync(Photo Item);

		Task<IEnumerable<ActivityType>> GetActivityTypesAsync();
		Task<int> CreateActivityTypeAsync(ActivityType Item);
		Task<bool> UpdateActivityTypeAsync(ActivityType Item);


		Task<IEnumerable<Profile>> GetProfilesAsync();
		Task<int> CreateProfileAsync(Profile Item);
		Task<bool> UpdateProfileAsync(Profile Item);

		
		Task<IEnumerable<Activity>> GetActivitiesAsync(int AccountID,DateTime Date);
		Task<int> CreateActivityAsync(Activity Item);
		Task<bool> DeleteActivityAsync(int ItemID);
		Task<bool> BulkDeleteActivitiesAsync(DateTime StartDate, DateTime EndDate, int EmployeeID);
		Task<bool> UpdateActivityAsync(Activity Item);

		
		Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int GroupID);
		Task<int> CreateGroupMemberAsync(GroupMember Item);
		Task<bool> DeleteGroupMemberAsync(int ItemID);

		
		Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID);
		Task<int> CreateGrantAsync(Grant Item);
		Task<bool> DeleteGrantAsync(int ItemID);
		Task<bool> UpdateGrantAsync(Grant Item);

		
		Task<IEnumerable<Group>> GetGroupsAsync(int AccountID);
		Task<int> CreateGroupAsync(Group Item);
		Task<bool> DeleteGroupAsync(int ItemID);
		Task<bool> UpdateGroupAsync(Group Item);

		
		Task<IEnumerable<Account>> GetAccountsAsync();
		Task<int> CreateAccountAsync(Account Item);
		Task<bool> UpdateAccountAsync(Account Item);

	
		Task<IEnumerable<Layer>> GetLayersAsync();
		Task<int> CreateLayerAsync(Layer Item);
		Task<bool> UpdateLayerAsync(Layer Item);

		
		Task<IEnumerable<EmployeeView>> GetEmployeeViewsAsync(int AccountID);
		Task<int> CreateEmployeeViewAsync(EmployeeView Item);
		Task<bool> DeleteEmployeeViewAsync(int ItemID);
		Task<bool> UpdateEmployeeViewAsync(EmployeeView Item);


		Task<IEnumerable<ActivityTypeView>> GetActivityTypeViewsAsync(int AccountID);
		Task<int> CreateActivityTypeViewAsync(ActivityTypeView Item);
		Task<bool> DeleteActivityTypeViewAsync(int ItemID);
		Task<bool> UpdateActivityTypeViewAsync(ActivityTypeView Item);


		Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int AccountID, int ViewID);
		Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int AccountID);
		Task<int> CreateEmployeeViewMemberAsync(EmployeeViewMember Item);
		Task<bool> DeleteEmployeeViewMemberAsync(int ItemID);


		Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync(int ViewID);
		Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync();
		Task<int> CreateActivityTypeViewMemberAsync(ActivityTypeViewMember Item);
		Task<bool> DeleteActivityTypeViewMemberAsync(int ItemID);


		Task<bool> HasWriteAccessToEmployeeAsync(int AccountID,int EmployeeID);
		Task<bool> HasWriteAccessToActivityAsync(int AccountID, int ActivityID);

	}
}
