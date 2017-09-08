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
	public class ePlanifDatabaseUnitTest:BaseUnitTest<ePlanifDatabase>
	{
		[TestMethod,TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Employee()
		{
			var row = new Employee() { CountryCode = "FR", FirstName = "test", LastName = "test", IsDisabled = false, MaxWorkingHoursPerWeek = 10 };
			await AssertInsertAsync(true,row);
			await AssertUpdateAsync(false, row, (item) => item.CountryCode = null);
			await AssertUpdateAsync(true, row, (item) => item.CountryCode = "US");
			await AssertUpdateAsync(false, row, (item) => item.FirstName = null);
			await AssertUpdateAsync(true, row, (item) => item.FirstName = "test2");
			await AssertUpdateAsync(false, row, (item) => item.LastName = null);
			await AssertUpdateAsync(true, row, (item) => item.LastName = "test2");
			await AssertUpdateAsync(false, row, (item) => item.IsDisabled = null);
			await AssertUpdateAsync(true, row, (item) => item.IsDisabled = true);
			await AssertUpdateAsync(true, row, (item) => item.MaxWorkingHoursPerWeek = null);
			await AssertUpdateAsync(true, row, (item) => item.MaxWorkingHoursPerWeek = 50);
			await AssertDeleteAsync(true, row);
		}
		[TestMethod, TestCategory("CRUD")]
		public async Task Should_Success_When_CRUD_Activity()
		{
			int[] employeeIDs = (await Database.SelectAsync<Employee>()).Select(item=>item.EmployeeID.Value).Take(2).ToArray();
			int[] activityTypeIDs = (await Database.SelectAsync<ActivityType>()).Select(item=>item.ActivityTypeID.Value).Take(2).ToArray();

			var row = new Activity() {  ActivityTypeID=activityTypeIDs[0], Comment="Salut", Duration=TimeSpan.FromHours(1), EmployeeID=employeeIDs[0],
				ProjectNumber =1234,IsDraft =false, RemedyRef="REMEDY", StartDate=DateTime.Now.Date,
				TrackedDuration =TimeSpan.FromHours(1) };
			await AssertInsertAsync(true, row);
			await AssertUpdateAsync(false, row, (item) => item.ActivityTypeID = null);
			await AssertUpdateAsync(true, row, (item) => item.ActivityTypeID = activityTypeIDs[1]);
			await AssertUpdateAsync(false, row, (item) => item.Comment = null);
			await AssertUpdateAsync(true, row, (item) => item.Comment = "Comment");
			await AssertUpdateAsync(false, row, (item) => item.Duration = null);
			await AssertUpdateAsync(true, row, (item) => item.Duration= TimeSpan.FromHours(10));
			await AssertUpdateAsync(false, row, (item) => item.EmployeeID = null);
			await AssertUpdateAsync(true, row, (item) => item.EmployeeID= employeeIDs[1]);
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
	}
}
