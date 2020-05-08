use LMS_Database

go
-------------------------------------------------------
-------------------------INSTRUCTOR--------------------
-------------------------------------------------------
create table Instructor (InsId int identity(1,1) primary key not null ,
FName nvarchar(50) not null ,LName nvarchar(50) not null,
[Email] varchar(300) not null unique , [Password] varchar(20) not null)

alter table Instructor add InsImage nvarchar(max) not null
alter table Instructor add Introductory_Video varchar(max) null
alter table Instructor add Expertise varchar(max) not null

alter table Instructor alter column [Password] varchar(200) not null
delete from Instructor

select * from Instructor
-----------------------------Instructor Procedures-------------------------------
----------instructor by id-------
go
alter proc getInstructorById @id int
as
begin
                                     
declare @InsId int

select @InsId =
 InstructorId from tblInstructorLogin where InstructorId = @id 
if(@InsId is not null)
 begin
 
   select 1 as [Online],*from Instructor where InsId = @id
 end
 else
   begin
     select 0 as [Online],*from Instructor where InsId = @id
   end
end

exec getInstructorById  1011
--***************for reset password*******************--
create table tblResetPasswordForInstructors (Id  uniqueIdentifier primary key , InsId int foreign key references Instructor(insId),
resetDateTime DateTime not null)

--***************spResetPassword**************
go
alter proc spResetPassword @Email varchar(300)
as 
begin 

declare @InstId int 
declare @FName varchar(50)

select @InstId = InsId , @FName = FName from Instructor
where [Email] = @Email

if (@InstId is not null)
 begin
   declare @GUID uniqueIdentifier
   set @GUID = NEWID()
   insert into tblResetPasswordForInstructors values(@GUID , @InstId,GETDATE())
   select 1 as returnValue , @GUID as uniqueId ,@Email as [Email] , @FName as FName
 end
else
  begin
   select 0 as returnValue , null as uniqueId ,null as [Email] , null as FName
 end
 end

 --*****************************ChangedPassword
 go
 create proc spChangedPassword @GUID uniqueidentifier ,@Password varchar(200)
 as
 begin
 declare @InsId int
  select @InsId =  InsId from tblResetPasswordForInstructors where Id=@GUID
  if(@InsId is null)
  begin
    select 0 as passwordchanged
  end
  else
   begin
    update Instructor set [Password]=@Password
	where InsId = @InsId
	
	Delete from tblResetPasswordForInstructors
	where Id = @GUID
	
	select 1 as  passwordchanged
   end
 end

--***************Log In*******************--
go
alter proc LoginInstructor @Email varchar(300),@Password varchar(20),
@InsId int output ,
@check bit output
                                                  

as
begin
declare @exist int 
set @InsId =(select InsId from Instructor
	           where [Email]=@Email and [Password]=@Password )

    if   (@InsId is null)
			   begin
                 set @check = 0
				
			   end
    else      
        begin                     
          set @check = 1
		  select @exist = InstructorId from tblInstructorLogin 
		  where InstructorId=@InsId
		  if(@exist is Null)
		  begin
		    insert into tblInstructorLogin values(@InsId)
          end
	    end
end


--------------EDIT---------------------
go
alter proc EditProfile @InsId int ,@FName nvarchar(50) ,@LName nvarchar(50),
@Email varchar(300) , @Password varchar(20),@InsImage nvarchar(max),@Url varchar(max)
as
begin
Update Instructor set FName=@FName ,LName = @LName,
Email= @Email,[Password]= @Password,InsImage= @InsImage ,Url=@Url
where @InsId = InsId
end

------------courses of an instructor -----------
go
alter proc CoursesOfOneInstructor 
@InsId int ,
@index int 
as
begin
 with courses  as (select * ,row_number() over(order by ID) as rownum from dbo.Course   where Course.InsId = @InsId )
 select * from courses where rownum between ((@index*12)+1) and (12*(@index+1))
end



----------------------------
go
Create proc CoursesOfOneInstructor2 
@InsId int 
as
begin
  select * from Course
end



-----------------count of courses------------
go
alter proc countOfCourses 
@InsId int ,
@count1 int output
as
begin 

