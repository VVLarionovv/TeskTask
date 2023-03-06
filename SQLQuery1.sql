create database [TestDB]
go

use [TestDB]
go
create table [dbo].[Users](
	[IDUser] [int] not null identity(1,1), 
	[FIO] [varchar](50) not null,
    [dateBirth] date not null UNIQUE,
    [Description] [varchar] (50) not null,
	[Department] [varchar] (50) not null,
	[Director] [varchar] (50) not null,
	[istheheadof] [TINYINT] not null,
	constraint [PK_User] primary key clustered
	([IDUser] ASC) on [PRIMARY]
);
select * from Users

use [TestDB]
go
create procedure [dbo].[UsersInsert] 
@FIO varchar (50),@dateBirth date, @Description varchar (50), @Department varchar (50),@Director varchar (50),@istheheadof TINYINT
as 
insert into [dbo].[Users] (FIO, dateBirth,[Description], Department, Director,istheheadof)
values (@FIO, @dateBirth, @Description, @Department, @Director, @istheheadof)
go

create procedure [dbo].[Users_Update]
@IDUser int,@FIO varchar (50),@dateBirth date, @Description varchar (50), @Department varchar (50),@Director varchar (50),@istheheadof TINYINT
as
update [dbo].[Users] set
FIO=@FIO, dateBirth=@dateBirth, [Description] = @Description, [Department]= @Department, Director=@Director, istheheadof=@istheheadof
where
IDUser=@IDUser
go

create procedure [dbo].[Users_Delete]
@IDUser [int]
as
delete from [dbo].[Users]
where
IDUser = @IDUser
go