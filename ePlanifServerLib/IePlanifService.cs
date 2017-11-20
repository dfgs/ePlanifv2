using ePlanifModelsLib;
using SharpDocLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerLib
{
	[Summary("ePlanif main contract")]
	[ServiceContract]
	public interface IePlanifService
	{
		
		[Summary("Returns a list of employees")]
		[Returns("An enumeration of employees, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<Employee>> GetEmployeesAsync();

		
		[Summary("Create a new employee")]
		[Parameter("Item","Employee you want to create")]
		[Returns("Unique ID of the created employee, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateEmployeeAsync(Employee Item);

		
		[Summary("Update an existing employee")]
		[Parameter("Item","Employee you want to update. EmployeeID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateEmployeeAsync(Employee Item);



		
		[Summary("Returns a list of photos of a given employee")]
		[Parameter("EmployeeID","Unique employee's ID")]
		[Returns("An enumeration of photos, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<Photo>> GetPhotosAsync(int EmployeeID);

		
		[Summary("Create a new photo")]
		[Parameter("Item","Photo you want to create")]
		[Returns("Unique ID of the created photo, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreatePhotoAsync(Photo Item);

		
		[Summary("Update an existing photo")]
		[Parameter("Item","Photo you want to update. PhotoID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdatePhotoAsync(Photo Item);

		
		[Summary("Delete an existing photo")]
		[Parameter("ItemID","Photo's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeletePhotoAsync(int ItemID);



		
		[Summary("Returns a list of activity types")]
		[Returns("An enumeration of activity types, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<ActivityType>> GetActivityTypesAsync();

		
		[Summary("Create a new activity type")]
		[Parameter("Item","Activity type you want to create")]
		[Returns("Unique ID of the created activity type, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateActivityTypeAsync(ActivityType Item);

		
		[Summary("Update an existing activity type")]
		[Parameter("Item","Activity type you want to update. ActivityTypeID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateActivityTypeAsync(ActivityType Item);

		
		[Summary("Returns a list of profiles")]
		[Returns("An enumeration of profiles, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<Profile>> GetProfilesAsync();

		
		[Summary("Create a new profile")]
		[Parameter("Item","Profile you want to create")]
		[Returns("Unique ID of the created profile, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateProfileAsync(Profile Item);

		
		[Summary("Update an existing profile")]
		[Parameter("Item","Profile you want to update. ProfileID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateProfileAsync(Profile Item);


		
		[Summary("Returns a list of activities")]
		[Returns("An enumeration of activities, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<Activity>> GetActivitiesAsync(DateTime Date);

		
		[Summary("Create a new activity")]
		[Parameter("Item","Activity you want to create")]
		[Returns("Unique ID of the created activity, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateActivityAsync(Activity Item);

		
		[Summary("Update an existing activity")]
		[Parameter("Item","Activity you want to update. ActivityID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateActivityAsync(Activity Item);

		
		[Summary("Delete an existing activity")]
		[Parameter("ItemID","Activity's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteActivityAsync(int ItemID);

		
		[Summary("Bulk delete activities between given dates, for the specified employee")]
		[Parameter("StartDate","Delete from this date")]
		[Parameter("EndDate","Delete to this date")]
		[Parameter("EmployeeID","Unique employee's ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> BulkDeleteActivitiesAsync(DateTime StartDate,DateTime EndDate,int EmployeeID);



		
		[Summary("Returns a list of group members")]
		[Returns("An enumeration of group members, or null in case of error")]
		[Parameter("GroupID","Unique ID of the group")] 
		[OperationContract]
		Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int GroupID);

		
		[Summary("Create a new group member")]
		[Parameter("Item","Group member you want to create")]
		[Returns("Unique ID of the created group member, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateGroupMemberAsync(GroupMember Item);

		
		[Summary("Delete an existing group member")]
		[Parameter("ItemID","Group member's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteGroupMemberAsync(int ItemID);

		
		[Summary("Returns a list of profile grants")]
		[Returns("An enumeration of profile grants, or null in case of error")]
		[Parameter("GroupID","Unique ID of the profile")] 
		[OperationContract]
		Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID);


		
		[Summary("Create a new grant")]
		[Parameter("Item","Grant you want to create")]
		[Returns("Unique ID of the created grant, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateGrantAsync(Grant Item);

		
		[Summary("Delete an existing grant")]
		[Parameter("ItemID","Grant's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteGrantAsync(int ItemID);

		
		[Summary("Update an existing grant")]
		[Parameter("Item","Grant you want to update. GrantID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateGrantAsync(Grant Item);
		
		
		[Summary("Returns a list of groups")]
		[Returns("An enumeration of groups, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<Group>> GetGroupsAsync();

		
		[Summary("Create a new group")]
		[Parameter("Item","Group you want to create")]
		[Returns("Unique ID of the created group, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateGroupAsync(Group Item);

		
		[Summary("Delete an existing group")]
		[Parameter("ItemID","Group's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteGroupAsync(int ItemID);

		
		[Summary("Update an existing group")]
		[Parameter("Item","Group you want to update. GroupID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateGroupAsync(Group Item);

		
		[Summary("Returns a list of accounts")]
		[Returns("An enumeration of accounts, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<Account>> GetAccountsAsync();

		
		[Summary("Create a new account")]
		[Parameter("Item","Account you want to create")]
		[Returns("Unique ID of the created account, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateAccountAsync(Account Item);

		
		[Summary("Update an existing account")]
		[Parameter("Item","Account you want to update. AccountID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateAccountAsync(Account Item);

		
		[Summary("Returns current calling account information")]

		[Returns("An account, or null in case of error")]
		[OperationContract]
		Task<Account> GetCurrentAccountAsync();

		
		[Summary("Returns current calling profile information")]
		[Returns("A profile, or null in case of error")]
		[OperationContract]
		Task<Profile> GetCurrentProfileAsync();

		
		[Summary("Returns option for the current account")]
		[Returns("An option, or null in case of error")]
		[OperationContract]
		Task<Option> GetOptionAsync();

		
		[Summary("Update an existing option")]
		[Parameter("Item","Option you want to update. OptionID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateOptionAsync(Option Option);



		
		[Summary("Returns a list of layers")]
		[Returns("An enumeration of layers, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<Layer>> GetLayersAsync();

		
		[Summary("Create a new layer")]
		[Parameter("Item","Layer you want to create")]
		[Returns("Unique ID of the created layer, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateLayerAsync(Layer Item);

		
		[Summary("Update an existing layer")]
		[Parameter("Item","Layer you want to update. LayerID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateLayerAsync(Layer Item);

		
		[Summary("Returns a list of employee views for the current account")]
		[Returns("An enumeration of employee views, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<EmployeeView>> GetEmployeeViewsAsync();

		
		[Summary("Create a new employee view")]
		[Parameter("Item","Employee view you want to create")]
		[Returns("Unique ID of the created employee view, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateEmployeeViewAsync(EmployeeView Item);

		
		[Summary("Delete an existing employee view")]
		[Parameter("ItemID","Employee view's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteEmployeeViewAsync(int ItemID);

		
		[Summary("Update an existing employee view")]
		[Parameter("Item","Employee view you want to update. EmployeeViewID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateEmployeeViewAsync(EmployeeView Item);

		
		[Summary("Returns a list of activity type views for the current account")]
		[Returns("An enumeration of activity type views, or null in case of error")]
		[OperationContract]
		Task<IEnumerable<ActivityTypeView>> GetActivityTypeViewsAsync();

		
		[Summary("Create a new activity type view")]
		[Parameter("Item","Activity type view you want to create")]
		[Returns("Unique ID of the created activity type view, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateActivityTypeViewAsync(ActivityTypeView Item);

		
		[Summary("Delete an existing activity type")]
		[Parameter("ItemID","Activity type's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteActivityTypeViewAsync(int ItemID);

		
		[Summary("Update an existing activity type view")]
		[Parameter("Item","Activity type view you want to update. ActivityTypeViewID value must be set")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> UpdateActivityTypeViewAsync(ActivityTypeView Item);

		
		[Summary("Returns a list of employee view members")]
		[Returns("An enumeration of employee view members, or null in case of error")]
		[Parameter("ViewID","Unique ID of the view")] 
		[OperationContract]
		Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int ViewID);

		
		[Summary("Create a new employee view member")]
		[Parameter("Item","Employee view member you want to create")]
		[Returns("Unique ID of the created employee view member, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateEmployeeViewMemberAsync(EmployeeViewMember Item);

		
		[Summary("Delete an existing employee view")]
		[Parameter("ItemID","Employee view's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteEmployeeViewMemberAsync(int ItemID);

		
		[Summary("Returns a list of activity type view members")]
		[Returns("An enumeration of activity type view members, or null in case of error")]
		[Parameter("ViewID","Unique ID of the view")] 
		[OperationContract]
		Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync(int ViewID);

		
		[Summary("Create a new activity type view member")]
		[Parameter("Item","Activity type view member you want to create")]
		[Returns("Unique ID of the created activity type view member, or -1 in case of failure")]
		[OperationContract]
		Task<int> CreateActivityTypeViewMemberAsync(ActivityTypeViewMember Item);

		
		[Summary("Delete an existing activity type view")]
		[Parameter("ItemID","Activity type view's unique ID")]
		[Returns("True in case of success, or false in case of failure")]
		[OperationContract]
		Task<bool> DeleteActivityTypeViewMemberAsync(int ItemID);

		
		[Summary("Indicates if current account has write access to the given employee")]
		[Returns("True if current account has write access, False otherwise")]
		[Parameter("EmployeeID","Unique ID of the employee")] 
		[OperationContract]
		Task<bool> HasWriteAccessToEmployeeAsync(int EmployeeID);

		
		[Summary("Indicates if current account has write access to the given activity type")]
		[Returns("True if current account has write access, False otherwise")]
		[Parameter("EmployeeID","Unique ID of the activity type")] 
		[OperationContract]
		Task<bool> HasWriteAccessToActivityAsync(int ActivityID);


	}

}
