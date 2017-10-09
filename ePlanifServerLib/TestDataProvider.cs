using DatabaseModelLib;
using DatabaseModelLib.Filters;
using ePlanifModelsLib;
using LogUtils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkerLib;

namespace ePlanifServerLib
{
	public class TestDataProvider : Worker, IDataProvider
	{
		public List<Activity> Activities = new List<Activity>();
		public List<ActivityType> ActivityTypes = new List<ActivityType>();
		public List<Employee> Employees = new List<Employee>();
		public List<Account> Accounts = new List<Account>();
		public List<Profile> Profiles = new List<Profile>();
		public List<Group> Groups = new List<Group>();
		public List<GroupMember> GroupMembers = new List<GroupMember>();
		public List<Grant> Grants = new List<Grant>();
		public List<Layer> Layers = new List<Layer>();
		public List<EmployeeView> EmployeeViews = new List<EmployeeView>();
		public List<ActivityTypeView> ActivityTypeViews = new List<ActivityTypeView>();
		public List<EmployeeViewMember> EmployeeViewMembers = new List<EmployeeViewMember>();
		public List<ActivityTypeViewMember> ActivityTypeViewMembers = new List<ActivityTypeViewMember>();
		public List<Option> Options = new List<Option>();
		public List<Photo> Photos = new List<Photo>();

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
			Employees.Add(new Employee() { EmployeeID = 4, FirstName = "Lisa", LastName = "Simpson", CountryCode = "US", MaxWorkingHoursPerWeek = null });
			Employees.Add(new Employee() { EmployeeID = 5, FirstName = "Maggie", LastName = "Simpson", CountryCode = "US", MaxWorkingHoursPerWeek = null });

			Photos.Add(new Photo() { PhotoID = 1, EmployeeID = 1, Data = GetPhoto("ePlanifServerLib.Images.homer.png") });
			Photos.Add(new Photo() { PhotoID = 2, EmployeeID = 2, Data = GetPhoto("ePlanifServerLib.Images.marje.png") });
			Photos.Add(new Photo() { PhotoID = 3, EmployeeID = 3, Data = GetPhoto("ePlanifServerLib.Images.bart.png") });
			Photos.Add(new Photo() { PhotoID = 4, EmployeeID = 4, Data = GetPhoto("ePlanifServerLib.Images.lisa.png") });
			Photos.Add(new Photo() { PhotoID = 5, EmployeeID = 5, Data = GetPhoto("ePlanifServerLib.Images.maggie.png") });

			Groups.Add(new Group() { GroupID = 1, Name = "Springfield" });
			Groups.Add(new Group() { GroupID = 2, Name = "Simpsons", ParentGroupID = 1 });
			Groups.Add(new Group() { GroupID = 3, Name = "Parents", ParentGroupID = 2 });
			Groups.Add(new Group() { GroupID = 4, Name = "Children", ParentGroupID = 2 });
			Groups.Add(new Group() { GroupID = 5, Name = "Flanders", ParentGroupID = 1 });

