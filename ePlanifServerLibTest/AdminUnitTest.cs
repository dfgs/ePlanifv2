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
	public class AdminUnitTest:BaseUnitTest
	{

		protected static TestDataProvider dataProvider;
		protected static ePlanifServiceHost serviceHost;


		[ClassInitialize]
		public static void Initialize(TestContext Context)
		{
			dataProvider = new TestDataProvider("admin");
			serviceHost = new ePlanifServiceHost(dataProvider);
			serviceHost.Open();
		}

		[ClassCleanup]
		public static void Cleanup()
		{
			serviceHost.Close();
		}



		[TestMethod]
		public override void TestInstantiateClient()
		{
			using (IePlanifServiceClient client=CreateClient())
			Assert.AreEqual(CommunicationState.Opened, client.State);
		}
		[TestMethod]
		public override void TestGetCurrentAccount()
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				Account result = client.GetCurrentAccount();
				Assert.AreEqual("admin", result.Login);
				Assert.IsFalse(result.IsDisabled.Value);
			}
		}
		[TestMethod]
		public override void TestGetCurrentProfile()
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				Profile result = client.GetCurrentProfile();
				Assert.IsFalse(result.IsDisabled.Value);
			}
		}
		[TestMethod]
		public override void TestGetEmployees()
		{
			AssertGetItems(true, (client) => client.GetEmployees, dataProvider.Employees);
		}
		[TestMethod]
		public override void TestCreateEmployee()
		{
			AssertCreateItem(true, (client) =>  client.CreateEmployee, new Employee() { CountryCode = "FR", FirstName = "Test", LastName = "Test", MaxWorkingHoursPerWeek = null, WriteAccess = false });
		}
		[TestMethod]
		public override void TestUpdateEmployee()
		{
			AssertUpdateItem(true, (client) => client.UpdateEmployee, dataProvider.Employees[0]);
		}
		[TestMethod]
		public override void TestGetActivityTypes()
		{
			AssertGetItems(true, (client) => client.GetActivityTypes, dataProvider.ActivityTypes);
		}
		[TestMethod]
		public override void TestCreateActivityType()
		{
			AssertCreateItem(true, (client) => client.CreateActivityType, new ActivityType() { BackgroundColor="Red", LayerID=1, MinEmployees=1, Name="Test", TextColor="Black"  });
		}
		[TestMethod]
		public override void TestUpdateActivityType()
		{
			AssertUpdateItem(true, (client) => client.UpdateActivityType, dataProvider.ActivityTypes[0]);
		}
		[TestMethod]
		public override void TestGetProfiles()
		{
			AssertGetItems(true, (client) => client.GetProfiles, dataProvider.Profiles);
		}
		[TestMethod]
		public override void TestCreateProfile()
		{
			AssertCreateItem(true, (client) => client.CreateProfile, new Profile() { AdministrateAccounts=true, AdministrateActivityTypes=true, AdministrateEmployees=true, CanRunReports=true, IsDisabled=false, Name="test" } );
		}
		[TestMethod]
		public override void TestUpdateProfile()
		{
			AssertUpdateItem(false, (client) => client.UpdateProfile, dataProvider.Profiles[0]);
			AssertUpdateItem(true, (client) => client.UpdateProfile, dataProvider.Profiles[1]);
		}
		[TestMethod]
		public override void TestGetActivities()
		{
			DateTime date = TestDataProvider.FirstMondayOfWeek(DateTime.Now);
			AssertGetItems(true, (client) => delegate () { return client.GetActivities(date); }, dataProvider.Activities.Where( item=>item.StartDate.Value.Date==date.Date) );
		}
		[TestMethod]
		public override void TestCreateActivity()
		{
			AssertCreateItem(true, (client) => client.CreateActivity, new Activity() { ActivityTypeID=1, Comment="test", Duration=TimeSpan.FromHours(1), EmployeeID=1, IsDraft=false, ProjectNumber=1234, RemedyRef="None", StartDate=DateTime.Now, TrackedDuration=null  });
		}
		[TestMethod]
		public override void TestDeleteActivity()
		{
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 1));
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 2));
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 3));
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 4));
			AssertDeleteItem(true, (client) => client.DeleteActivity, dataProvider.Activities.First(item => item.EmployeeID == 5));
		}
		[TestMethod]
		public override void TestUpdateActivity()
		{
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 1));
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 2));
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 3));
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 4));
			AssertUpdateItem(true, (client) => client.UpdateActivity, dataProvider.Activities.First(item => item.EmployeeID == 5));
		}
		[TestMethod]
		public override void TestGetGroupMembers()
		{
			AssertGetItems(true, (client) => delegate () { return client.GetGroupMembers(1); }, dataProvider.Employees.Select((item) => new GroupMember() { EmployeeID = item.EmployeeID, GroupID = 1, GroupMemberID = -1 }));
			AssertGetItems(true, (client) => delegate () { return client.GetGroupMembers(2); }, Enumerable.Empty<GroupMember>());
			AssertGetItems(true, (client) => delegate () { return client.GetGroupMembers(3); }, dataProvider.GroupMembers.Where(item => item.GroupID == 3));
		}
		[TestMethod]
		public override void TestCreateGroupMember()
		{
			AssertCreateItem(false, (client) => client.CreateGroupMember, new GroupMember() { GroupID = 1, EmployeeID = 1 }); // duplicate must fail
			AssertCreateItem(false, (client) => client.CreateGroupMember, new GroupMember() { GroupID = 4, EmployeeID = 3 }); // duplicate must fail
			AssertCreateItem(true, (client) => client.CreateGroupMember, new GroupMember() { GroupID = 4, EmployeeID = 1 });  // not a duplicate
		}
		[TestMethod]
		public override void TestDeleteGroupMember()
		{
			AssertDeleteItem(false, (client) => client.DeleteGroupMember, new GroupMember() { GroupMemberID = -1 }); // virtual item must fail
			AssertDeleteItem(true, (client) => client.DeleteGroupMember, dataProvider.GroupMembers[0] ); 
		}
		[TestMethod]
		public override void TestGetGrants()
		{
			AssertGetItems(true, (client) => delegate () { return client.GetGrants(1); }, new Grant[] { new Grant() { GrantID = -1, GroupID = 1, WriteAccess = true, ProfileID = 1 } });
			AssertGetItems(true, (client) => delegate () { return client.GetGrants(2); }, dataProvider.Grants.Where(item=>item.ProfileID==2) );
			AssertGetItems(true, (client) => delegate () { return client.GetGrants(3); }, dataProvider.Grants.Where(item => item.ProfileID == 3));
		}
		[TestMethod]
		public override void TestCreateGrant()
		{
			AssertCreateItem(false, (client) => client.CreateGrant, new Grant() { ProfileID=1, GroupID=1 }); // duplicate must fail
			AssertCreateItem(false, (client) => client.CreateGrant, new Grant() { ProfileID=2,GroupID=4 }); // duplicate must fail
			AssertCreateItem(true, (client) => client.CreateGrant, new Grant() { ProfileID=3, GroupID=4 });  // not a duplicate
		}
		[TestMethod]
		public override void TestDeleteGrant()
		{
			AssertDeleteItem(false, (client) => client.DeleteGrant, new Grant() { GrantID = -1 }); // virtual item must fail
			AssertDeleteItem(true, (client) => client.DeleteGrant, dataProvider.Grants[0]);
		}
		[TestMethod]
		public override void TestGetGroups()
		{
			AssertGetItems(true, (client) => client.GetGroups,dataProvider.Groups );
		}
		[TestMethod]
		public override void TestCreateGroup()
		{
			AssertCreateItem(true, (client) => client.CreateGroup, new Group() {  Name="test", ParentGroupID=1 }); 
		}
		[TestMethod]
		public override void TestDeleteGroup()
		{
			AssertDeleteItem(false, (client) => client.DeleteGroup, dataProvider.Groups[0]);
			AssertDeleteItem(true, (client) => client.DeleteGroup, dataProvider.Groups[4]);
		}
		[TestMethod]
		public override void TestUpdateGroup()
		{
			AssertUpdateItem(true, (client) => client.UpdateGroup, dataProvider.Groups[0]);
		}
		[TestMethod]
		public override void TestGetAccounts()
		{
			AssertGetItems(true, (client) => client.GetAccounts, dataProvider.Accounts);
		}
		[TestMethod]
		public override void TestCreateAccount()
		{
			AssertCreateItem(true, (client) => client.CreateAccount, new Account() {  EmployeeID=1, IsDisabled=false, Login="test", ProfileID=1, SelfWriteAccess=true });
		}
		[TestMethod]
		public override void TestUpdateAccount()
		{
			AssertUpdateItem(true, (client) => client.UpdateAccount, dataProvider.Accounts[0]);
		}
		[TestMethod]
		public override void TestGetLayers()
		{
			AssertGetItems(true, (client) => client.GetLayers, dataProvider.Layers);
		}
		[TestMethod]
		public override void TestCreateLayer()
		{
			AssertCreateItem(true, (client) => client.CreateLayer, new Layer() { Color="Red", IsDisabled=false, Name="test" });
		}
		[TestMethod]
		public override void TestUpdateLayer()
		{
			AssertUpdateItem(false, (client) => client.UpdateLayer, new Layer() { LayerID=1, IsDisabled=true });
			AssertUpdateItem(true, (client) => client.UpdateLayer, dataProvider.Layers[1]);
		}
		[TestMethod]
		public override void TestGetEmployeeViews()
		{
			AssertGetItems(true, (client) => client.GetEmployeeViews, dataProvider.EmployeeViews);
		}
		[TestMethod]
		public override void TestCreateEmployeeView()
		{
			AssertCreateItem(false, (client) => client.CreateEmployeeView, new EmployeeView() { AccountID = 999, Name = "test" }); // cannot create view for someone else
			AssertCreateItem(true, (client) => client.CreateEmployeeView, new EmployeeView() { AccountID = 1, Name = "test" }); 
		}
		[TestMethod]
		public override void TestDeleteEmployeeView()
		{
			AssertDeleteItem(false, (client) => client.DeleteEmployeeView, dataProvider.EmployeeViews.First(item => item.AccountID != 1)); // cannot delete view for someone else
			AssertDeleteItem(true, (client) => client.DeleteEmployeeView, dataProvider.EmployeeViews.First(item=>item.AccountID==1)); 
		}
		[TestMethod]
		public override void TestUpdateEmployeeView()
		{
			AssertUpdateItem(false, (client) => client.UpdateEmployeeView, dataProvider.EmployeeViews.First(item => item.AccountID != 1)); // cannot Update view for someone else
			AssertUpdateItem(true, (client) => client.UpdateEmployeeView, dataProvider.EmployeeViews.First(item => item.AccountID == 1));
		}
		[TestMethod]
		public override void TestGetActivityTypeViews()
		{
			AssertGetItems(true, (client) => client.GetActivityTypeViews, dataProvider.ActivityTypeViews);
		}
		[TestMethod]
		public override void TestCreateActivityTypeView()
		{
			AssertCreateItem(false, (client) => client.CreateActivityTypeView, new ActivityTypeView() { AccountID = 2, Name = "test" }); // cannot create view for someone else
			AssertCreateItem(true, (client) => client.CreateActivityTypeView, new ActivityTypeView() { AccountID = 1, Name = "test" }); // cannot create view for someone else
		}
		[TestMethod]
		public override void TestDeleteActivityTypeView()
		{
			AssertDeleteItem(false, (client) => client.DeleteActivityTypeView, dataProvider.ActivityTypeViews.First(item => item.AccountID != 1)); // cannot delete view for someone else
			AssertDeleteItem(true, (client) => client.DeleteActivityTypeView, dataProvider.ActivityTypeViews.First(item => item.AccountID == 1));
		}
		[TestMethod]
		public override void TestUpdateActivityTypeView()
		{
			AssertUpdateItem(false, (client) => client.UpdateActivityTypeView, dataProvider.ActivityTypeViews.First(item => item.AccountID != 1)); // cannot Update view for someone else
			AssertUpdateItem(true, (client) => client.UpdateActivityTypeView, dataProvider.ActivityTypeViews.First(item => item.AccountID == 1));
		}
		[TestMethod]
		public override void TestGetEmployeeViewMembers()
		{
			AssertGetItems(true, (client) => delegate() { return client.GetEmployeeViewMembers(1); }, dataProvider.EmployeeViewMembers.Where(item=>item.EmployeeViewID==1));
		}
		[TestMethod]
		public override void TestCreateEmployeeViewMember()
		{
			AssertCreateItem(false, (client) => client.CreateEmployeeViewMember, new EmployeeViewMember() { EmployeeViewID = 1, EmployeeID = 1 }); // cannot duplicate
			AssertCreateItem(true, (client) => client.CreateEmployeeViewMember, new EmployeeViewMember() { EmployeeViewID = 2, EmployeeID = 3 }); 
		}
		[TestMethod]
		public override void TestDeleteEmployeeViewMember()
		{
			AssertDeleteItem(true, (client) => client.DeleteEmployeeViewMember, dataProvider.EmployeeViewMembers[0] );
		}
		[TestMethod]
		public override void TestGetActivityTypeViewMembers()
		{
			AssertGetItems(true, (client) => delegate () { return client.GetActivityTypeViewMembers(1); }, dataProvider.ActivityTypeViewMembers.Where(item => item.ActivityTypeViewID == 1));
		}
		[TestMethod]
		public override void TestCreateActivityTypeViewMember()
		{
			AssertCreateItem(false, (client) => client.CreateActivityTypeViewMember, new ActivityTypeViewMember() { ActivityTypeViewID = 1, ActivityTypeID = 1 }); // cannot duplicate
			AssertCreateItem(true, (client) => client.CreateActivityTypeViewMember, new ActivityTypeViewMember() { ActivityTypeViewID = 2, ActivityTypeID = 3 });
		}
		[TestMethod]
		public override void TestDeleteActivityTypeViewMember()
		{
			AssertDeleteItem(true, (client) => client.DeleteActivityTypeViewMember, dataProvider.ActivityTypeViewMembers[0]);
		}


	}
}
