CREATE VIEW [dbo].[GrantsPerProfile]
AS
SELECT        GrantID, ProfileID, GroupID
FROM            dbo.[Grant]
union select -1,1,1


CREATE VIEW [dbo].[GrantedGroupsPerProfile]
AS
WITH RecursiveGroup AS (SELECT dbo.[Grant].ProfileID, dbo.[Group].GroupID
FROM  dbo.[Grant] INNER JOIN
dbo.[Group] ON dbo.[Grant].GroupID = dbo.[Group].GroupID
UNION
SELECT 1 AS ProfileID, 1 AS GroupID
UNION ALL
SELECT        ParentGroup.ProfileID, ChildGroup.GroupID
FROM            dbo.[Group] AS ChildGroup INNER JOIN
RecursiveGroup AS ParentGroup ON ChildGroup.ParentGroupID = ParentGroup.GroupID)
SELECT        ProfileID, GroupID
FROM            RecursiveGroup AS RecursiveGroup

CREATE VIEW [dbo].[GrantedGroupsPerAccount]
AS
SELECT DISTINCT dbo.Account.AccountID, dbo.GrantedGroupsPerProfile.GroupID
FROM            dbo.GrantedGroupsPerProfile INNER JOIN
dbo.Account ON dbo.GrantedGroupsPerProfile.ProfileID = dbo.Account.ProfileID


CREATE VIEW [dbo].[GroupMembers]
AS
SELECT        GroupMemberID,dbo.[Group].GroupID, dbo.GroupMember.EmployeeID
FROM            dbo.GroupMember INNER JOIN
                         dbo.[Group] ON dbo.GroupMember.GroupID = dbo.[Group].GroupID
UNION
SELECT        -1,1, EmployeeID
FROM            Employee

CREATE VIEW [dbo].[GrantedEmployeesPerAccount]
AS
SELECT DISTINCT dbo.Account.AccountID, dbo.GroupMembers.EmployeeID
FROM            dbo.GrantedGroupsPerAccount INNER JOIN
                         dbo.Account ON dbo.GrantedGroupsPerAccount.AccountID = dbo.Account.AccountID INNER JOIN
                         dbo.GroupMembers ON dbo.GrantedGroupsPerAccount.GroupID = dbo.GroupMembers.GroupID
UNION
SELECT        AccountID, EmployeeID
FROM            Account
WHERE        EmployeeID IS NOT NULL