set @count1 = (select count(ID) from Course where Course.InsId =@InsId)
 
end

declare @count1 int
exec countOfCourses 1 ,@count1 output
select @count1
-------------------------------------------------------
-------------------------COURSE------------------------
-------------------------------------------------------
create table Course (ID int identity(1,1) primary key not null ,
CourseId int not null unique ,CourseTitle nvarchar(200) not null , CourseDescription nvarchar(max) not null ,
CourseImage nvarchar(max) not null ,InsId int foreign key references Instructor (InsId))

alter table Course add CategoryId int foreign key references Category(CategoryId),
Duration decimal not null , StartCourse date not null , EndCourse date not null , [Status] nvarchar(200) not null
alter table Course add Price decimal not null
alter table Course add [Level] int not null
alter table Course drop column CourseId

----------------------------------Course Procedure--------------------
go
create proc AddCourse @CourseTitle nvarchar(200), @CourseDescription nvarchar(max)  ,
@CourseImage nvarchar(max),@InsId int,@CategoryId int,@Duration decimal ,
 @StartCourse date , @EndCourse date  ,@Price decimal,@Level int 
 as
 begin
 insert into Course values( @CourseTitle , @CourseDescription ,
@CourseImage ,@InsId ,@CategoryId ,@Duration ,
 @StartCourse , @EndCourse ,@Price ,@Level 
 )
 end
 ---------------------Retrive All Courses---------------
 go
 create proc AllCourses 
 as
 begin
  select * from Course
  end
  --------------------delete course----------
  go
  alter proc DeleteCourse @id int
  as
  begin 
  delete from Lesson where Lesson.ID=@id
  delete from Course where ID=@id
  end
 
    --------------------delete all courses----------
  go
  create proc DeleteAllCourses
  as
  begin 
  delete from Course 
  end
    --------------------Editcourse----------
  go
  create proc EditCourse @ID int, @CourseTitle nvarchar(200) , @CourseDescription nvarchar(max)  ,
@CourseImage nvarchar(max), @Duration decimal , @StartCourse date , @EndCourse date 
,@Price decimal ,@Level int 
  as
  begin 
  Update  Course set 
  CourseTitle = @CourseTitle,CourseDescription= @CourseDescription ,
CourseImage=@CourseImage ,Duration= @Duration  , StartCourse= @StartCourse  ,EndCourse= @EndCourse 
,Price= @Price ,[Level] =@Level
  where ID=@ID
  end
  -------------------------get course by id------------------
  go
  create proc GetCourseById @courseId int
  as
  begin
  select * from Course where ID = @courseId
  end
--------------------------------------------------------
-------------------------LESSON-------------------------
--------------------------------------------------------

create table Lesson (LessonId int identity(1,1) primary key not null ,
LessonTitle nvarchar(max) not null , video nvarchar(max) null ,
VideoSize int null , TextContent nvarchar(max) null, TextContentSize int null,
ID int foreign key references Course (ID))

alter table Lesson Add LessonImage nvarchar(max) not null
-------------------Add Lesson--------------------
go
create proc AddLesson @LessonTitle nvarchar(max) , @video nvarchar(max) ,
@VideoSize int , @TextContent nvarchar(max), @TextContentSize int ,
@ID int ,@LessonImage nvarchar(max) 
as
begin
  insert into Lesson Values(@LessonTitle, @video,
@VideoSize, @TextContent, @TextContentSize,
@ID,@LessonImage)
end
-------------------------------
go
create proc LessonesOfOneCourse 
@CourseId int ,
@index int 
as
begin
 with Lessones  as (select * ,row_number() over(order by ID) as rownum from dbo.Lesson   where Lesson.ID = @CourseId )
 select * from Lessones where rownum between ((@index*12)+1) and (12*(@index+1))
end


exec LessonesOfOneCourse 4,0 

select * from Course
update Lesson set video='hyaa.mp4' where LessonId = 1

select *from Instructor
----------------------
go
create proc countOfLessons 
@courseId int ,
@count int output
as 
begin
  set @count =(select count(LessonId) from Lesson where Lesson.ID=@courseId)
end

-----------------delete lesson--------
 go