			GroupMembers.Add(new GroupMember() { GroupMemberID = 1, GroupID = 3, EmployeeID = 1 });
			GroupMembers.Add(new GroupMember() { GroupMemberID = 2, GroupID = 3, EmployeeID = 2 });
			GroupMembers.Add(new GroupMember() { GroupMemberID = 3, GroupID = 4, EmployeeID = 3 });
			GroupMembers.Add(new GroupMember() { GroupMemberID = 4, GroupID = 4, EmployeeID = 4 });
			GroupMembers.Add(new GroupMember() { GroupMemberID = 5, GroupID = 4, EmployeeID = 5 });

			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 1, BackgroundColor = "LightGreen", LayerID = 1, MinEmployees = 1, Name = "Monitor nuclear plant", TextColor = "Red" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 2, BackgroundColor = "Green", LayerID = 1, MinEmployees = 0, Name = "Send security report", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 3, BackgroundColor = "LightGreen", LayerID = 1, MinEmployees = 1, Name = "Care of baby", TextColor = "Red" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 4, BackgroundColor = "Green", LayerID = 1, MinEmployees = 0, Name = "Go to kwik&mark", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 5, BackgroundColor = "LightBlue", LayerID = 1, MinEmployees = 2, Name = "Go to school", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 6, BackgroundColor = "Lavender", LayerID = 2, MinEmployees = 0, Name = "Clean house", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 7, BackgroundColor = "Lavender", LayerID = 2, MinEmployees = 0, Name = "Repair car", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 8, BackgroundColor = "Lavender", LayerID = 2, MinEmployees = 1, Name = "Feed pets", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 9, BackgroundColor = "SteelBlue", LayerID = 3, MinEmployees = 0, Name = "Go the Moe's bar", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 10, BackgroundColor = "SteelBlue", LayerID = 3, MinEmployees = 0, Name = "Read book", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 11, BackgroundColor = "LightSteelBlue", LayerID = 3, MinEmployees = 0, Name = "Play sax", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 12, BackgroundColor = "LightSteelBlue", LayerID = 3, MinEmployees = 0, Name = "Play skate", TextColor = "Black" });
			ActivityTypes.Add(new ActivityType() { ActivityTypeID = 13, BackgroundColor = "Pink", LayerID = 1, MinEmployees = 0, Name = "Enjoy life", TextColor = "Blue" });


			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 1, AccountID = 1, Name = "All" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 2, AccountID = 1, Name = "Parents" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 3, AccountID = 1, Name = "Children" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 4, AccountID = 1, Name = "Empty" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 5, AccountID = 1, Name = "TBD" });

			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 6, AccountID = 2, Name = "All" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 7, AccountID = 2, Name = "Parents" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 8, AccountID = 2, Name = "Children" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 9, AccountID = 2, Name = "Empty" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 10, AccountID = 2, Name = "TBD" });

			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 11, AccountID = 3, Name = "All" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 12, AccountID = 3, Name = "Parents" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 13, AccountID = 3, Name = "Children" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 14, AccountID = 3, Name = "Empty" });
			EmployeeViews.Add(new EmployeeView() { EmployeeViewID = 15, AccountID = 3, Name = "TBD" });

			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 1, AccountID = 1, Name = "All" });
			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 2, AccountID = 1, Name = "Empty" });
			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 3, AccountID = 1, Name = "TBD" });

			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 4, AccountID = 2, Name = "All" });
			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 5, AccountID = 2, Name = "Empty" });
			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 6, AccountID = 2, Name = "TBD" });

			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 7, AccountID = 3, Name = "All" });
			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 8, AccountID = 3, Name = "Empty" });
			ActivityTypeViews.Add(new ActivityTypeView() { ActivityTypeViewID = 9, AccountID = 3, Name = "TBD" });


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

			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 11, EmployeeViewID = 6, EmployeeID = 1 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 12, EmployeeViewID = 6, EmployeeID = 2 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 13, EmployeeViewID = 6, EmployeeID = 3 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 14, EmployeeViewID = 6, EmployeeID = 4 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 15, EmployeeViewID = 6, EmployeeID = 5 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 16, EmployeeViewID = 7, EmployeeID = 1 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 17, EmployeeViewID = 7, EmployeeID = 2 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 18, EmployeeViewID = 8, EmployeeID = 3 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 19, EmployeeViewID = 8, EmployeeID = 4 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 20, EmployeeViewID = 8, EmployeeID = 5 });

			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 21, EmployeeViewID = 10, EmployeeID = 1 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 22, EmployeeViewID = 10, EmployeeID = 2 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 23, EmployeeViewID = 10, EmployeeID = 3 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 24, EmployeeViewID = 10, EmployeeID = 4 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 25, EmployeeViewID = 10, EmployeeID = 5 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 26, EmployeeViewID = 11, EmployeeID = 1 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 27, EmployeeViewID = 11, EmployeeID = 2 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 28, EmployeeViewID = 12, EmployeeID = 3 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 29, EmployeeViewID = 12, EmployeeID = 4 });
			EmployeeViewMembers.Add(new EmployeeViewMember() { EmployeeViewMemberID = 30, EmployeeViewID = 12, EmployeeID = 5 });


			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 1, ActivityTypeViewID = 1, ActivityTypeID = 1 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 2, ActivityTypeViewID = 1, ActivityTypeID = 2 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 3, ActivityTypeViewID = 1, ActivityTypeID = 3 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 4, ActivityTypeViewID = 1, ActivityTypeID = 4 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 5, ActivityTypeViewID = 1, ActivityTypeID = 5 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 6, ActivityTypeViewID = 1, ActivityTypeID = 6 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 7, ActivityTypeViewID = 1, ActivityTypeID = 7 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 8, ActivityTypeViewID = 1, ActivityTypeID = 8 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 9, ActivityTypeViewID = 1, ActivityTypeID = 9 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 10, ActivityTypeViewID = 1, ActivityTypeID = 10 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 11, ActivityTypeViewID = 1, ActivityTypeID = 11 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 12, ActivityTypeViewID = 1, ActivityTypeID = 12 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 13, ActivityTypeViewID = 1, ActivityTypeID = 13 });

			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 14, ActivityTypeViewID = 4, ActivityTypeID = 1 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 15, ActivityTypeViewID = 4, ActivityTypeID = 2 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 16, ActivityTypeViewID = 4, ActivityTypeID = 3 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 17, ActivityTypeViewID = 4, ActivityTypeID = 4 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 18, ActivityTypeViewID = 4, ActivityTypeID = 5 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 19, ActivityTypeViewID = 4, ActivityTypeID = 6 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 20, ActivityTypeViewID = 4, ActivityTypeID = 7 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 21, ActivityTypeViewID = 4, ActivityTypeID = 8 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 22, ActivityTypeViewID = 4, ActivityTypeID = 9 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 23, ActivityTypeViewID = 4, ActivityTypeID = 10 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 24, ActivityTypeViewID = 4, ActivityTypeID = 11 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 25, ActivityTypeViewID = 4, ActivityTypeID = 12 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 26, ActivityTypeViewID = 4, ActivityTypeID = 13 });

			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 27, ActivityTypeViewID = 7, ActivityTypeID = 1 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 28, ActivityTypeViewID = 7, ActivityTypeID = 2 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 29, ActivityTypeViewID = 7, ActivityTypeID = 3 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 30, ActivityTypeViewID = 7, ActivityTypeID = 4 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 31, ActivityTypeViewID = 7, ActivityTypeID = 5 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 32, ActivityTypeViewID = 7, ActivityTypeID = 6 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 33, ActivityTypeViewID = 7, ActivityTypeID = 7 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 34, ActivityTypeViewID = 7, ActivityTypeID = 8 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 35, ActivityTypeViewID = 7, ActivityTypeID = 9 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 36, ActivityTypeViewID = 7, ActivityTypeID = 10 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 37, ActivityTypeViewID = 7, ActivityTypeID = 11 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 38, ActivityTypeViewID = 7, ActivityTypeID = 12 });
			ActivityTypeViewMembers.Add(new ActivityTypeViewMember() { ActivityTypeViewMemberID = 39, ActivityTypeViewID = 7, ActivityTypeID = 13 });

			Grants.Add(new Grant() { GrantID = 1, ProfileID = 2, GroupID = 3, WriteAccess = false });
			Grants.Add(new Grant() { GrantID = 2, ProfileID = 2, GroupID = 4, WriteAccess = true });
			Grants.Add(new Grant() { GrantID = 3, ProfileID = 3, GroupID = 2, WriteAccess = true });

			int id = 0;
			DateTime firstDay=FirstDayOfWeek(DateTime.Now);
			DateTime currentDay;
			for(int t=0;t<14;t++)
			{
				currentDay = firstDay.AddDays(t);
				switch(currentDay.DayOfWeek)
				{
					case DayOfWeek.Monday:
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 7, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = true });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 9, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 3, StartDate = currentDay.AddMinutes(420), Duration = TimeSpan.FromMinutes(480), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 10, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false});

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 5, ActivityTypeID = 13, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(600), TrackedDuration = null, IsDraft = false });
						break;
					case DayOfWeek.Tuesday:
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 1, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(420), TrackedDuration = TimeSpan.FromMinutes(210), IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 9, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 3, StartDate = currentDay.AddMinutes(420), Duration = TimeSpan.FromMinutes(480), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 10, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 5, ActivityTypeID = 13, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(600), TrackedDuration = null, IsDraft = false });
						break;
					case DayOfWeek.Wednesday:
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 1, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(420), TrackedDuration = TimeSpan.FromMinutes(210), IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 9, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 10, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 4, StartDate = currentDay.AddMinutes(600), Duration = TimeSpan.FromMinutes(60), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 12, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 3, StartDate = currentDay.AddMinutes(420), Duration = TimeSpan.FromMinutes(480), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 11, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 5, ActivityTypeID = 13, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(600), TrackedDuration = null, IsDraft = false });
						break;
					case DayOfWeek.Thursday:
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 1, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(420), TrackedDuration = TimeSpan.FromMinutes(210), IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 9, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 3, StartDate = currentDay.AddMinutes(420), Duration = TimeSpan.FromMinutes(480), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 4, StartDate = currentDay.AddMinutes(600), Duration = TimeSpan.FromMinutes(60), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 10, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 5, ActivityTypeID = 13, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(600), TrackedDuration = null, IsDraft = false });
						break;
					case DayOfWeek.Friday:
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 1, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(420), TrackedDuration = TimeSpan.FromMinutes(210), IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 2, StartDate = currentDay.AddMinutes(960), Duration = TimeSpan.FromMinutes(60), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 9, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 3, StartDate = currentDay.AddMinutes(420), Duration = TimeSpan.FromMinutes(480), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 10, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 5, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 5, ActivityTypeID = 13, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(600), TrackedDuration = null, IsDraft = false });
						break;
					case DayOfWeek.Saturday:
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 7, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = true });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 9, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 3, StartDate = currentDay.AddMinutes(420), Duration = TimeSpan.FromMinutes(480), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 10, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 12, StartDate = currentDay.AddMinutes(480), Duration = TimeSpan.FromMinutes(420), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 11, StartDate = currentDay.AddMinutes(480), Duration = TimeSpan.FromMinutes(420), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 5, ActivityTypeID = 13, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(600), TrackedDuration = null, IsDraft = false });
						break;
					case DayOfWeek.Sunday:
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 7, StartDate = currentDay.AddMinutes(840), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = true });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 6, StartDate = currentDay.AddMinutes(480), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 1, ActivityTypeID = 9, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 3, StartDate = currentDay.AddMinutes(420), Duration = TimeSpan.FromMinutes(480), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 8, StartDate = currentDay.AddMinutes(1200), Duration = TimeSpan.FromMinutes(360), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 2, ActivityTypeID = 10, StartDate = currentDay.AddMinutes(1260), Duration = TimeSpan.FromMinutes(180), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 3, ActivityTypeID = 12, StartDate = currentDay.AddMinutes(480), Duration = TimeSpan.FromMinutes(420), TrackedDuration = null, IsDraft = false });
						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 4, ActivityTypeID = 11, StartDate = currentDay.AddMinutes(480), Duration = TimeSpan.FromMinutes(420), TrackedDuration = null, IsDraft = false });

						Activities.Add(new Activity() { ActivityID = id++, EmployeeID = 5, ActivityTypeID = 13, StartDate = currentDay.AddMinutes(540), Duration = TimeSpan.FromMinutes(600), TrackedDuration = null, IsDraft = false });
						break;
				}
			}


			Options.Add(new Option() { OptionID = 1, AccountID = 1, FirstDayOfWeek= DayOfWeek.Monday,CalendarWeekRule= CalendarWeekRule.FirstDay });
			Options.Add(new Option() { OptionID = 2, AccountID = 2, FirstDayOfWeek = DayOfWeek.Monday, CalendarWeekRule = CalendarWeekRule.FirstDay });
			Options.Add(new Option() { OptionID = 3, AccountID = 3, FirstDayOfWeek = DayOfWeek.Monday, CalendarWeekRule = CalendarWeekRule.FirstDay });

		}

		public byte[] GetPhoto(string Name)
		{
			byte[] buffer;
			Stream stream;

			stream=Assembly.GetCallingAssembly().GetManifestResourceStream(Name);
			buffer = new byte[stream.Length];
			stream.Read(buffer, 0, (int)stream.Length);

			return buffer;
		}
		public static DateTime FirstDayOfWeek(DateTime Date)
		{
			int diff = Date.DayOfWeek - CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			if (diff < 0) diff += 7;
			return Date.AddDays(-1 * diff).Date;
		}
		public static DateTime FirstMondayOfWeek(DateTime Date)
		{
			DateTime date = FirstDayOfWeek(Date);
			while (date.DayOfWeek != DayOfWeek.Monday) date=date.AddDays(1);
			return date;
		}

		private int GetProfileID(int AccountID)
		{
			return Accounts.First(item => item.AccountID == AccountID).ProfileID.Value;
		}

		private IEnumerable<Grant> GetGrantsPerProfile()
		{
			return
				(from grant in Grants
				select grant).Union(new Grant[] { new Grant() { GrantID=-1,GroupID=1,ProfileID=1,WriteAccess=true } } );
		}

		private IEnumerable<GroupMember> GetGroupMembers()
		{
			return
				(from groupMember in GroupMembers
					select groupMember).Union( 
					from employee in Employees
					select new GroupMember() { GroupMemberID=-1, EmployeeID=employee.EmployeeID,GroupID=1 }
					);
		}

		private IEnumerable<(int ParentGroupID, Group Group)> GetGroupChildren(int ParentGroupID)
		{
			foreach(Group group in Groups.Where(item=>item.ParentGroupID==ParentGroupID))
			{
				yield return (ParentGroupID,group);
				foreach((int,Group) item in GetGroupChildren(group.GroupID.Value))
				{
					yield return (ParentGroupID, item.Item2);
				}
			}
		}

		private IEnumerable<(int ParentGroupID,Group Group)> GetGroupHierarchy()
		{
			foreach (Group group in Groups)
			{
				yield return (group.GroupID.Value,group);
				foreach ((int,Group) item in GetGroupChildren(group.GroupID.Value))
				{
					yield return (group.GroupID.Value, item.Item2);
				}
			}
		}

		private IEnumerable<Group> GetGrantedGroups(int ProfileID)
		{
			return (from grant in GetGrantsPerProfile()
					join grantedGroup in GetGroupHierarchy() on grant.GroupID equals grantedGroup.ParentGroupID
					select grantedGroup.Group).Distinct();
		}

		private IEnumerable<Employee> GetGrantedEmployees(int ProfileID)
		{
			return from grant in GetGrantsPerProfile() where grant.ProfileID==ProfileID
				   join grantedGroup in GetGroupHierarchy() on grant.GroupID equals grantedGroup.ParentGroupID
				   join member in GroupMembers on grantedGroup.Group.GroupID equals member.GroupID
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


		public async Task<IEnumerable<Photo>> GetPhotosAsync(int EmployeeID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Photos.Where(item => item.EmployeeID == EmployeeID));
		}
		public async Task<int> CreatePhotoAsync(Photo Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert<Photo>(Photos, Item));
		}
		public async Task<bool> DeletePhotoAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete<Photo>(Photos, ItemID));
		}
		public async Task<bool> UpdatePhotoAsync(Photo Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update<Photo>(Photos, Item));
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
				foreach(GroupMember member in GroupMembers.Where(item => item.GroupID == GroupID))
				{
					result.Add(member);
				}
			}
			return await Task.FromResult(result);
		}

		public async Task<int> CreateGroupMemberAsync(GroupMember Item)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(GroupMembers, Item));
		}

		public async Task<bool> DeleteGroupMemberAsync(int ItemID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Delete(GroupMembers, ItemID));
		}



		public async Task<IEnumerable<Grant>> GetGrantsAsync(int ProfileID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return  await Task.FromResult( from grant in GetGrantsPerProfile() where grant.ProfileID==ProfileID select grant );
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




		public async Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int AccountID, int ViewID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			List<Employee> grantedEmployees = new List<Employee>(GetGrantedEmployees(GetProfileID(AccountID)));
			List<EmployeeViewMember> result;

			result = new List<EmployeeViewMember>();
			foreach (EmployeeViewMember member in EmployeeViewMembers.Where(item => item.EmployeeViewID == ViewID))
			{
				if (grantedEmployees.FirstOrDefault(item => item.EmployeeID == member.EmployeeID) == null) continue;
				result.Add(member);
			}
			return await Task.FromResult(result);
		}
		public async Task<IEnumerable<EmployeeViewMember>> GetEmployeeViewMembersAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			List<Employee> grantedEmployees = new List<Employee>(GetGrantedEmployees(GetProfileID(AccountID)));
			List<EmployeeViewMember> result;

			result = new List<EmployeeViewMember>();
			foreach (EmployeeViewMember member in EmployeeViewMembers)
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
		public async Task<IEnumerable<ActivityTypeViewMember>> GetActivityTypeViewMembersAsync()
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(ActivityTypeViewMembers);
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

		public async Task<Option> GetOptionAsync(int AccountID)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Options.FirstOrDefault(item => item.AccountID == AccountID));
		}

		public async Task<bool> CreateOptionAsync(Option Option)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Insert(Options, Option) > 0);
		}

		public async Task<bool> UpdateOptionAsync(Option Option)
		{
			WriteLog(LogLevels.Debug, LogActions.Enter);
			return await Task.FromResult(Update(Options, Option));
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
