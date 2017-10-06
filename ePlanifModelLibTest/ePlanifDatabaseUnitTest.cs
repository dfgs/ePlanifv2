using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ePlanifModelsLib;
using System.Threading.Tasks;
using DatabaseModelLib;
using DatabaseModelLibTest;
using System.Linq;

namespace ePlanifModelLibTest
{
	[TestClass]
	public class ePlanifDatabaseUnitTest : BaseUnitTest<ePlanifDatabase>
	{
		private static Profile profile1, profile2;
		private static Employee employee1, employee2;
		private static ActivityType activityType1, activityType2;
		private static Layer layer1, layer2;
		private static Group group1, group2;
		private static Account account1, account2;
		private static EmployeeView employeeView1, employeeView2;
		private static ActivityTypeView activityTypeView1, activityTypeView2;

		[ClassInitialize]
		public static async Task Initialize(TestContext Context)
		{
			profile1 = new Profile() { Name = "test", AdministrateAccounts = true, AdministrateActivityTypes = true, AdministrateEmployees = true, CanRunReports = true, IsDisabled = false,SelfWriteAccess=true };
			profile2 = new Profile() { Name = "test", AdministrateAccounts = true, AdministrateActivityTypes = true, AdministrateEmployees = true, CanRunReports = true, IsDisabled = false, SelfWriteAccess = true };
			await Database.InsertAsync(profile1);
			await Database.InsertAsync(profile2);

			account1 = new Account() { EmployeeID = null, IsDisabled = false, Login = "test", ProfileID = profile1.ProfileID.Value  };
			account2 = new Account() { EmployeeID = null, IsDisabled = false, Login = "test", ProfileID = profile1.ProfileID.Value };
			await Database.InsertAsync(account1);
			await Database.InsertAsync(account2);

			group1 = new Group() { Name = "test", ParentGroupID = 1 };
			group2 = new Group() { Name = "test", ParentGroupID = 1 };
			await Database.InsertAsync(group1);
			await Database.InsertAsync(group2);

			employee1 = new Employee() { CountryCode = "US", FirstName = "test", LastName = "test", IsDisabled = false, MaxWorkingHoursPerWeek = null };
			employee2 = new Employee() { CountryCode = "US", FirstName = "test", LastName = "test", IsDisabled = false, MaxWorkingHoursPerWeek = null };
			await Database.InsertAsync(employee1);
			await Database.InsertAsync(employee2);

			layer1 = new Layer() { Color = "Red", IsDisabled = false, Name = "test" };
			layer2 = new Layer() { Color = "Red", IsDisabled = false, Name = "test" };
			await Database.InsertAsync(layer1);
			await Database.InsertAsync(layer2);

			activityType1 = new ActivityType() { BackgroundColor = "Black", LayerID = layer1.LayerID.Value, MinEmployees = null, IsDisabled = false, Name = "test", TextColor = "White" };
			activityType2 = new ActivityType() { BackgroundColor = "Black", LayerID = layer1.LayerID.Value, MinEmployees = null, IsDisabled = false, Name = "test", TextColor = "White" };
			await Database.InsertAsync(activityType1);
			await Database.InsertAsync(activityType2);

			employeeView1 = new EmployeeView() { AccountID = account1.AccountID.Value, Name = "test" };
			employeeView2 = new EmployeeView() { AccountID = account1.AccountID.Value, Name = "test" };
			await Database.InsertAsync(employeeView1);
			await Database.InsertAsync(employeeView2);

			activityTypeView1 = new ActivityTypeView() { AccountID = account1.AccountID.Value, Name = "test" };
			activityTypeView2 = new ActivityTypeView() { AccountID = account1.AccountID.Value, Name = "test" };
			await Database.InsertAsync(activityTypeView1);
			await Database.InsertAsync(activityTypeView2);
		}

