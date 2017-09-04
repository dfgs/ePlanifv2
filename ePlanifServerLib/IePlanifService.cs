using ePlanifModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerLib
{
	[ServiceContract]
	public interface IePlanifService
	{
		[OperationContract]
		Task<IEnumerable<Employee>> GetEmployeesAsync();
		[OperationContract]
		Task<int> CreateEmployeeAsync(Employee Item);
		[OperationContract]
		Task<bool> UpdateEmployeeAsync(Employee Item);


		[OperationContract]
		Task<IEnumerable<ActivityType>> GetActivityTypesAsync();
		[OperationContract]
		Task<int> CreateActivityTypeAsync(ActivityType Item);
		[OperationContract]
		Task<bool> UpdateActivityTypeAsync(ActivityType Item);

		[OperationContract]
		Task<IEnumerable<Profile>> GetProfilesAsync();
		[OperationContract]
		Task<int> CreateProfileAsync(Profile Item);
		[OperationContract]
		Task<bool> UpdateProfileAsync(Profile Item);


		[OperationContract]
		Task<IEnumerable<Activity>> GetActivitiesAsync(DateTime Date);
		[OperationContract]
		Task<int> CreateActivityAsync(Activity Item);
		[OperationContract]
		Task<bool> DeleteActivityAsync(int ItemID);
		[OperationContract]
		Task<bool> BulkDeleteActivitiesAsync(DateTime StartDate,DateTime EndDate,int EmployeeID);
		[OperationContract]
		Task<bool> UpdateActivityAsync(Activity Item);
		[OperationContract]
		Task<bool> HasWriteAccessAsync(int EmployeeID);


		[OperationContract]
		Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int GroupID);
		[OperationContract]
		Task<int> CreateGroupMemberAsync(GroupMember Item);
		[OperationContract]
		Task<bool> DeleteGroupMemberAsync(int ItemID);

		[OperationContract]
		Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID);
		[OperationContract]
		Task<int> CreateGrantAsync(Grant Item);
		[OperationContract]
		Task<bool> DeleteGrantAsync(int ItemID);
		[OperationContract]
		Task<bool> UpdateGrantAsync(Grant Item);

		[OperationContract]
		Task<IEnumerable<Group>> GetGroupsAsync();
		[OperationContract]
		Task<int> CreateGroupAsync(Group Item);
		[OperationContract]
		Task<bool> DeleteGroupAsync(int ItemID);
		[OperationContract]
		Task<bool> UpdateGroupAsync(Group Item);

		[OperationContract]
		Task<IEnumerable<Account>> GetAccountsAsync();
		[OperationContract]
		Task<int> CreateAccountAsync(Account Item);
		[OperationContract]
		Task<bool> UpdateAccountAsync(Account Item);

		[OperationContract]
		Task<Account> GetCurrentAccountAsync();
		[OperationContract]
		Task<Profile> GetCurrentProfileAsync();


		[OperationContract]
		Task<IEnumerable<Layer>> GetLayersAsync();
		[OperationContract]
		Task<int> CreateLayerAsync(Layer Item);
		[OperationContract]
		Task<bool> UpdateLayerAsync(Layer Item);

		[OperationContract]
		Task<IEnumerable<EmployeeView>> GetEmployeeViewsAsync();
		[OperationContract]
		Task<int> CreateEmployeeViewAsync(EmployeeView Item);
		[OperationContract]
		Task<bool> DeleteEmployeeViewAsync(int ItemID);
		[OperationContract]
		Task<bool> UpdateEmployeeViewAsync(EmployeeView Item);

		[OperationContract]
		Task<IEnumerable<ActivityTypeView>> GetActivityTypeViewsAsync();
		[OperationContract]
		Task<int> CreateActivityTypeViewAsync(ActivityTypeView Item);
		[OperationContract]
		Task<bool> DeleteActivityTypeViewAsync(int ItemID);
		[OperationContract]
		Task<bool> UpdateActivityTypeViewAsync(ActivityTypeView Item);

		[OperationContract]
		Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int ViewID);
		[OperationContract]
		Task<int> CreateEmployeeViewMemberAsync(EmployeeViewMember Item);
		[OperationContract]
		Task<bool> DeleteEmployeeViewMemberAsync(int ItemID);



		[OperationContract]
		Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync(int ViewID);
		[OperationContract]
		Task<int> CreateActivityTypeViewMemberAsync(ActivityTypeViewMember Item);
		[OperationContract]
		Task<bool> DeleteActivityTypeViewMemberAsync(int ItemID);



	}

}
