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
	public class TestDataProvider : Worker, IDataProvider
	{
		private List<Activity> Activities = new List<Activity>();
		private List<ActivityType> ActivityTypes = new List<ActivityType>();
		private List<Employee> Employees = new List<Employee>();
		private List<Account> Accounts = new List<Account>();
		private List<Profile> Profiles = new List<Profile>();
		private List<Group> Groups = new List<Group>();
		private List<GroupMember> Members = new List<GroupMember>();
		private List<Grant> Grants = new List<Grant>();
		private List<Layer> Layers = new List<Layer>();
		private List<EmployeeView> EmployeeViews = new List<EmployeeView>();
		private List<ActivityTypeView> ActivityTypeViews = new List<ActivityTypeView>();
		private List<EmployeeViewMember> EmployeeViewMembers = new List<EmployeeViewMember>();
		private List<ActivityTypeViewMember> ActivityTypeViewMembers = new List<ActivityTypeViewMember>();

		private string fakeLogin;

		public TestDataProvider(string FakeLogin) : base("TestDataProvider")
		{

			this.fakeLogin = FakeLogin;

			Profiles.Add(new Profile() { ProfileID = 1, Name = "Administrator", AdministrateAccounts = true, AdministrateActivityTypes = true, AdministrateEmployees = true, CanRunReports = true });
			Profiles.Add(new Profile() { ProfileID = 2, Name = "Children", AdministrateAccounts = false, AdministrateActivityTypes = false, AdministrateEmployees = false, CanRunReports = false});
			Profiles.Add(new Profile() { ProfileID = 3, Name = "Parents", AdministrateAccounts = true, AdministrateActivityTypes = true, AdministrateEmployees = true, CanRunReports = true });

			Accounts.Add(new Account() { AccountID = 1, Login = "admin", ProfileID = 3 });
			Accounts.Add(new Account() { AccountID = 2, Login = "liza", ProfileID = 2 });
			Accounts.Add(new Account() { AccountID = 3, Login = "bart", ProfileID = 2 });

			Layers.Add(new Layer() { LayerID = 1, Name = "Job" });
			Layers.Add(new Layer() { LayerID = 2, Name = "Home tasks", Color = "Violet" });
			Layers.Add(new Layer() { LayerID = 3, Name = "Personal tasks", Color = "Gold" });

			Employees.Add(new Employee() { EmployeeID = 1, FirstName = "Homer", LastName = "Simpson", CountryCode = "US", MaxWorkingHoursPerWeek = 10 });
			Employees.Add(new Employee() { EmployeeID = 2, FirstName = "Marje", LastName = "Simpson", CountryCode = "US", MaxWorkingHoursPerWeek = 50 });
			Employees.Add(new Employee() { EmployeeID = 3, FirstName = "Bart", LastName = "Simpson", CountryCode = "US", MaxWorkingHoursPerWeek = null });
			Employees.Add(new Employee() { EmployeeID = 4, FirstName = "Liza", LastName = "Simpson", CountryCode = "US", MaxWorkingHoursPerWeek = null });
			Employees.Add(new Employee() { EmployeeID = 5, FirstName = "Maggy", LastName = "Simpson", CountryCode = "US", MaxWorkingHoursPerWeek = null });

			Groups.Add(new Group() { GroupID = 1, Name = "Springfield" });
			Groups.Add(new Group() { GroupID = 2, Name = "Simpsons", ParentGroupID = 1 });
			Groups.Add(new Group() { GroupID = 3, Name = "Parents", ParentGroupID = 2 });
			Groups.Add(new Group() { GroupID = 4, Name = "Children", ParentGroupID = 2 });

			Members.Add(new GroupMember() { GroupMemberID = 1, GroupID = 3, EmployeeID = 1 });
			Members.Add(new GroupMember() { GroupMemberID = 1, GroupID = 3, EmployeeID = 2 });
			Members.Add(new GroupMember() { GroupMemberID = 1, GroupID = 4, EmployeeID = 3 });
			Members.Add(new GroupMember() { GroupMemberID = 1, GroupID = 4, EmployeeID = 4 });
			Members.Add(new GroupMember() { GroupMemberID = 1, GroupID = 4, EmployeeID = 5 });

			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 1, BackgroundColor = "LightGreen", LayerID = 1, MinEmployees = 1, Name = "Monitor nuclear plant", TextColor = "Red" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 2, BackgroundColor = "Green", LayerID = 1, MinEmployees = 0, Name = "Send security report", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 3, BackgroundColor = "LightGreen", LayerID = 1, MinEmployees = 1, Name = "Care of baby", TextColor = "Red" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 4, BackgroundColor = "Green", LayerID = 1, MinEmployees = 0, Name = "Go to kwik&mark", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 5, BackgroundColor = "LightBlue", LayerID = 1, MinEmployees = 2, Name = "Go to school", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 6, BackgroundColor = "Lavender", LayerID = 2, MinEmployees = 0, Name = "Clean house", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 7, BackgroundColor = "Lavender", LayerID = 2, MinEmployees = 0, Name = "Repair car", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 8, BackgroundColor = "Lavender", LayerID = 2, MinEmployees = 0, Name = "Feed pets", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 9, BackgroundColor = "SteelBlue", LayerID = 3, MinEmployees = 0, Name = "Go the Moe's bar", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 10, BackgroundColor = "SteelBlue", LayerID = 3, MinEmployees = 0, Name = "Read book", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 11, BackgroundColor = "LightSteelBlue", LayerID = 3, MinEmployees = 0, Name = "Play sax", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 12, BackgroundColor = "LightSteelBlue", LayerID = 3, MinEmployees = 0, Name = "Play skate", TextColor = "Black" });


			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 1, AccountID = 1, Name = "All" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 2, AccountID = 1, Name = "Parents" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 3, AccountID = 1, Name = "Children" });
			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID=1,AccountID=1,Name="All" });

			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 1, EmployeeViewID = 1, EmployeeID = 1 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 2, EmployeeViewID = 1, EmployeeID = 2 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 3, EmployeeViewID = 1, EmployeeID = 3 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 4, EmployeeViewID = 1, EmployeeID = 4 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 5, EmployeeViewID = 1, EmployeeID = 5 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 6, EmployeeViewID = 2, EmployeeID = 1 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 7, EmployeeViewID = 2, EmployeeID = 2 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 8, EmployeeViewID = 3, EmployeeID = 3 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 9, EmployeeViewID = 3, EmployeeID = 4 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 10, EmployeeViewID = 3, EmployeeID = 5 });

			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 1, ActivityTypeViewID = 1, ActivityTypeID = 1 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 2, ActivityTypeViewID = 1, ActivityTypeID = 2 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 3, ActivityTypeViewID = 1, ActivityTypeID = 3 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 4, ActivityTypeViewID = 1, ActivityTypeID = 4 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 5, ActivityTypeViewID = 1, ActivityTypeID = 5 });

			Grants.Add(new Grant() { GrantID = 1, ProfileID = 2, GroupID = 3, WriteAccess = false });
			Grants.Add(new Grant() { GrantID = 2, ProfileID = 2, GroupID = 4, WriteAccess = true });
			Grants.Add(new Grant() { GrantID = 3, ProfileID = 3, GroupID = 2, WriteAccess = true });
		}


		private int GetProfileID(int AccountID)
		{
			return Accounts.First(item => item.AccountID == AccountID).ProfileID.Value;
		}

		private IEnumerable<Grant> GrantsPerProfile()
		{
			return
				(from grant in Grants
				select grant).Union(new Grant[] { new Grant() { GrantID=-1,GroupID=1,ProfileID=1,WriteAccess=true } } );
		}

		private IEnumerable<GroupMember> GroupMembers()
		{
			return
				(from groupMember in Members
					select groupMember).Union( 
					from employee in Employees
					select new GroupMember() { GroupMemberID=-1, EmployeeID=employee.EmployeeID,GroupID=1 }
					);
		}

		private IEnumerable<(int ParentGroupID, Group Group)> GroupChildren(int ParentGroupID)
		{
			foreach(Group group in Groups.Where(item=>item.ParentGroupID==ParentGroupID))
			{
				yield return (ParentGroupID,group);
				foreach((int,Group) item in GroupChildren(group.GroupID.Value))
				{
					yield return (ParentGroupID, item.Item2);
				}
			}
		}

		private IEnumerable<(int ParentGroupID,Group Group)> GroupHierarchy()
		{
			foreach (Group group in Groups)
			{
				yield return (group.GroupID.Value,group);
				foreach ((int,Group) item in GroupChildren(group.GroupID.Value))
				{
					yield return (group.GroupID.Value, item.Item2);
				}
			}
		}

		private IEnumerable<Group> GetGrantedGroups(int ProfileID)
		{
			return (from grant in GrantsPerProfile()
					join grantedGroup in GroupHierarchy() on grant.GroupID equals grantedGroup.ParentGroupID
					select grantedGroup.Group).Distinct();
		}

		private IEnumerable<Employee> GetGrantedEmployees(int ProfileID)
		{
			return from grant in GrantsPerProfile() where grant.ProfileID==ProfileID
				   join grantedGroup in GroupHierarchy() on grant.GroupID equals grantedGroup.ParentGroupID
				   join member in Members on grantedGroup.Group.GroupID equals member.GroupID
				   join employee in Employees on member.EmployeeID equals employee.EmployeeID
				   group  new Employee(employee) { WriteAccess=grant.WriteAccess }  by new { employee.EmployeeID, employee.FirstName, employee.LastName, employee.CountryCode, employee.IsDisabled, employee.MaxWorkingHoursPerWeek } into employeeGroup
				   select new Employee()
				   {
					   EmployeeID = employeeGroup.Key.EmployeeID,
					   CountryCode = employeeGroup.Key.CountryCode,
					   FirstName = employeeGroup.Key.FirstName,
					   LastName = employeeGroup.Key.LastName,
					   IsDisabled = employeeGroup.Key.IsDisabled,
					   MaxWorkingHoursPerWeek = employeeGroup.Key.MaxWorkingHoursPerWeek,
					   WriteAccess = (from item in employeeGroup select item.WriteAccess.Value).Max()
				   };

		}

		private int Insert<ItemType>(List<ItemType> List, ItemType Item)
		{
			int key;

			if (List.Count == 0) key = 1;
			else key = List.Max( item=> (int)Schema<ItemType>.PrimaryKey.GetValue(item) )+1;
			Schema<ItemType>.PrimaryKey.SetValue(Item, key);
			List.Add(Item);

			return key;
		}

		private bool Update<ItemType>(List<ItemType> List, ItemType Item)
		{
			ItemType oldItem;
			int key;
			int index;

			key = (int)Schema<ItemType>.PrimaryKey.GetValue(Item);
			oldItem = List.First(item => ValueType.Equals( Schema<ItemType>.PrimaryKey.GetValue(item) , key));
			index = List.IndexOf(oldItem);
			List[index] = Item;

			return true;
		}

		private bool Delete<ItemType>(List<ItemType> List, int ID)
		{
			ItemType oldItem;
			int index;

			oldItem = List.First(item => ValueType.Equals(Schema<ItemType>.PrimaryKey.GetValue(item), ID));
			index = List.IndexOf(oldItem);
			List.RemoveAt(index);

			return true;
		}

		public Account GetAccount(string Login)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return Accounts.FirstOrDefault(item => (item.IsDisabled == false) && (item.Login.Value.Value.ToLower()==fakeLogin.ToLower()) );
		}

		public Profile GetProfile(int ProfileID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return Profiles.FirstOrDefault(item => (item.IsDisabled == false) && (item.ProfileID == ProfileID));
		}

		public async Task<IEnumerable<Employee>> GetEmployeesAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(GetGrantedEmployees(GetProfileID(AccountID)).Distinct() );
		}

		public async Task<int> CreateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Employees, Item));
		}

		public async Task<bool> UpdateEmployeeAsync(Employee Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Employees, Item));
		}

		public async Task<IEnumerable<ActivityType>> GetActivityTypesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(ActivityTypes);
		}

		public async Task<int> CreateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(ActivityTypes, Item));
		}

		public async Task<bool> UpdateActivityTypeAsync(ActivityType Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(ActivityTypes, Item));
		}


		public async Task<IEnumerable<Profile>> GetProfilesAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Profiles);
		}

		public async Task<int> CreateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Profiles, Item));
		}

		public async Task<bool> UpdateProfileAsync(Profile Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Profiles, Item));
		}

		public async Task<IEnumerable<Account>> GetAccountsAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Accounts);
		}

		public async Task<int> CreateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Accounts, Item));
		}

		public async Task<bool> UpdateAccountAsync(Account Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Accounts, Item));
		}



		public async Task<IEnumerable<Activity>> GetActivitiesAsync(int AccountID,DateTime Date)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			List<Employee> grantedEmployees = new List<Employee>(GetGrantedEmployees(GetProfileID(AccountID)));
			List<Activity> result;

			result = new List<Activity>();
			foreach (Activity activity in Activities.Where(item=>item.StartDate.Value.Date==Date))
			{
				if (grantedEmployees.FirstOrDefault(item=>item.EmployeeID==activity.EmployeeID)==null) continue;
				result.Add(activity);
			}
			return await Task.FromResult(result);
		}

		public async Task<int> CreateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Activities, Item));
		}
		public async Task<bool> DeleteActivityAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(Activities, ItemID));
		}
		public async Task<bool> BulkDeleteActivitiesAsync(DateTime StartDate, DateTime EndDate, int EmployeeID)
		{
			Activity[] items;
			items = Activities.Where(item => (item.EmployeeID == EmployeeID) && (item.StartDate.Value.Date >= StartDate) && (item.StartDate.Value.Date <= EndDate)).ToArray();
			foreach(Activity activity in items)
			{
				Activities.Remove(activity);
			}
			return await Task.FromResult(true);
		}
		public async Task<bool> UpdateActivityAsync(Activity Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Activities, Item));
		}
		

		public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int GroupID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			List<GroupMember> result;

			result = new List<GroupMember>();
			if (GroupID==1)
			{
				foreach(Employee employee in Employees)
				{
					result.Add(new GroupMember() { GroupMemberID=-1, EmployeeID=employee.EmployeeID, GroupID=GroupID });
				}
			}
			else
			{
				foreach(GroupMember member in Members.Where(item => item.GroupID == GroupID))
				{
					result.Add(member);
				}
			}
			return await Task.FromResult(result);
		}

		public async Task<int> CreateGroupMemberAsync(GroupMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Members, Item));
		}

		public async Task<bool> DeleteGroupMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(Members, ItemID));
		}



		public async Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return  await Task.FromResult( from grant in GrantsPerProfile() where grant.ProfileID==ProfileID select grant );
		}

		public async Task<int> CreateGrantAsync(Grant Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Grants, Item));
		}

		public async Task<bool> DeleteGrantAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(Grants, ItemID));
		}

		public async Task<bool> UpdateGrantAsync(Grant Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Grants, Item));
		}


		public async Task<IEnumerable<Group>> GetGroupsAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult( GetGrantedGroups(GetProfileID(AccountID)) );
		}

		public async Task<int> CreateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Groups, Item));
		}

		public async Task<bool> DeleteGroupAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(Groups, ItemID));
		}

		public async Task<bool> UpdateGroupAsync(Group Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Groups, Item));
		}


		public async Task<IEnumerable<Layer>> GetLayersAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Layers);
		}

		public async Task<int> CreateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Layers, Item));
		}

		public async Task<bool> UpdateLayerAsync(Layer Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Layers, Item));
		}


		public async Task<IEnumerable<EmployeeView>> GetEmployeeViewsAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(EmployeeViews.Where(item => item.AccountID == AccountID));
		}

		public async Task<int> CreateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(EmployeeViews, Item));
		}
		public async Task<bool> DeleteEmployeeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(EmployeeViews, ItemID));
		}
		public async Task<bool> UpdateEmployeeViewAsync(EmployeeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(EmployeeViews, Item));
		}

		public async Task<IEnumerable<ActivityTypeView>> GetActivityTypeViewsAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(ActivityTypeViews.Where(item => item.AccountID == AccountID));
		}

		public async Task<int> CreateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(ActivityTypeViews, Item));
		}
		public async Task<bool> DeleteActivityTypeViewAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(ActivityTypeViews, ItemID));
		}
		public async Task<bool> UpdateActivityTypeViewAsync(ActivityTypeView Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(ActivityTypeViews, Item));
		}




		public async Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int AccountID,int ViewID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			List<Employee> grantedEmployees = new List<Employee>(GetGrantedEmployees(GetProfileID(AccountID)));
			List<EmployeeViewMember> result;

			result = new List<EmployeeViewMember>();
			foreach (EmployeeViewMember member in EmployeeViewMembers.Where(item => item.EmployeeViewID==ViewID))
			{
				if (grantedEmployees.FirstOrDefault(item => item.EmployeeID == member.EmployeeID) == null) continue;
				result.Add(member);
			}
			return await Task.FromResult(result);
		}

		public async Task<int> CreateEmployeeViewMemberAsync(EmployeeViewMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(EmployeeViewMembers, Item));
		}

		public async Task<bool> DeleteEmployeeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(EmployeeViewMembers, ItemID));
		}

		public async Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync(int ViewID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(ActivityTypeViewMembers.Where(item => item.ActivityTypeViewID == ViewID));
		}
		public async Task<int> CreateActivityTypeViewMemberAsync(ActivityTypeViewMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(ActivityTypeViewMembers, Item));
		}

		public async Task<bool> DeleteActivityTypeViewMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(ActivityTypeViewMembers, ItemID));
		}


		public async Task<bool> HasWriteAccessToEmployeeAsync(int AccountID,int EmployeeID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(GetGrantedEmployees(GetProfileID(AccountID)).FirstOrDefault(item => (item.EmployeeID == EmployeeID) && (item.WriteAccess==true)) != null);
		}

		public async Task<bool> HasWriteAccessToActivityAsync(int AccountID,int ActivityID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			Activity activity = Activities.First(item => item.ActivityID == ActivityID);
			return await Task.FromResult(GetGrantedEmployees(GetProfileID(AccountID)).FirstOrDefault(item => (item.EmployeeID == activity.EmployeeID) && (item.WriteAccess==true) ) != null);
		}


	}
}
