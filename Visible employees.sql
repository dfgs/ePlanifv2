use ePlanifDatabase

DECLARE @AccountID integer;
DECLARE @ProfileID integer;
DECLARE @EmployeeID integer;

set @AccountID=1;
set @ProfileID=2;
set @EmployeeID=5;


With ParentGroup(GroupID, [Name], Level) 
AS 
( 
SELECT ParentGroup.GroupID, ParentGroup.[Name], 0 as Level 
FROM[Group] as ParentGroup 
where[ParentGroup].GroupID in  
	(select GroupID from[Grant] where ProfileID = @ProfileID) 
UNION ALL 
select[Group].GroupID,[Group].[Name],Level+1 
FROM[Group] inner join ParentGroup on [Group].ParentGroupID=ParentGroup.GroupID 
) 
select distinct * from
(
select Employee.* from 
ParentGroup inner join Member On ParentGroup.GroupID=Member.GroupID 
inner join Employee on Member.EmployeeID= Employee.EmployeeID 
union
select Employee.* from Employee where EmployeeID=@EmployeeID
) as Employee

left join(select * from EmployeeFilter where EmployeeFilter.AccountID = @AccountID) as EmployeeFilter 
on Employee.EmployeeID = EmployeeFilter.EmployeeID  

where Employee.IsDisabled = 0 and EmployeeFilterID is null 


order by LastName, FirstName;
