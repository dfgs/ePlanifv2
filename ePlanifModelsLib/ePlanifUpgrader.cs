using DatabaseModelLib;
using DatabaseModelLib.Filters;
using DatabaseUpgraderLib;
using SqlDatabaseUpgraderLib;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ePlanifModelsLib
{
	public class ePlanifUpgrader : SqlDatabaseUpgrader
	{
		public ePlanifUpgrader(ePlanifDatabase Database) : base(Database)
		{
		}

		

		protected override IEnumerable<SqlCommand> OnGetCustomSchemaCommands()
		{

			yield return new SqlCommand(
				"CREATE VIEW[dbo].[GrantsPerProfile] " +
				"AS " +
				"SELECT        GrantID, ProfileID, GroupID, WriteAccess " +
				"FROM            dbo.[Grant] " +
				"union select -1,1,1,CAST(1 as BIT)");

			yield return new SqlCommand(
				"CREATE VIEW [dbo].[GrantedGroupsPerProfile] " +
				"AS " +
				"WITH RecursiveGroup AS(SELECT  dbo.GrantsPerProfile.ProfileID, dbo.[Group].GroupID, CAST(dbo.GrantsPerProfile.WriteAccess as int) as WriteAccess " +
				"FROM            dbo.GrantsPerProfile INNER JOIN " +
				"dbo.[Group] ON dbo.GrantsPerProfile.GroupID = dbo.[Group].GroupID " +
				"UNION ALL " +
				"SELECT        ParentGroup.ProfileID, ChildGroup.GroupID, WriteAccess " +
				"FROM            dbo.[Group] AS ChildGroup INNER JOIN RecursiveGroup AS ParentGroup ON ChildGroup.ParentGroupID = ParentGroup.GroupID) " +
				"SELECT distinct ProfileID, GroupID, CAST(MAX(WriteAccess) as bit) as WriteAccess FROM RecursiveGroup AS RecursiveGroup group by ProfileID, GroupID ");

			yield return new SqlCommand(
				"CREATE VIEW[dbo].[GrantedGroupsPerAccount] " +
				"AS " +
				"SELECT DISTINCT dbo.Account.AccountID, dbo.GrantedGroupsPerProfile.GroupID,dbo.GrantedGroupsPerProfile.WriteAccess " +
				"FROM            dbo.GrantedGroupsPerProfile INNER JOIN " +
				"dbo.Account ON dbo.GrantedGroupsPerProfile.ProfileID = dbo.Account.ProfileID "
				);

			yield return new SqlCommand(
				"CREATE VIEW[dbo].[GroupMembers] " +
				"AS " +
				"SELECT        GroupMemberID, dbo.[Group].GroupID, dbo.GroupMember.EmployeeID " +
				"FROM            dbo.GroupMember INNER JOIN " +
				"dbo.[Group] ON dbo.GroupMember.GroupID = dbo.[Group].GroupID " +
				"UNION " +
				"SELECT - 1, 1, EmployeeID " +
				"FROM            Employee"
				);

			yield return new SqlCommand(
				"CREATE VIEW[dbo].[GrantedEmployeesPerAccount] " +
				"AS " +
				"select distinct AccountID, EmployeeID, CAST(MAX(WriteAccess) as bit) as WriteAccess " +
				"from " +
				"(SELECT dbo.Account.AccountID, dbo.GroupMembers.EmployeeID, CAST(dbo.GrantedGroupsPerAccount.WriteAccess as int) as WriteAccess " +
				"FROM            dbo.GrantedGroupsPerAccount INNER JOIN " +
				"						 dbo.Account ON dbo.GrantedGroupsPerAccount.AccountID = dbo.Account.AccountID INNER JOIN " +
				"						 dbo.GroupMembers ON dbo.GrantedGroupsPerAccount.GroupID = dbo.GroupMembers.GroupID " +
				"UNION " +
				"SELECT  AccountID, EmployeeID, CAST(SelfWriteAccess as int) " +
				"FROM    Account inner join [Profile] on Account.ProfileID=[Profile].ProfileID " +
				"WHERE   EmployeeID IS NOT NULL) as tmp " +
				"group by AccountID, EmployeeID");

			yield return new SqlCommand(
				"CREATE FUNCTION[dbo].[HasWriteAccessToEmployee](@AccountID int, @EmployeeID int) RETURNS bit " +
				"AS BEGIN " +
					"DECLARE @result bit " +
					"SELECT @result =[WriteAccess] from[dbo].[GrantedEmployeesPerAccount] " +
					"where EmployeeID = @EmployeeID and AccountID = @AccountID " +
					"RETURN @result " +
				"END "
			);

			yield return new SqlCommand(
				"CREATE FUNCTION[dbo].[HasWriteAccessToActivity](@AccountID int, @ActivityID int) RETURNS bit " +
				"AS BEGIN " +
					"DECLARE @result bit " +
					"SELECT @result =[WriteAccess] from [dbo].[GrantedEmployeesPerAccount] inner join Activity On GrantedEmployeesPerAccount.EmployeeID=Activity.EmployeeID " +
					"where ActivityID = @ActivityID and AccountID = @AccountID " +
					"RETURN @result " +
				"END "
			);

			yield return new SqlCommand("CREATE ROLE db_procexecutor;");

			yield return new SqlCommand("GRANT EXECUTE TO db_procexecutor;");

			yield return Database.CreateInsertCommand(new Profile() { Name = "Administrator", AdministrateAccounts = true, AdministrateActivityTypes = true, AdministrateEmployees = true, CanRunReports = true }, 0);
			yield return Database.CreateInsertCommand(new Group() { Name = "Company" }, 0);
			yield return Database.CreateInsertCommand(new Layer() { Name = "Default" }, 0);



		}


	}
}
