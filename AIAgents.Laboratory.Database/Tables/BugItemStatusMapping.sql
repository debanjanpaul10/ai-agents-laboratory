CREATE TABLE [dbo].[BugItemStatusMapping]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
  [StatusName] NVARCHAR(MAX) NOT NULL,
  [IsActive] BIT NOT NULL DEFAULT 1,
  [DateCreated] DATETIME NOT NULL DEFAULT GETUTCDATE(),
  [CreatedBy] NVARCHAR(MAX) NOT NULL,
  [DateModified] DATETIME NOT NULL DEFAULT GETUTCDATE(),
  [ModifiedBy] NVARCHAR(MAX) NOT NULL,
)
GO;

CREATE NONCLUSTERED INDEX ix_BugItemStatusMapping_Id_IsActive ON [dbo].[BugItemStatusMapping]([Id], [IsActive])
GO;