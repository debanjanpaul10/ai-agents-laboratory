CREATE TABLE [dbo].[BugReportDetails]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
  [Title] NVARCHAR(100) NOT NULL,
  [Description] NVARCHAR(MAX) NOT NULL,
  [BugSeverityId] INT NOT NULL,
  [BugStatusId] INT NOT NULL,
  [AgentDetails] NVARCHAR(MAX) NULL,
  [DateCreated] DATETIME NOT NULL DEFAULT GETUTCDATE(),
  [CreatedBy] NVARCHAR(MAX) NOT NULL,
  [DateModified] DATETIME NOT NULL DEFAULT GETUTCDATE(),
  [ModifiedBy] NVARCHAR(MAX) NOT NULL,
  [IsActive] BIT NOT NULL DEFAULT 1,
  CONSTRAINT [FK_BugReportDetails_BugSeverityMapping] FOREIGN KEY ([BugSeverityId]) REFERENCES [dbo].[BugSeverityMapping]([Id]),
  CONSTRAINT [FK_BugReportDetails_BugItemStatusMapping] FOREIGN KEY ([BugStatusId]) REFERENCES [dbo].[BugItemStatusMapping]([Id])
)
GO;

CREATE NONCLUSTERED INDEX ix_BugReportDetails_Id_BugSeverityId_BugStatusId_IsActive ON [dbo].[BugReportDetails]([Id], [BugSeverityId], [BugStatusId], [IsActive])
GO;
