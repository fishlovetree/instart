xunit����nuget����xunit��Shouldly��xunit.runner.visualstudio

���ݿⴴ����sql:
CREATE TABLE [dbo].[User] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [UserName]   NVARCHAR (50) NOT NULL,
    [Password]   NVARCHAR (50) NOT NULL,
    [Role]       INT           DEFAULT ((0)) NOT NULL,
    [Status]     INT           DEFAULT ((1)) NOT NULL,
    [CreateTime] DATETIME      NOT NULL,
    [ModifyTime] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


