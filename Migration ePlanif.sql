use ePlanifDatabase;

declare @typeJourDelta int ;
declare @typeAstreinteDelta int ;

declare @startMinutes int
declare @startDate datetime
declare @duration TIME
declare @durationMinutes int
declare @durationPause int
declare @durationMorning int
declare @comment nvarchar(max)
declare @activityTypeID int
declare @employeeID int
declare @projectNumber int
declare @remedyRef nvarchar(max)
declare @isDraft bit

declare @stopMinutes int


declare activityCursor CURSOR FORWARD_ONLY FAST_FORWARD READ_ONLY for 
select MinutesDebut,DATEADD(MINUTE,MinutesDebut,DATEADD(DAY,NumJourDsSemaine+(NumSemaineDsAnnee-1)*7 , '2017-01-02 00:00:00'  ) ), -- attention date inversée si OS anglais
DATEADD(MINUTE,Duree, TIMEFROMPARTS(0,0,0,0,0)),Duree,DureePause,Commentaire,NumTypeActivite,NumTech,NumeroAffaire,IsNull(NumeroRemedy,'NA'),0 from ePlanif.dbo.Activite where Annee=2017



delete from ActivityTypeViewMember;
delete from EmployeeViewMember;
delete from GroupMember
delete from [Grant];
delete from Activity;
delete from ActivityType;
update Account set EmployeeID=null;
delete from Employee;


Set Identity_Insert [Employee] On
--
--	Employee from Technicien
--
insert into [Employee] (EmployeeID,FirstName,LastName, MaxWorkingHoursPerWeek,CountryCode, IsDisabled)
select NumTech ,Prenom ,Nom ,40,'FR',0 from EtraliCommon.dbo.Technicien where Valide=1 
Set Identity_Insert [Employee] Off


--
--	Activity type from activity type
--
Set Identity_Insert [ActivityType] On
insert into [ActivityType] (ActivityTypeID,Name,BackgroundColor,TextColor,IsDisabled,LayerID)
select NumTypeActivite ,Description,'LightGreen', Couleur,0,1 from ePlanif.dbo.TypeActivite where Valide=1
select @typeJourDelta=max(ActivityTypeID)+1 from ActivityType;

--
--	Activity type from day type
--
insert into [ActivityType] (ActivityTypeID,Name,BackgroundColor,TextColor,IsDisabled,LayerID)
select NumTypeJour+@typeJourDelta ,Description,Couleur, 'Black',0,1 from EtraliCommon.dbo.TypeJour where NumTypeJour<>1 and Valide=1 
select @typeAstreinteDelta=max(ActivityTypeID)+1 from ActivityType;

--
--	Activity type from on call type
--
insert into [ActivityType] (ActivityTypeID,Name,BackgroundColor,TextColor,IsDisabled,LayerID)
select NumTypeAstreinte+@typeAstreinteDelta ,Description,Couleur, 'Black',0,2 from ePlanif.dbo.TypeAstreinte 
Set Identity_Insert [ActivityType] Off


--
--	Activity
--


OPEN activityCursor
FETCH NEXT FROM activityCursor into @startMinutes,@startDate,@duration,@durationMinutes,@durationPause,@comment,@activityTypeID,@employeeID,@projectNumber,@remedyRef,@isDraft
WHILE @@FETCH_STATUS=0
BEGIN
	if (@remedyRef='') set @remedyRef='NA'
	--print CAST(@startDate as TIME)
	set @stopMinutes=@durationMinutes+@startMinutes
	if (@durationPause=0) or (@stopMinutes<=12*60+30) begin 
		insert into [Activity] (StartDate,Duration,Comment,ActivityTypeID,EmployeeID,ProjectNumber,RemedyRef,IsDraft) values (@startDate,@duration,@comment,@activityTypeID,@employeeID,@projectNumber,@remedyRef,@isDraft )
	end else begin
		set @durationMorning=12*60+30-@startMinutes
		if @durationMorning>0 begin
			insert into [Activity] (StartDate,Duration,Comment,ActivityTypeID,EmployeeID,ProjectNumber,RemedyRef,IsDraft) values (@startDate,DATEADD(MINUTE,@durationMorning, TIMEFROMPARTS(0,0,0,0,0)),@comment,@activityTypeID,@employeeID,@projectNumber,@remedyRef,@isDraft )
		end
		insert into [Activity] (StartDate,Duration,Comment,ActivityTypeID,EmployeeID,ProjectNumber,RemedyRef,IsDraft) values (DATEADD(MINUTE,12*60+30-@startMinutes+@durationPause,@startDate),DATEADD(MINUTE,@durationMinutes-@durationMorning, TIMEFROMPARTS(0,0,0,0,0)),@comment,@activityTypeID,@employeeID,@projectNumber,@remedyRef,@isDraft )
	end

	FETCH NEXT FROM activityCursor into @startMinutes,@startDate,@duration,@durationMinutes,@durationPause,@comment,@activityTypeID,@employeeID,@projectNumber,@remedyRef,@isDraft
END
CLOSE activityCursor
DEALLOCATE activityCursor

--
--	Activity from day 
--

insert into [Activity] (StartDate,Duration,Comment,ActivityTypeID,EmployeeID,ProjectNumber,RemedyRef,IsDraft)
select DATEADD(MINUTE,8*60+30,DATEADD(DAY,NumJourDsSemaine+(NumSemaineDsAnnee-1)*7 , '2017-01-02 00:00:00'  ) ), -- attention date inversée si OS anglais
DATEADD(MINUTE,240, TIMEFROMPARTS(0,0,0,0,0)),Commentaire,NumTypeJour+@typeJourDelta,NumTech,null,'NA',0 from ePlanif.dbo.Jour where Annee=2017 and NumTypeJour<>1
insert into [Activity] (StartDate,Duration,Comment,ActivityTypeID,EmployeeID,ProjectNumber,RemedyRef,IsDraft)
select DATEADD(MINUTE,14*60+00,DATEADD(DAY,NumJourDsSemaine+(NumSemaineDsAnnee-1)*7 , '2017-01-02 00:00:00'  ) ), -- attention date inversée si OS anglais
DATEADD(MINUTE,3*60+32, TIMEFROMPARTS(0,0,0,0,0)),Commentaire,NumTypeJour+@typeJourDelta,NumTech,null,'NA',0 from ePlanif.dbo.Jour where Annee=2017 and NumTypeJour<>1

--
--	Activity from on call
--

insert into [Activity] (StartDate,Duration,Comment,ActivityTypeID,EmployeeID,ProjectNumber,RemedyRef,IsDraft)
select DATEADD(MINUTE,18*60+00,DATEADD(DAY,NumJourDsSemaine+(NumSemaineDsAnnee-1)*7 , '2017-01-02 00:00:00'  ) ), -- attention date inversée si OS anglais
DATEADD(MINUTE,6*60, TIMEFROMPARTS(0,0,0,0,0)),null,NumTypeAstreinte+@typeAstreinteDelta,NumTech,null,'NA',0 from ePlanif.dbo.Astreinte where Annee=2017 
