using DatabaseModelLib;
using DatabaseModelLib.Filters;
using SqlDatabaseModelLib;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifModelsLib
{
	public class ePlanifDatabase : SqlDatabase
    {
		public static Table<Activity> Activities = new Table<Activity>();
		public static Table<ActivityType> ActivityTypes = new Table<ActivityType>();
		public static Table<Employee> Employees = new Table<Employee>();
		public static Table<Account> Accounts = new Table<Account>();
		public static Table<Profile> Profiles = new Table<Profile>();
		public static Table<Group> Groups = new Table<Group>();
		public static Table<GroupMember> GroupMembers = new Table<GroupMember>();
		public static Table<Grant> Grants = new Table<Grant>();
		public static Table<Layer> Layers = new Table<Layer>();
		public static Table<EmployeeView> EmployeeViews = new Table<EmployeeView>();
		public static Table<ActivityTypeView> ActivityTypeViews = new Table<ActivityTypeView>();
		public static Table<EmployeeViewMember> EmployeeViewMembers = new Table<EmployeeViewMember>();
		public static Table<ActivityTypeViewMember> ActivityTypeViewMembers = new Table<ActivityTypeViewMember>();

		public static Relation<ActivityType, Activity, int> ActivityTypeToActivity = new Relation<ActivityType, Activity, int>(ActivityType.ActivityTypeIDColumn, Activity.ActivityTypeIDColumn, DeleteReferentialAction.None);
		public static Relation<Employee, Activity, int> ActivityTypeToEmployee = new Relation<Employee, Activity, int>(Employee.EmployeeIDColumn, Activity.EmployeeIDColumn, DeleteReferentialAction.None);
		public static Relation<Employee, Account, int> AccountToEmployee = new Relation<Employee, Account, int>(Employee.EmployeeIDColumn, Account.EmployeeIDColumn, DeleteReferentialAction.None);
		public static Relation<Profile, Account, int> AccountToProfile = new Relation<Profile, Account, int>(Profile.ProfileIDColumn, Account.ProfileIDColumn, DeleteReferentialAction.None);
		public static Relation<Group, Group, int> GroupToGroup = new Relation<Group, Group, int>(Group.GroupIDColumn, Group.ParentGroupIDColumn, DeleteReferentialAction.None);
		public static Relation<Employee, GroupMember, int> GroupMemberToEmployee = new Relation<Employee, GroupMember, int>(Employee.EmployeeIDColumn, GroupMember.EmployeeIDColumn, DeleteReferentialAction.None);
		public static Relation<Group, GroupMember, int> GroupMemberToGroup = new Relation<Group, GroupMember, int>(Group.GroupIDColumn, GroupMember.GroupIDColumn, DeleteReferentialAction.None);
		public static Relation<Group, Grant, int> GrantToGroup = new Relation<Group, Grant, int>(Group.GroupIDColumn, Grant.GroupIDColumn, DeleteReferentialAction.None);
		public static Relation<Profile, Grant, int> GrantToProfile = new Relation<Profile, Grant, int>(Profile.ProfileIDColumn, Grant.ProfileIDColumn, DeleteReferentialAction.None);
		public static Relation<Layer, ActivityType, int> ActivityTypeToLayer = new Relation<Layer, ActivityType, int>(Layer.LayerIDColumn,ActivityType.LayerIDColumn , DeleteReferentialAction.None);
		public static Relation<Account, EmployeeView, int> AccountToEmployeeView = new Relation<Account, EmployeeView, int>(Account.AccountIDColumn, EmployeeView.AccountIDColumn, DeleteReferentialAction.None);
		public static Relation<Account, ActivityTypeView, int> AccountToActivityTypeView = new Relation<Account, ActivityTypeView, int>(Account.AccountIDColumn, ActivityTypeView.AccountIDColumn, DeleteReferentialAction.None);
		public static Relation<EmployeeView, EmployeeViewMember, int> EmployeeViewToEmployeeViewMember = new Relation<EmployeeView, EmployeeViewMember, int>(EmployeeView.EmployeeViewIDColumn, EmployeeViewMember.EmployeeViewIDColumn, DeleteReferentialAction.None);
		public static Relation<Employee, EmployeeViewMember, int> EmployeeToEmployeeViewMember = new Relation<Employee, EmployeeViewMember, int>(Employee.EmployeeIDColumn, EmployeeViewMember.EmployeeIDColumn, DeleteReferentialAction.None);
		public static Relation<ActivityTypeView, ActivityTypeViewMember, int> ActivityTypeViewToActivityTypeViewMember = new Relation<ActivityTypeView, ActivityTypeViewMember, int>(ActivityTypeView.ActivityTypeViewIDColumn, ActivityTypeViewMember.ActivityTypeViewIDColumn, DeleteReferentialAction.None);
		public static Relation<ActivityType, ActivityTypeViewMember, int> ActivityTypeToActivityTypeViewMember = new Relation<ActivityType, ActivityTypeViewMember, int>(ActivityType.ActivityTypeIDColumn, ActivityTypeViewMember.ActivityTypeIDColumn, DeleteReferentialAction.None);

		public ePlanifDatabase() : this("127.0.0.1")
		{

		}

		public ePlanifDatabase(string ServerName) : base(ServerName)
        {
			
        }

		
	


	}
}
