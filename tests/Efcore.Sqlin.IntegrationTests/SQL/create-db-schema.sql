use [master]

--create the db for test data
if not exists (select top 1 1 from sys.databases where [name] = 'testsqlin')
begin
    create database [testsqlin]
end

--switch to the new database
use [testsqlin]

--create the table to be filled with bogus data
if object_id('TestModel', 'U') is null
begin
    create table TestModel
    (
        [Id] int primary key identity(1,1),
        [Name] nvarchar(4000) not null,
        [Email] nvarchar(2000) not null
    )
end