alter proc DeleteLesson @id int
  as
  begin 
  delete from Lesson where Lesson.LessonId=@id
  
  end
 
   
    --------------------Editlesson----------
  go
  alter proc EditLesson @LessonId int  ,
@LessonTitle nvarchar(max) , @video nvarchar(max)  ,
@VideoSize int , @TextContent nvarchar(max), @TextContentSize int,@ID int,
@LessonImage nvarchar(max)
  as
  begin 
  Update Lesson set LessonTitle= @LessonTitle , video= @video,
VideoSize= @VideoSize ,TextContent= @TextContent ,TextContentSize= @TextContentSize,
ID=@ID,LessonImage= @LessonImage 
  
  where LessonId=@LessonId
  end




  -------------------------get Lesson by id------------------
  go
alter proc GetLessonById @LessonId int
  as
  begin
  select * from Lesson where LessonId = @LessonId
  end
-------------------------------------------------------
-------------------------Quize-------------------------
-------------------------------------------------------

create table Quize (QuizeId int identity(1,1) primary key not null ,
Title nvarchar(100) not null , ID int foreign key references Course (ID),
InsId int foreign key references Instructor(InsId))

---------------------------------------------------------
------------------------Questions------------------------
---------------------------------------------------------

create table Question (QuestionId int primary key identity(1,1) not null,
Question nvarchar(max) not null , ID int foreign key references Course(Id)) 

---------------------------------------------------------
------------------------Quize Questions------------------
---------------------------------------------------------

create table QuizeQuestions (Id int primary key not null identity(1,1),
QuestionId  int foreign key references Question(QuestionId) ,QuizeId int foreign key references Quize(QuizeId))

-------------------------------------------------------
-------------------------Category--------------------
-------------------------------------------------------
create table Category (CategoryId int Primary Key identity(1,1) not null ,
CategoryName nvarchar(20) not null unique , [Description] nvarchar(max) not null,
CategoryImage nvarchar(max)  not null)


insert into Category values ('Design','very benifittttttttttt','Design.jpg'),('Development','jgxfgkhgdxjhftrdj','Development.jpg'),('Photography','kljkhcx','photography.jpg')

go
create proc AllCategory
as
begin 
select * from Category
end

---------------------



 ---------------------------------------
 create table Message (MessageId int identity(1,1) primary key , 
 [Text] nvarchar(max) not null , TimeSend datetime , InstructorSender int foreign key references Instructor(InsId),
 InstructorReciver int foreign key references Instructor(InsId) , SenderFName nvarchar(50),
 RecieverFName nvarchar(50)) 


 alter table Message add [Type] varchar(100)



 go
alter proc AddMessage @Text nvarchar(max) , @TimeSend datetime , @InstructorSender int ,
 @InstructorReciver int , @SenderFName nvarchar(50),
 @RecieverFName nvarchar(50),@From varchar(200),@Type varchar(100)
 as
 begin
   insert into Message values(@Text, @TimeSend, @InstructorSender,
 @InstructorReciver, @SenderFName,
 @RecieverFName,@From,@Type)
 end


 ----------
 go
alter proc GetAllMessage
@InstructorSender int,
 @InstructorReciver int

 as
 begin
  select * from Message where(
 InstructorSender = @InstructorSender
 or
  InstructorSender= @InstructorReciver)
  and (
 InstructorReciver= @InstructorReciver 
 or 
 InstructorReciver= @InstructorSender) 
  
 order by (TimeSend)
 end

-----------------------Search--------------------
go

alter proc SearchByName @Type varchar(100),@Name varchar(300)
as
begin 

   if(@Type='Instructor')
	 begin
	    select * from Instructor
	    where FName like '%'+@Name+'%' or LName like '%'+@Name+'%'
	 end
end

-------------------------------
create table tblInstructorLogin (Id int primary key identity(1,1) not null , InstructorId int unique foreign key references Instructor 
(InsId))

 
select * from  tblInstructorLogin 
exec getInstructorById 1017


go
create proc Logout @InsId int 
as
begin
   delete from tblInstructorLogin where
   InstructorId=@InsId
end



delete from [Instructor]


select * from Message

