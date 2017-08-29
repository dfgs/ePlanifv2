using ePlanifModelsLib;
using ePlanifServerLibTest.ePlanifService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifServerLibTest
{
	public abstract class TestContext
	{
		private ePlanifDatabase db;

		private string domain;
		public string Domain
		{
			get { return domain; }
		}
		private string login;
		public string Login
		{
			get { return login; }
		}
		private string password;
		public string Password
		{
			get { return password; }
		}

		private Account account;
		public Account Account
		{
			get { return account; }
		}
		protected Employee existingEmployee;
		protected Employee createdEmployee;

		public TestContext(string Domain, string Login, string Password)
		{
			this.domain = Domain; this.login = Login; this.password = Password;
			db = new ePlanifDatabase("localhost");
		}

		public async Task InitializeAsync()
		{
			account = OnCreateAccount();
			if (account != null) await db.InsertAsync(account);

			existingEmployee = new Employee() { FirstName = "test", LastName = "test", MaxWorkingHoursPerWeek = 10 };
			await db.InsertAsync(existingEmployee);
		}

		public async Task CleanupAsync()
		{
			if (account != null)
				await db.DeleteAsync(account);
			await db.DeleteAsync(existingEmployee);
			if (createdEmployee != null) await db.DeleteAsync(createdEmployee);
		}


		protected abstract Account OnCreateAccount();


		protected abstract void OnAssertInstantiateClient(IePlanifServiceClient Client);
		protected abstract void OnAssertGetEmployees(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateEmployee(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateEmployee(IePlanifServiceClient Client);
		protected abstract void OnAssertGetActivityTypes(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateActivityType(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateActivityType(IePlanifServiceClient Client);
		protected abstract void OnAssertGetProfiles(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateProfile(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateProfile(IePlanifServiceClient Client);
		protected abstract void OnAssertGetActivities(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateActivity(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteActivity(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateActivity(IePlanifServiceClient Client);
		protected abstract void OnAssertGetGroupMembers(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateGroupMember(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteGroupMember(IePlanifServiceClient Client);
		protected abstract void OnAssertGetGrants(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateGrant(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteGrant(IePlanifServiceClient Client);
		protected abstract void OnAssertGetGroups(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateGroup(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteGroup(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateGroup(IePlanifServiceClient Client);
		protected abstract void OnAssertGetAccounts(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateAccount(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateAccount(IePlanifServiceClient Client);
		protected abstract void OnAssertGetCurrentAccount(IePlanifServiceClient Client);
		protected abstract void OnAssertGetCurrentProfile(IePlanifServiceClient Client);
		protected abstract void OnAssertGetLayers(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateLayer(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateLayer(IePlanifServiceClient Client);
		protected abstract void OnAssertGetEmployeeViews(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateEmployeeView(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteEmployeeView(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateEmployeeView(IePlanifServiceClient Client);
		protected abstract void OnAssertGetActivityTypeViews(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateActivityTypeView(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteActivityTypeView(IePlanifServiceClient Client);
		protected abstract void OnAssertUpdateActivityTypeView(IePlanifServiceClient Client);
		protected abstract void OnAssertGetEmployeeViewMembers(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateEmployeeViewMember(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteEmployeeViewMember(IePlanifServiceClient Client);
		protected abstract void OnAssertGetActivityTypeViewMembers(IePlanifServiceClient Client);
		protected abstract void OnAssertCreateActivityTypeViewMember(IePlanifServiceClient Client);
		protected abstract void OnAssertDeleteActivityTypeViewMember(IePlanifServiceClient Client);








		public void TestInstantiateClient()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertInstantiateClient(session.Client); }
		}
		public void TestGetEmployees()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetEmployees(session.Client); }
		}
		public void TestCreateEmployee()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) {  OnAssertCreateEmployee(session.Client); }
		}
		public void TestUpdateEmployee()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateEmployee(session.Client); }
		}
		public void TestGetActivityTypes()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetActivityTypes(session.Client); }
		}
		public void TestCreateActivityType()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateActivityType(session.Client); }
		}
		public void TestUpdateActivityType()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateActivityType(session.Client); }
		}
		public void TestGetProfiles()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetProfiles(session.Client); }
		}
		public void TestCreateProfile()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateProfile(session.Client); }
		}
		public void TestUpdateProfile()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateProfile(session.Client); }
		}
		public void TestGetActivities()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetActivities(session.Client); }
		}
		public void TestCreateActivity()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateActivity(session.Client); }
		}
		public void TestDeleteActivity()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteActivity(session.Client); }
		}
		public void TestUpdateActivity()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateActivity(session.Client); }
		}
		public void TestGetGroupMembers()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetGroupMembers(session.Client); }
		}
		public void TestCreateGroupMember()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateGroupMember(session.Client); }
		}
		public void TestDeleteGroupMember()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteGroupMember(session.Client); }
		}
		public void TestGetGrants()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetGrants(session.Client); }
		}
		public void TestCreateGrant()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateGrant(session.Client); }
		}
		public void TestDeleteGrant()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteGrant(session.Client); }
		}
		public void TestGetGroups()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetGroups(session.Client); }
		}
		public void TestCreateGroup()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateGroup(session.Client); }
		}
		public void TestDeleteGroup()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteGroup(session.Client); }
		}
		public void TestUpdateGroup()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateGroup(session.Client); }
		}
		public void TestGetAccounts()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetAccounts(session.Client); }
		}
		public void TestCreateAccount()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateAccount(session.Client); }
		}
		public void TestUpdateAccount()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateAccount(session.Client); }
		}
		public void TestGetCurrentAccount()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetCurrentAccount(session.Client); }
		}
		public void TestGetCurrentProfile()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetCurrentProfile(session.Client); }
		}
		public void TestGetLayers()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetLayers(session.Client); }
		}
		public void TestCreateLayer()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateLayer(session.Client); }
		}
		public void TestUpdateLayer()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateLayer(session.Client); }
		}
		public void TestGetEmployeeViews()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetEmployeeViews(session.Client); }
		}
		public void TestCreateEmployeeView()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateEmployeeView(session.Client); }
		}
		public void TestDeleteEmployeeView()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteEmployeeView(session.Client); }
		}
		public void TestUpdateEmployeeView()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateEmployeeView(session.Client); }
		}
		public void TestGetActivityTypeViews()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetActivityTypeViews(session.Client); }
		}
		public void TestCreateActivityTypeView()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateActivityTypeView(session.Client); }
		}
		public void TestDeleteActivityTypeView()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteActivityTypeView(session.Client); }
		}
		public void TestUpdateActivityTypeView()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertUpdateActivityTypeView(session.Client); }
		}
		public void TestGetEmployeeViewMembers()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetEmployeeViewMembers(session.Client); }
		}
		public void TestCreateEmployeeViewMember()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateEmployeeViewMember(session.Client); }
		}
		public void TestDeleteEmployeeViewMember()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteEmployeeViewMember(session.Client); }
		}
		public void TestGetActivityTypeViewMembers()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertGetActivityTypeViewMembers(session.Client); }
		}
		public void TestCreateActivityTypeViewMember()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertCreateActivityTypeViewMember(session.Client); }
		}
		public void TestDeleteActivityTypeViewMember()
		{
			using(ImpersonatedSession session=new ImpersonatedSession(domain,login,password)) { OnAssertDeleteActivityTypeViewMember(session.Client); }
		}



	}


}
