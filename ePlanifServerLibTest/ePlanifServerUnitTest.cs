using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ePlanifServerLibTest.ePlanifService;
using System.Security.Principal;
using ePlanifModelsLib;
using System.ServiceModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ePlanifServerLibTest
{

	//
	// You must create Windows users in order to run unit tests
	// ePlanifUnitTestAdmin 
	// ePlanifUnitTestUser 
	// ePlanifUnitTestInvalid 
	// ePlanifUnitTestDisa 
	//

	[TestClass]
	public class ePlanifServerUnitTest:IDisposable
	{
		
		private List<TestContext> contextes;
	
		public void Dispose()
		{
			foreach (TestContext context in contextes)
			{
				
			}
			contextes.Clear();
		}

		[TestInitialize]
		public async Task Initialize()
		{
			contextes = new List<TestContext>();
			contextes.Add(new TestContextAdmin(".", "ePlanifUnitTestAdmin", "ePlanifUnitTestAdmin"));
			contextes.Add(new TestContextUser(".", "ePlanifUnitTestUser", "ePlanifUnitTestUser"));
			contextes.Add(new TestContextInvalid(".", "ePlanifUnitTestInval", "ePlanifUnitTestInvalid"));
			contextes.Add(new TestContextDisable(".", "ePlanifUnitTestDisa", "ePlanifUnitTestDisable"));

			foreach (TestContext context in contextes)
			{
				await context.InitializeAsync();
			}
			//Microsoft.VisualStudio.TestTools.UnitTesting.
		}
	
		[TestCleanup]
		public async Task Cleanup()
		{
			foreach (TestContext context in contextes)
			{
				await context.CleanupAsync();
			}
			contextes.Clear();
		}

		[TestMethod,TestProperty("toto", "1")]
		public void TestInstantiateClient()
		{
			
			foreach (TestContext context in contextes)
			{
				context.TestInstantiateClient();
			}
		}

		[TestMethod]
		public void TestGetEmployees()
		{
			foreach (TestContext context in contextes)
			{
				context.TestGetEmployees();
			}
		}

		[TestMethod]
		public void TestCreateEmployee()
		{
			foreach (TestContext context in contextes)
			{
				context.TestCreateEmployee();
			}
		}

		[TestMethod]
		public void TestUpdateEmployee()
		{
			foreach (TestContext context in contextes)
			{
				context.TestUpdateEmployee();
			}
		}


		[TestMethod]
		public void TestGetActivityTypes()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateActivityType()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateActivityType()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetProfiles()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateProfile()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateProfile()
		{
			Assert.Fail();
		}


		[TestMethod]
		public void TestGetActivities()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateActivity()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteActivity()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateActivity()
		{
			Assert.Fail();
		}


		[TestMethod]
		public void TestGetGroupMembers()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateGroupMember()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteGroupMember()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetGrants()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateGrant()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteGrant()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetGroups()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateGroup()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteGroup()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateGroup()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetAccounts()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateAccount()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateAccount()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetCurrentAccount()
		{
			foreach (TestContext context in contextes)
			{
				context.TestGetCurrentAccount();
			}
		}

		[TestMethod]
		public void TestGetCurrentProfile()
		{
			foreach (TestContext context in contextes)
			{
				context.TestGetCurrentProfile();
			}
		}



		[TestMethod]
		public void TestGetLayers()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateLayer()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateLayer()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetEmployeeViews()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateEmployeeView()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteEmployeeView()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateEmployeeView()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetActivityTypeViews()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateActivityTypeView()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteActivityTypeView()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestUpdateActivityTypeView()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TestGetEmployeeViewMembers()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateEmployeeViewMember()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteEmployeeViewMember()
		{
			Assert.Fail();
		}



		[TestMethod]
		public void TestGetActivityTypeViewMembers()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestCreateActivityTypeViewMember()
		{
			Assert.Fail();
		}
		[TestMethod]
		public void TestDeleteActivityTypeViewMember()
		{
			Assert.Fail();
		}














	}
}