		[ClassCleanup]
		public static async Task Cleanup()
		{
			await Database.DeleteAsync(activityTypeView1);
			await Database.DeleteAsync(activityTypeView2);
			await Database.DeleteAsync(employeeView1);
			await Database.DeleteAsync(employeeView2);
			await Database.DeleteAsync(activityType1);
			await Database.DeleteAsync(activityType2);
			await Database.DeleteAsync(layer1);
			await Database.DeleteAsync(layer2);
			await Database.DeleteAsync(employee1);
			await Database.DeleteAsync(employee2);
			await Database.DeleteAsync(group1);
			await Database.DeleteAsync(group2);
			await Database.DeleteAsync(account1);
			await Database.DeleteAsync(account2);
			await Database.DeleteAsync(profile1);
			await Database.DeleteAsync(profile2);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Activity()
		{
			var row = new Activity() {  ActivityTypeID=activityType1.ActivityTypeID.Value, Comment="Salut", Duration=TimeSpan.FromHours(1), EmployeeID=employee1.EmployeeID.Value,
				ProjectNumber =1234,IsDraft =false, RemedyRef="REMEDY", StartDate=DateTime.Now.Date,
				TrackedDuration =TimeSpan.FromHours(1) };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.Comment = null);
			await AssertUpdateAsync(true, row, (item) => item.Comment = "Comment");
			await AssertUpdateAsync(false, row, (item) => item.ActivityTypeID = null);
			await AssertUpdateAsync(true, row, (item) => item.ActivityTypeID = activityType2.ActivityTypeID.Value);
			await AssertUpdateAsync(false, row, (item) => item.Duration = null);
			await AssertUpdateAsync(true, row, (item) => item.Duration= TimeSpan.FromHours(10));
			await AssertUpdateAsync(false, row, (item) => item.EmployeeID = null);
			await AssertUpdateAsync(true, row, (item) => item.EmployeeID= employee2.EmployeeID.Value);
			await AssertUpdateAsync(true, row, (item) => item.ProjectNumber = null);
			await AssertUpdateAsync(true, row, (item) => item.ProjectNumber = 50);
			await AssertUpdateAsync(false, row, (item) => item.IsDraft = null);
			await AssertUpdateAsync(true, row, (item) => item.IsDraft = false);
			await AssertUpdateAsync(false, row, (item) => item.RemedyRef = null);
			await AssertUpdateAsync(true, row, (item) => item.RemedyRef = "REF");
			await AssertUpdateAsync(false, row, (item) => item.StartDate = null);
			await AssertUpdateAsync(true, row, (item) => item.StartDate = DateTime.Now.Date.AddDays(1));
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_ActivityType()
		{
			var row = new ActivityType() { BackgroundColor="Red", IsDisabled=false, LayerID=layer1.LayerID.Value, MinEmployees=1, Name="test", TextColor="Blue"  };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.Name = null);
			await AssertUpdateAsync(true, row, (item) => item.Name = "test2");
			await AssertUpdateAsync(false, row, (item) => item.IsDisabled = null);
			await AssertUpdateAsync(true, row, (item) => item.IsDisabled = true);
			await AssertUpdateAsync(false, row, (item) => item.BackgroundColor = null);
			await AssertUpdateAsync(true, row, (item) => item.BackgroundColor = "Blue");
			await AssertUpdateAsync(false, row, (item) => item.LayerID = null);
			await AssertUpdateAsync(true, row, (item) => item.LayerID = layer2.LayerID.Value);
			await AssertUpdateAsync(true, row, (item) => item.MinEmployees = null);
			await AssertUpdateAsync(true, row, (item) => item.MinEmployees = 1);
			await AssertUpdateAsync(false, row, (item) => item.TextColor = null);
			await AssertUpdateAsync(true, row, (item) => item.TextColor = "White");
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Employee()
		{
			var row = new Employee() { CountryCode = "FR", FirstName = "test", LastName = "test", IsDisabled = false, MaxWorkingHoursPerWeek = 10 };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.IsDisabled = null);
			await AssertUpdateAsync(true, row, (item) => item.IsDisabled = true);
			await AssertUpdateAsync(false, row, (item) => item.CountryCode = null);
			await AssertUpdateAsync(true, row, (item) => item.CountryCode = "US");
			await AssertUpdateAsync(false, row, (item) => item.FirstName = null);
			await AssertUpdateAsync(true, row, (item) => item.FirstName = "test2");
			await AssertUpdateAsync(false, row, (item) => item.LastName = null);
			await AssertUpdateAsync(true, row, (item) => item.LastName = "test2");
			await AssertUpdateAsync(true, row, (item) => item.MaxWorkingHoursPerWeek = null);
			await AssertUpdateAsync(true, row, (item) => item.MaxWorkingHoursPerWeek = 50);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Account()
		{
			var row = new Account() { EmployeeID = employee1.EmployeeID.Value, IsDisabled = false, Login = "test", ProfileID=profile1.ProfileID.Value };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(true, row, (item) => item.EmployeeID = null);
			await AssertUpdateAsync(true, row, (item) => item.EmployeeID = employee2.EmployeeID.Value);
			await AssertUpdateAsync(false, row, (item) => item.IsDisabled = null);
			await AssertUpdateAsync(true, row, (item) => item.IsDisabled = true);
			await AssertUpdateAsync(false, row, (item) => item.Login = null);
			await AssertUpdateAsync(true, row, (item) => item.Login = "test2");
			await AssertUpdateAsync(false, row, (item) => item.ProfileID= null);
			await AssertUpdateAsync(true, row, (item) => item.ProfileID = profile2.ProfileID.Value);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Profile()
		{
			var row = new Profile() { Name = "test", AdministrateAccounts = true, AdministrateActivityTypes = true, AdministrateEmployees = true, CanRunReports = true, IsDisabled = false,SelfWriteAccess=false };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.Name = null);
			await AssertUpdateAsync(true, row, (item) => item.Name = "test2");
			await AssertUpdateAsync(false, row, (item) => item.IsDisabled = null);
			await AssertUpdateAsync(true, row, (item) => item.IsDisabled = true);
			await AssertUpdateAsync(false, row, (item) => item.AdministrateAccounts = null);
			await AssertUpdateAsync(true, row, (item) => item.AdministrateAccounts = false);
			await AssertUpdateAsync(false, row, (item) => item.AdministrateActivityTypes = null);
			await AssertUpdateAsync(true, row, (item) => item.AdministrateActivityTypes = false);
			await AssertUpdateAsync(false, row, (item) => item.AdministrateEmployees = null);
			await AssertUpdateAsync(true, row, (item) => item.AdministrateEmployees = false);
			await AssertUpdateAsync(false, row, (item) => item.CanRunReports = null);
			await AssertUpdateAsync(true, row, (item) => item.CanRunReports = false);
			await AssertUpdateAsync(false, row, (item) => item.SelfWriteAccess = null);
			await AssertUpdateAsync(true, row, (item) => item.SelfWriteAccess = true);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Group()
		{
			var row = new Group() { Name = "test", ParentGroupID=group1.GroupID.Value   };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.Name = null);
			await AssertUpdateAsync(true, row, (item) => item.Name = "test2");
			await AssertUpdateAsync(true, row, (item) => item.ParentGroupID = null);
			await AssertUpdateAsync(true, row, (item) => item.ParentGroupID = group2.GroupID.Value);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_GroupMember()
		{
			var row = new GroupMember() {  GroupID=group1.GroupID.Value, EmployeeID=employee1.EmployeeID.Value };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.GroupID = null);
			await AssertUpdateAsync(true, row, (item) => item.GroupID = group2.GroupID.Value);
			await AssertUpdateAsync(false, row, (item) => item.EmployeeID = null);
			await AssertUpdateAsync(true, row, (item) => item.EmployeeID = employee2.EmployeeID.Value);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Grant()
		{
			var row = new Grant() {  GroupID=group1.GroupID.Value, ProfileID= profile1.ProfileID.Value, WriteAccess=true};
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.GroupID = null);
			await AssertUpdateAsync(true, row, (item) => item.GroupID = group2.GroupID.Value);
			await AssertUpdateAsync(false, row, (item) => item.ProfileID = null);
			await AssertUpdateAsync(true, row, (item) => item.ProfileID = profile2.ProfileID.Value);
			await AssertUpdateAsync(false, row, (item) => item.WriteAccess = null);
			await AssertUpdateAsync(true, row, (item) => item.WriteAccess= false);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Layer()
		{
			var row = new Layer() {  Color="Black", IsDisabled=false,  Name="test" };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.Name = null);
			await AssertUpdateAsync(true, row, (item) => item.Name = "test2");
			await AssertUpdateAsync(false, row, (item) => item.IsDisabled = null);
			await AssertUpdateAsync(true, row, (item) => item.IsDisabled = true);
			await AssertUpdateAsync(false, row, (item) => item.Color = null);
			await AssertUpdateAsync(true, row, (item) => item.Color = "White");
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_EmployeeView()
		{
			var row = new EmployeeView() { AccountID = account1.AccountID.Value, Name = "test" };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.Name = null);
			await AssertUpdateAsync(true, row, (item) => item.Name = "test2");
			await AssertUpdateAsync(false, row, (item) => item.AccountID = null);
			await AssertUpdateAsync(true, row, (item) => item.AccountID = account2.AccountID.Value);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_ActivityTypeView()
		{
			var row = new ActivityTypeView() { AccountID = account1.AccountID.Value, Name = "test" };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.Name = null);
			await AssertUpdateAsync(true, row, (item) => item.Name = "test2");
			await AssertUpdateAsync(false, row, (item) => item.AccountID = null);
			await AssertUpdateAsync(true, row, (item) => item.AccountID = account2.AccountID.Value);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_EmployeeViewMember()
		{
			var row = new EmployeeViewMember() { EmployeeViewID = employeeView1.EmployeeViewID.Value, EmployeeID = employee1.EmployeeID.Value };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.EmployeeViewID = null);
			await AssertUpdateAsync(true, row, (item) => item.EmployeeViewID = employeeView2.EmployeeViewID.Value);
			await AssertUpdateAsync(false, row, (item) => item.EmployeeID = null);
			await AssertUpdateAsync(true, row, (item) => item.EmployeeID = employee2.EmployeeID.Value);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_ActivityTypeViewMember()
		{
			var row = new ActivityTypeViewMember() { ActivityTypeViewID = activityTypeView1.ActivityTypeViewID.Value, ActivityTypeID = activityType1.ActivityTypeID.Value };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.ActivityTypeViewID = null);
			await AssertUpdateAsync(true, row, (item) => item.ActivityTypeViewID = activityTypeView2.ActivityTypeViewID.Value);
			await AssertUpdateAsync(false, row, (item) => item.ActivityTypeID = null);
			await AssertUpdateAsync(true, row, (item) => item.ActivityTypeID = activityType2.ActivityTypeID.Value);
			await AssertDeleteAsync(true, row);
		}

		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Option()
		{
			var row = new Option() { AccountID = account1.AccountID.Value, FirstDayOfWeek=DayOfWeek.Friday,CalendarWeekRule=System.Globalization.CalendarWeekRule.FirstFourDayWeek  };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.FirstDayOfWeek = DayOfWeek.Monday);
			await AssertUpdateAsync(true, row, (item) => item.CalendarWeekRule= System.Globalization.CalendarWeekRule.FirstFullWeek);
			await AssertDeleteAsync(true, row);
		}



	}
}
