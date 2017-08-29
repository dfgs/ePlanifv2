using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePlanifModelsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ePlanifServerLibTest.ePlanifService;

namespace ePlanifServerLibTest
{
	public class TestContextAdmin:TestContext
	{

		public TestContextAdmin(string Domain, string Login, string Password):base(Domain,Login,Password)
		{

		}

		protected override Account OnCreateAccount()
		{
			return new Account() { Login = $"{Domain}\\{Login}", IsDisabled = false, ProfileID = 1 };
		}

		protected override void OnAssertCreateAccount(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateActivity(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateActivityType(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateActivityTypeView(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateActivityTypeViewMember(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateEmployee(IePlanifServiceClient Client)
		{
			createdEmployee = new Employee() { FirstName = "test", LastName = "test", IsDisabled = false, MaxWorkingHoursPerWeek = 10 };
			createdEmployee.EmployeeID = Client.CreateEmployee(createdEmployee);
			Assert.AreNotEqual(-1, createdEmployee.EmployeeID);
		}

		protected override void OnAssertCreateEmployeeView(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateEmployeeViewMember(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateGrant(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateGroup(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateGroupMember(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateLayer(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertCreateProfile(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteActivity(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteActivityTypeView(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteActivityTypeViewMember(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteEmployeeView(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteEmployeeViewMember(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteGrant(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteGroup(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertDeleteGroupMember(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetAccounts(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetActivities(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetActivityTypes(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetActivityTypeViewMembers(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetActivityTypeViews(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetCurrentAccount(IePlanifServiceClient Client)
		{
			Account account = Client.GetCurrentAccount();
			Assert.AreNotEqual(account.AccountID, 0);
			Assert.AreNotEqual(account.AccountID, -1);
			Assert.AreEqual(account.IsDisabled, false);
			Assert.IsTrue(account.Login.ToString().Contains(Login));
			Assert.AreEqual(account.ProfileID, 1);
		}

		protected override void OnAssertGetCurrentProfile(IePlanifServiceClient Client)
		{
			Profile profile = Client.GetCurrentProfile();
			Assert.AreEqual(profile.ProfileID,1);
			Assert.AreEqual(profile.IsDisabled, false);
			Assert.AreEqual(profile.AdministrateAccounts, true);
			Assert.AreEqual(profile.AdministrateActivityTypes, true);
			Assert.AreEqual(profile.AdministrateEmployees, true);
			Assert.AreEqual(profile.CanRunReports, true);
		}

		protected override void OnAssertGetEmployees(IePlanifServiceClient Client)
		{
			Employee[] result = Client.GetEmployees();
			Assert.IsNotNull(result);
		}

		protected override void OnAssertGetEmployeeViewMembers(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetEmployeeViews(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetGrants(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetGroupMembers(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetGroups(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetLayers(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertGetProfiles(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertInstantiateClient(IePlanifServiceClient Client)
		{
			Assert.IsNotNull(Client);
			Assert.AreEqual(System.ServiceModel.CommunicationState.Opened, Client.State);
		}

		protected override void OnAssertUpdateAccount(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertUpdateActivity(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertUpdateActivityType(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertUpdateActivityTypeView(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertUpdateEmployee(IePlanifServiceClient Client)
		{
			existingEmployee.LastName = "Updated test";
			Assert.IsTrue(Client.UpdateEmployee(existingEmployee));
		}

		protected override void OnAssertUpdateEmployeeView(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertUpdateGroup(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertUpdateLayer(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		protected override void OnAssertUpdateProfile(IePlanifServiceClient Client)
		{
			Assert.Fail();
		}

		
	}
}
