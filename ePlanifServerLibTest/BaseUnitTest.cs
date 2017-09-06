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

	
	//[TestClass]
	public abstract class BaseUnitTest
	{


		

		protected IePlanifServiceClient CreateClient()
		{
			IePlanifServiceClient client;

			client = new IePlanifServiceClient("ePlanif", "net.tcp://localhost:8523/ePlanif");
			client.Open();

			return client;
		}

		protected void AssertCollectionAreIdentical<ItemType>(IEnumerable<ItemType> List1,IEnumerable<ItemType> List2)
		{
			ItemType[] l1, l2;
			l1 = List1.OrderBy(item => Schema<ItemType>.PrimaryKey.GetValue(item)).ToArray();
			l2 = List2.OrderBy(item => Schema<ItemType>.PrimaryKey.GetValue(item)).ToArray();

			Assert.AreEqual(l1.Length,l2.Length, "Collection are not identical");
			for (int t = 0; t < l1.Length; t++)
			{
				if (!Schema<ItemType>.AreEquals(l1[t], l2[t])) Assert.Fail("Collection are not identical");
			}
		}

		public void AssertGetItems<ItemType>(bool SuccessExpected, Func<IePlanifServiceClient, Func<IEnumerable<ItemType>>> Func, IEnumerable<ItemType> Items)
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				var deleg = Func(client);
				var result = deleg.Invoke();
				AssertCollectionAreIdentical<ItemType>(Items, result);
			}
		}

		public void AssertCreateItem<ItemType>(bool SuccessExpected, Func<IePlanifServiceClient, Func<ItemType, int>> Func,ItemType Item)
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				var deleg = Func(client);
				var result = deleg.Invoke(Item);
				Assert.AreEqual(SuccessExpected,( result!=-1) ,"Creation failed");
			}
		}

		public void AssertUpdateItem<ItemType>(bool SuccessExpected, Func<IePlanifServiceClient, Func<ItemType, bool>> Func,ItemType Item)
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				var deleg = Func(client);
				var result=deleg.Invoke(Item);
				Assert.AreEqual(SuccessExpected, result,"Update failed");
			}
		}

		public void AssertDeleteItem<ItemType>(bool SuccessExpected, Func<IePlanifServiceClient, Func<int, bool>> Func, ItemType Item)
		{
			using (IePlanifServiceClient client = CreateClient())
			{
				var deleg = Func(client);
				var result = deleg.Invoke((int)Schema<ItemType>.PrimaryKey.GetValue(Item));
				Assert.AreEqual(SuccessExpected, result, "Deletion failed");
			}
		}


		public abstract void TestInstantiateClient();
		public abstract void TestGetEmployees();
		public abstract void TestCreateEmployee();
		public abstract void TestUpdateEmployee();
		public abstract void TestGetActivityTypes();
		public abstract void TestCreateActivityType();
		public abstract void TestUpdateActivityType();
		public abstract void TestGetProfiles();
		public abstract void TestCreateProfile();
		public abstract void TestUpdateProfile();
		public abstract void TestGetActivities();
		public abstract void TestCreateActivity();
		public abstract void TestDeleteActivity();
		public abstract void TestUpdateActivity();
		public abstract void TestGetGroupMembers();
		public abstract void TestCreateGroupMember();
		public abstract void TestDeleteGroupMember();
		public abstract void TestGetGrants();
		public abstract void TestCreateGrant();
		public abstract void TestDeleteGrant();
		public abstract void TestGetGroups();
		public abstract void TestCreateGroup();
		public abstract void TestDeleteGroup();
		public abstract void TestUpdateGroup();
		public abstract void TestGetAccounts();
		public abstract void TestCreateAccount();
		public abstract void TestUpdateAccount();
		public abstract void TestGetCurrentAccount();
		public abstract void TestGetCurrentProfile();
		public abstract void TestGetLayers();
		public abstract void TestCreateLayer();
		public abstract void TestUpdateLayer();
		public abstract void TestGetEmployeeViews();
		public abstract void TestCreateEmployeeView();
		public abstract void TestDeleteEmployeeView();
		public abstract void TestUpdateEmployeeView();
		public abstract void TestGetActivityTypeViews();
		public abstract void TestCreateActivityTypeView();
		public abstract void TestDeleteActivityTypeView();
		public abstract void TestUpdateActivityTypeView();
		public abstract void TestGetEmployeeViewMembers();
		public abstract void TestCreateEmployeeViewMember();
		public abstract void TestDeleteEmployeeViewMember();
		public abstract void TestGetActivityTypeViewMembers();
		public abstract void TestCreateActivityTypeViewMember();
		public abstract void TestDeleteActivityTypeViewMember();


	}
}
