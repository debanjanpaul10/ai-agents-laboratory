CREATE TABLE [dbo].[RegisteredApplications]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
  [ApplicationName] NVARCHAR(100) NOT NULL,
  [Description] NVARCHAR(MAX) NOT NULL,
  [ApplicationRegistrationGuid] UNIQUEIDENTIFIER NULL,
  [IsAzureRegistered] BIT NOT NULL DEFAULT 0,
  [DateCreated] DATETIME NOT NULL DEFAULT GETUTCDATE(),
  [CreatedBy] NVARCHAR(MAX) NOT NULL,
  [DateModified] DATETIME NOT NULL DEFAULT GETUTCDATE(),
  [ModifiedBy] NVARCHAR(MAX) NOT NULL,
  [IsActive] BIT NOT NULL DEFAULT 1,
)
GO;

CREATE NONCLUSTERED INDEX ix_RegisteredApplication_Id_IsActive
ON [dbo].[RegisteredApplications]([Id], [IsActive], [ApplicationRegistrationGuid])
GO;
