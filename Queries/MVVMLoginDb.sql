create database MVVMLoginDb
go

use MVVMLoginDb
go

create table [User]
(
	Id  UNIQUEIDENTIFIER primary Key default NEWID(),
	Username nvarchar (50) unique not null,
	[Password] nvarchar (50) unique not null,
	[Name] nvarchar (50) unique not null,
	LastName nvarchar (50) unique not null,
	Email nvarchar (100) unique not null,
)
go

insert into [User] values (NEWID(), 'admin1', 'admin1', 'Stacey', 'Gonzaga', 'staceyGonzaga@gmail.com')
insert into [User] values (NEWID(), 'admin2', 'admin2', 'Nathan', 'Rijndorp', 'nathanRijndorp@gmail.com')
insert into [User] values (NEWID(), 'admin3', 'admin3', 'Jaen', 'Toyoda', 'jaenToyoda@gmail.com')
go