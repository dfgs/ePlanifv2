using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ePlanifServerLibTest.ePlanifService;
using System.Security.Principal;
using ePlanifModelsLib;
using System.ServiceModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using ePlanifServerLib;
using DatabaseModelLib;
using System.Linq;

namespace ePlanifServerLibTest
{
		
	[TestClass]
	public class LizaUnitTest:BaseUnitTest
	{
		protected override int ServicePort
		{
			get { return 8524; }
		}

		protected static TestDataProvider dataProvider;
		protected static ePlanifServiceHost serviceHost;

		private static int accountID = 2;
		private static int otherAccountID = 1;
		private static string login = "liza";


		#region Initialize
		[ClassInitialize]
		public static void Initialize(TestContext Context)
		{
			dataProvider = new TestDataProvider(login);
			serviceHost = new ePlanifServiceHost(dataProvider,8524);
			serviceHost.Open();
		}
		[ClassCleanup]
		public static void Cleanup()
		{
			serviceHost.Close();
		}
		#endregion

		#region Connection
		[TestMethod,TestCategory("Connection")]
		public void Should_Success_When_InstanceClient()
		{
			using (IePlanifServiceClient client=CreateClient())
			Assert.AreEqual(CommunicationState.Opened, client.State);
		}
		[TestMethod, TestCategory("Connection")]
		public void Should_Success_When_GetCurrentAccount()
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				Account result = client.GetCurrentAccount();
				Assert.AreEqual(login, result.Login);
				Assert.IsFalse(result.IsDisabled.Value);
			}
		}
		[TestMethod, TestCategory("Connection")]
		public void Should_Success_When_GetCurrentProfile()
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				Profile result = client.GetCurrentProfile();
				Assert.IsFalse(result.IsDisabled.Value);
			}
		}
		#endregion

		#region Employee
		[TestMethod, TestCategory("Employee")]
		public void Should_Success_When_GetEmployees()
		{
			AssertGetItems(true, (client) => client.GetEmployees, dataProvider.Employees);
		}
		[TestMethod, TestCategory("Employee")]
		public void Should_Fail_When_CreateEmployee()
		{
			AssertCreateItem(false, (client) =>  client.CreateEmployee, new Employee() { CountryCode = "FR", FirstName = "Test", LastName = "Test", MaxWorkingHoursPerWeek = null, WriteAccess = false });
		}
		[TestMethod, TestCategory("Employee")]
		public void Should_Fail_When_UpdateEmployee()
		{
			AssertUpdateItem(false, (client) => client.UpdateEmployee, dataProvider.Employees[0]);
		}
		#endregion

		#region ActivityType
		[TestMethod, TestCategory("ActivityType")]
		public void Should_Success_When_GetActivityTypes()
		{
			AssertGetItems(true, (client) => client.GetActivityTypes, dataProvider.ActivityTypes);
		}
		[TestMethod, TestCategory("ActivityType")]
		public void Should_Fail_When_CreateActivityType()
		{
			AssertCreateItem(false, (client) => client.CreateActivityType, new ActivityType() { BackgroundColor="Red", LayerID=1, MinEmployees=1, Name="Test", TextColor="Black"  });
		}
		[TestMethod, TestCategory("ActivityType")]
		public void Should_Fail_When_UpdateActivityType()
		{
			AssertUpdateItem(false, (client) => client.UpdateActivityType, dataProvider.ActivityTypes[0]);
		}
		#endregion

		#region Profile
		[TestMethod, TestCategory("Profile")]
		public void Should_Fail_When_GetProfiles()
		{
			AssertGetItems(false, (client) => client.GetProfiles, dataProvider.Profiles);
		}
		[TestMethod, TestCategory("Profile")]
		public void Should_Fail_When_CreateProfile()
		{
			AssertCreateItem(false, (client) => client.CreateProfile, new Profile() { AdministrateAccounts=true, AdministrateActivityTypes=true, AdministrateEmployees=true, CanRunReports=true, IsDisabled=false, Name="test" } );
		}
		[TestMethod, TestCategory("Profile")]
		public void Should_Fail_When_UpdateProfile()
		{
			AssertUpdateItem(false, (client) => client.UpdateProfile, dataProvider.Profiles[1]);
		}
		[TestMethod, TestCategory("Profile")]
		public void Should_Fail_When_UpdateAdminProfile()
		{
			AssertUpdateItem(false, (client) => client.UpdateProfile, dataProvider.Profiles[0]);
		}
		#endregion

		#region Activity
		[TestMethod, TestCategory("Activity")]
		public void Should_Success_When_GetActivities()
		{
			DateTime date = TestDataProvider.FirstMondayOfWeek(DateTime.Now);
			AssertGetItems(true, (client) => delegate () { return client.GetActivities(date); }, dataProvider.Activities.Where( item=>item.StartDate.Value.Date==date.Date) );
		}
		[TestMethod, TestCategory("Activity")]
		public void Should_Success_When_CreateActivity()
		{
			AssertCreateItem(true, (client) => client.CreateActivity, new Activity() { ActivityTypeID = 1, Comment = "test", Duration = TimeSpan.FromHours(1), EmployeeID = 4, IsDraft = false, ProjectNumber = 1234, RemedyRef = "None", StartDate = DateTime.Now, TrackedDuration = null });
		}
		[TestMethod, TestCategory("Activity")]
		public void Should_Fail_When_CreateActivityForAnotherEmployee()
		{
			AssertCreateItem(false, (client) => client.CreateActivity, new Activity() { ActivityTypeID = 1, Comment = "test", Duration = TimeSpan.FromHours(1), EmployeeID = 1, IsDraft = false, ProjectNumber = 1234, RemedyRef = "None", StartDate = DateTime.Now, TrackedDuration = null });
		}
		[TestMethod, TestCategory("Activity")]
		public void Should_Success_When_DeleteActivity()
		{
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 3));
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 4));
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 5));
		}
		[TestMethod, TestCategory("Activity")]
		public void Should_Fail_When_DeleteActivityForAnotherEmployee()
		{
			AssertDeleteItem(false, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 1));
			AssertDeleteItem(false, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 2));
		}
		[TestMethod, TestCategory("Activity")]
		public void Should_Success_When_UpdateActivity()
		{
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 3));
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 4));
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 5));
		}
		[TestMethod, TestCategory("Activity")]
		public void Should_Fail_When_UpdateActivitForAnotherEmployeey()
		{
			AssertUpdateItem(false, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 1));
			AssertUpdateItem(false, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 2));
		}
		#endregion

		#region GroupMember
		[TestMethod, TestCategory("GroupMember")]
		public void Should_Fail_When_GetRootGroupMembers()
		{
			AssertGetItems(false, (client) => delegate () { return client.GetGroupMembers(1); }, dataProvider.Employees.Select((item) => new GroupMember() { EmployeeID = item.EmployeeID, GroupID = 1, GroupMemberID = -1 }));
		}
		[TestMethod, TestCategory("GroupMember")]
		public void Should_Fail_When_GetEmptyGroupMembers()
		{
			AssertGetItems(false, (client) => delegate () { return client.GetGroupMembers(2); }, Enumerable.Empty<GroupMember>());
		}
		[TestMethod, TestCategory("GroupMember")]
		public void Should_Fail_When_GetGroupMembers()
		{
			AssertGetItems(false, (client) => delegate () { return client.GetGroupMembers(3); }, dataProvider.GroupMembers.Where(item => item.GroupID == 3));
		}
		[TestMethod, TestCategory("GroupMember")]
		public void Should_Fail_When_DuplicateGroupMember()
		{
			AssertCreateItem(false, (client) => client.CreateGroupMember, new GroupMember() { GroupID = 1, EmployeeID = 1 }); // duplicate must fail
			AssertCreateItem(false, (client) => client.CreateGroupMember, new GroupMember() { GroupID = 4, EmployeeID = 3 }); // duplicate must fail
		}
		[TestMethod, TestCategory("GroupMember")]
		public void Should_Fail_When_CreateGroupMember()
		{
			AssertCreateItem(false, (client) => client.CreateGroupMember, new GroupMember() { GroupID = 4, EmployeeID = 1 });  // not a duplicate
		}
		[TestMethod, TestCategory("GroupMember")]
		public void Should_Fail_When_DeleteVirtualGroupMember()
		{
			AssertDeleteItem(false, (client) => client.DeleteGroupMember, new GroupMember() { GroupMemberID = -1 }); // virtual item must fail
		}
		[TestMethod, TestCategory("GroupMember")]
		public void Should_Fail_When_DeleteGroupMember()
		{
			AssertDeleteItem(false, (client) => client.DeleteGroupMember, dataProvider.GroupMembers[0]);
		}
		#endregion

		#region Grant
		[TestMethod, TestCategory("Grant")]
		public void Should_Fail_When_GetVirtualGrants()
		{
			AssertGetItems(false, (client) => delegate () { return client.GetGrants(1); }, new Grant[] { new Grant() { GrantID = -1, GroupID = 1, WriteAccess = true, ProfileID = 1 } });
		}
		[TestMethod, TestCategory("Grant")]
		public void Should_Fail_When_GetGrants()
		{
			AssertGetItems(false, (client) => delegate () { return client.GetGrants(2); }, dataProvider.Grants.Where(item => item.ProfileID == 2));
			AssertGetItems(false, (client) => delegate () { return client.GetGrants(3); }, dataProvider.Grants.Where(item => item.ProfileID == 3));
		}
		[TestMethod, TestCategory("Grant")]
		public void Should_Fail_When_DuplicateGrant()
		{
			AssertCreateItem(false, (client) => client.CreateGrant, new Grant() { ProfileID = 1, GroupID = 1 }); // duplicate must fail
			AssertCreateItem(false, (client) => client.CreateGrant, new Grant() { ProfileID = 2, GroupID = 4 }); // duplicate must fail
		}
		[TestMethod, TestCategory("Grant")]
		public void Should_Fail_When_CreateGrant()
		{
			AssertCreateItem(false, (client) => client.CreateGrant, new Grant() { ProfileID = 3, GroupID = 4 });  // not a duplicate
		}
		[TestMethod, TestCategory("Grant")]
		public void Should_Fail_When_DeleteVirtualGrant()
		{
			AssertDeleteItem(false, (client) => client.DeleteGrant, new Grant() { GrantID = -1 }); // virtual item must fail
		}
		[TestMethod, TestCategory("Grant")]
		public void Should_Fail_When_DeleteGrant()
		{
			AssertDeleteItem(false, (client) => client.DeleteGrant, dataProvider.Grants[0]);
		}
		#endregion

		#region Group
		[TestMethod, TestCategory("Group")]
		public void Should_Success_When_GetGroups()
		{
			AssertGetItems(true, (client) => client.GetGroups,dataProvider.Groups );
		}
		[TestMethod, TestCategory("Group")]
		public void Should_Fail_When_CreateGroup()
		{
			AssertCreateItem(false, (client) => client.CreateGroup, new Group() {  Name="test", ParentGroupID=1 }); 
		}
		[TestMethod, TestCategory("Group")]
		public void Should_Fail_When_DeleteRootGroup()
		{
			AssertDeleteItem(false, (client) => client.DeleteGroup, dataProvider.Groups[0]);
		}
		[TestMethod, TestCategory("Group")]
		public void Should_Fail_When_DeleteGroup()
		{
			AssertDeleteItem(false, (client) => client.DeleteGroup, dataProvider.Groups[4]);
		}
		[TestMethod, TestCategory("Group")]
		public void Should_Fail_When_UpdateGroup()
		{
			AssertUpdateItem(false, (client) => client.UpdateGroup, dataProvider.Groups[0]);
		}
		#endregion

		#region Account
		[TestMethod, TestCategory("Account")]
		public void Should_Fail_When_GetAccounts()
		{
			AssertGetItems(false, (client) => client.GetAccounts, dataProvider.Accounts);
		}
		[TestMethod, TestCategory("Account")]
		public void Should_Fail_When_CreateAccount()
		{
			AssertCreateItem(false, (client) => client.CreateAccount, new Account() {  EmployeeID=1, IsDisabled=false, Login="test", ProfileID=1 });
		}
		[TestMethod, TestCategory("Account")]
		public void Should_Fail_When_UpdateAccount()
		{
			AssertUpdateItem(false, (client) => client.UpdateAccount, dataProvider.Accounts[0]);
		}
		#endregion

		#region Layer
		[TestMethod, TestCategory("Layer")]
		public void Should_Success_When_GetLayers()
		{
			AssertGetItems(true, (client) => client.GetLayers, dataProvider.Layers);
		}
		[TestMethod, TestCategory("Layer")]
		public void Should_Fail_When_CreateLayer()
		{
			AssertCreateItem(false, (client) => client.CreateLayer, new Layer() { Color="Red", IsDisabled=false, Name="test" });
		}
		[TestMethod, TestCategory("Layer")]
		public void Should_Fail_When_DisableDefaultLayer()
		{
			AssertUpdateItem(false, (client) => client.UpdateLayer, new Layer() { LayerID = 1, IsDisabled = true });
		}
		[TestMethod, TestCategory("Layer")]
		public void Should_Fail_When_UpdateLayer()
		{
			AssertUpdateItem(false, (client) => client.UpdateLayer, dataProvider.Layers[0]);
		}
		#endregion

		#region EmployeeView
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Success_When_GetEmployeeViews()
		{
			AssertGetItems(true, (client) => client.GetEmployeeViews, dataProvider.EmployeeViews.Where(item=>item.AccountID==accountID));
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Fail_When_CreateEmployeeViewForAnotherAccount()
		{
			AssertCreateItem(false, (client) => client.CreateEmployeeView, new EmployeeView() { AccountID = otherAccountID, Name = "test" }); // cannot create view for someone else
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Success_When_CreateEmployeeView()
		{
			AssertCreateItem(true, (client) => client.CreateEmployeeView, new EmployeeView() { AccountID = accountID, Name = "test" });
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Fail_When_DeleteEmployeeViewForAnotherAccount()
		{
			EmployeeView other;
			other = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			AssertDeleteItem(false, (client) =>  client.DeleteEmployeeView, other); 
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Fail_When_DeleteHackedEmployeeView()
		{
			EmployeeView other, hack;
			other = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			hack = new EmployeeView();Schema<EmployeeView>.Clone(other, hack);
			hack.EmployeeViewID = other.EmployeeViewID; hack.AccountID = accountID;// try to hack data
			AssertDeleteItem(false, (client) => client.DeleteEmployeeView, hack); // cannot hack data
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Success_When_DeleteEmployeeView()
		{
			AssertUpdateItem(true, (client) => client.UpdateEmployeeView, dataProvider.EmployeeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value == "TBD")));
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Fail_When_UpdateEmployeeViewForAnotherAccount()
		{
			EmployeeView other;
			other = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			AssertUpdateItem(false, (client) => client.UpdateEmployeeView, other);
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Fail_When_UpdateHackedEmployeeView()
		{
			EmployeeView other, hack;
			other = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			hack = new EmployeeView(); Schema<EmployeeView>.Clone(other, hack);
			hack.EmployeeViewID = other.EmployeeViewID; hack.AccountID = accountID;// try to hack data
			AssertUpdateItem(false, (client) => client.UpdateEmployeeView, hack); // cannot hack data
		}
		[TestMethod, TestCategory("EmployeeView")]
		public void Should_Success_When_UpdateEmployeeView()
		{
			AssertUpdateItem(true, (client) => client.UpdateEmployeeView, dataProvider.EmployeeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value == "TBD")));
		}
		#endregion

		#region ActivityTypeView
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Success_When_GetActivityTypeViews()
		{
			AssertGetItems(true, (client) => client.GetActivityTypeViews, dataProvider.ActivityTypeViews.Where(item => item.AccountID == accountID));
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Fail_When_CreateActivityTypeViewForAnotherAccount()
		{
			AssertCreateItem(false, (client) => client.CreateActivityTypeView, new ActivityTypeView() { AccountID = otherAccountID, Name = "test" }); // cannot create view for someone else
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Success_When_CreateActivityTypeView()
		{
			AssertCreateItem(true, (client) => client.CreateActivityTypeView, new ActivityTypeView() { AccountID = accountID, Name = "test" });
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Fail_When_DeleteActivityTypeViewForAnotherAccount()
		{
			ActivityTypeView other;
			other = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			AssertDeleteItem(false, (client) => client.DeleteActivityTypeView, other);
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Fail_When_DeleteHackedActivityTypeView()
		{
			ActivityTypeView other, hack;
			other = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			hack = new ActivityTypeView(); Schema<ActivityTypeView>.Clone(other, hack);
			hack.ActivityTypeViewID = other.ActivityTypeViewID; hack.AccountID = accountID;// try to hack data
			AssertDeleteItem(false, (client) => client.DeleteActivityTypeView, hack); // cannot hack data
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Success_When_DeleteActivityTypeView()
		{
			AssertUpdateItem(true, (client) => client.UpdateActivityTypeView, dataProvider.ActivityTypeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value == "TBD")));
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Fail_When_UpdateActivityTypeViewForAnotherAccount()
		{
			ActivityTypeView other;
			other = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			AssertUpdateItem(false, (client) => client.UpdateActivityTypeView, other);
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Fail_When_UpdateHackedActivityTypeView()
		{
			ActivityTypeView other, hack;
			other = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "TBD"));
			hack = new ActivityTypeView(); Schema<ActivityTypeView>.Clone(other, hack);
			hack.ActivityTypeViewID = other.ActivityTypeViewID; hack.AccountID = accountID;// try to hack data
			AssertUpdateItem(false, (client) => client.UpdateActivityTypeView, hack); // cannot hack data
		}
		[TestMethod, TestCategory("ActivityTypeView")]
		public void Should_Success_When_UpdateActivityTypeView()
		{
			AssertUpdateItem(true, (client) => client.UpdateActivityTypeView, dataProvider.ActivityTypeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value == "TBD")));
		}
		#endregion

		#region EmployeeViewMember
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Success_When_GetEmployeeViewMembers()
		{
			int viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).EmployeeViewID.Value;
			AssertGetItems(true, (client) => delegate () { return client.GetEmployeeViewMembers(viewID); }, dataProvider.EmployeeViewMembers.Where(item => item.EmployeeViewID == viewID));
		}
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Fail_When_GetEmployeeViewMembersForAnotherAccount()
		{
			int viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value != "Empty")).EmployeeViewID.Value;
			AssertGetItems(false, (client) => delegate () { return client.GetEmployeeViewMembers(viewID); }, dataProvider.EmployeeViewMembers.Where(item => item.EmployeeViewID == viewID));
		}
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Fail_When_DuplicateEmployeeViewMember()
		{
			EmployeeViewMember existing;
			int viewID;
			
			viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).EmployeeViewID.Value;
			existing = dataProvider.EmployeeViewMembers.First(item => item.EmployeeViewID == viewID);
			AssertCreateItem(false, (client) => client.CreateEmployeeViewMember, new EmployeeViewMember() { EmployeeViewID = existing.EmployeeViewID, EmployeeID = existing.EmployeeID }); // cannot duplicate
		}
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Success_When_CreateEmployeeViewMember()
		{
			int viewID;

			viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value == "Empty")).EmployeeViewID.Value;
			AssertCreateItem(true, (client) => client.CreateEmployeeViewMember, new EmployeeViewMember() { EmployeeViewID = viewID, EmployeeID = 1 });
		}
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Fail_When_CreateEmployeeViewMemberForAnotherAccount()
		{
			int viewID;

			viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "Empty")).EmployeeViewID.Value;
			AssertCreateItem(false, (client) => client.CreateEmployeeViewMember, new EmployeeViewMember() { EmployeeViewID = viewID, EmployeeID = 1 });
		}
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Success_When_DeleteEmployeeViewMember()
		{
			EmployeeViewMember existing;
			int viewID;

			viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).EmployeeViewID.Value;
			existing = dataProvider.EmployeeViewMembers.First(item => item.EmployeeViewID == viewID);
			AssertDeleteItem(true, (client) => client.DeleteEmployeeViewMember, existing);
		}
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Fail_When_DeleteEmployeeViewMemberForAnotherAccount()
		{
			EmployeeViewMember existing;
			int viewID;

			viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value != "Empty")).EmployeeViewID.Value;
			existing = dataProvider.EmployeeViewMembers.First(item => item.EmployeeViewID == viewID);
			AssertDeleteItem(false, (client) => client.DeleteEmployeeViewMember, existing);
		}
		[TestMethod, TestCategory("EmployeeViewMember")]
		public void Should_Success_When_DeleteHackedEmployeeViewMember()
		{
			EmployeeViewMember existing, hack;
			int viewID;

			viewID = dataProvider.EmployeeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value != "Empty")).EmployeeViewID.Value;
			existing = dataProvider.EmployeeViewMembers.First(item => item.EmployeeViewID == viewID);
			hack = new EmployeeViewMember();Schema<EmployeeViewMember>.Clone(existing, hack);
			hack.EmployeeViewMemberID = existing.EmployeeViewMemberID;hack.EmployeeViewID = dataProvider.EmployeeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).EmployeeViewID.Value;// try to hack data
			AssertDeleteItem(false, (client) =>  client.DeleteEmployeeViewMember, hack);
		}
		#endregion

		#region ActivityTypeViewMember
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Success_When_GetActivityTypeViewMembers()
		{
			int viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).ActivityTypeViewID.Value;
			AssertGetItems(true, (client) => delegate () { return client.GetActivityTypeViewMembers(viewID); }, dataProvider.ActivityTypeViewMembers.Where(item => item.ActivityTypeViewID == viewID));
		}
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Fail_When_GetActivityTypeViewMembersForAnotherAccount()
		{
			int viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value != "Empty")).ActivityTypeViewID.Value;
			AssertGetItems(false, (client) => delegate () { return client.GetActivityTypeViewMembers(viewID); }, dataProvider.ActivityTypeViewMembers.Where(item => item.ActivityTypeViewID == viewID));
		}
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Fail_When_DuplicateActivityTypeViewMember()
		{
			ActivityTypeViewMember existing;
			int viewID;

			viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).ActivityTypeViewID.Value;
			existing = dataProvider.ActivityTypeViewMembers.First(item => item.ActivityTypeViewID == viewID);
			AssertCreateItem(false, (client) => client.CreateActivityTypeViewMember, new ActivityTypeViewMember() { ActivityTypeViewID = existing.ActivityTypeViewID, ActivityTypeID = existing.ActivityTypeID }); // cannot duplicate
		}
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Success_When_CreateActivityTypeViewMember()
		{
			int viewID;

			viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value == "Empty")).ActivityTypeViewID.Value;
			AssertCreateItem(true, (client) => client.CreateActivityTypeViewMember, new ActivityTypeViewMember() { ActivityTypeViewID = viewID, ActivityTypeID = 1 });
		}
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Fail_When_CreateActivityTypeViewMemberForAnotherAccount()
		{
			int viewID;

			viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value == "Empty")).ActivityTypeViewID.Value;
			AssertCreateItem(false, (client) => client.CreateActivityTypeViewMember, new ActivityTypeViewMember() { ActivityTypeViewID = viewID, ActivityTypeID = 1 });
		}
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Success_When_DeleteActivityTypeViewMember()
		{
			ActivityTypeViewMember existing;
			int viewID;

			viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).ActivityTypeViewID.Value;
			existing = dataProvider.ActivityTypeViewMembers.First(item => item.ActivityTypeViewID == viewID);
			AssertDeleteItem(true, (client) => client.DeleteActivityTypeViewMember, existing);
		}
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Fail_When_DeleteActivityTypeViewMemberForAnotherAccount()
		{
			ActivityTypeViewMember existing;
			int viewID;

			viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value != "Empty")).ActivityTypeViewID.Value;
			existing = dataProvider.ActivityTypeViewMembers.First(item => item.ActivityTypeViewID == viewID);
			AssertDeleteItem(false, (client) => client.DeleteActivityTypeViewMember, existing);
		}
		[TestMethod, TestCategory("ActivityTypeViewMember")]
		public void Should_Success_When_DeleteHackedActivityTypeViewMember()
		{
			ActivityTypeViewMember existing, hack;
			int viewID;

			viewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == otherAccountID) && (item.Name.Value.Value != "Empty")).ActivityTypeViewID.Value;
			existing = dataProvider.ActivityTypeViewMembers.First(item => item.ActivityTypeViewID == viewID);
			hack = new ActivityTypeViewMember(); Schema<ActivityTypeViewMember>.Clone(existing, hack);
			hack.ActivityTypeViewMemberID = existing.ActivityTypeViewMemberID; hack.ActivityTypeViewID = dataProvider.ActivityTypeViews.First(item => (item.AccountID == accountID) && (item.Name.Value.Value != "Empty")).ActivityTypeViewID.Value;// try to hack data
			AssertDeleteItem(false, (client) => client.DeleteActivityTypeViewMember, hack);
		}
		#endregion

	}
}
