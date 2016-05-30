if exists (select * from sysobjects where id = OBJECT_ID('[Person]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Person]

CREATE TABLE [Person] (
[Id] [int]  IDENTITY (1, 1)  NOT NULL,
[FirstName] [nvarchar]  (50) NULL,
[LastName] [nvarchar]  (50) NULL,
[Active] [bit]  NULL,
[Sex] [int]  NULL,
[CrteatDate] [datetime]  NULL)