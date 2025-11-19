-- Write your own SQL object definition here, and it'll be included in your package.
IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugItemStatusMapping]
WHERE [IsActive]=1 AND [StatusName]='Not Started')
BEGIN
    INSERT INTO [dbo].[BugItemStatusMapping]
        ([StatusName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('Not Started', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END

IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugItemStatusMapping]
WHERE [IsActive]=1 AND [StatusName]='Active')
BEGIN
    INSERT INTO [dbo].[BugItemStatusMapping]
        ([StatusName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('Active', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END

IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugItemStatusMapping]
WHERE [IsActive]=1 AND [StatusName]='Resolved')
BEGIN
    INSERT INTO [dbo].[BugItemStatusMapping]
        ([StatusName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('Resolved', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END

IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugItemStatusMapping]
WHERE [IsActive]=1 AND [StatusName]='Closed')
BEGIN
    INSERT INTO [dbo].[BugItemStatusMapping]
        ([StatusName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('Closed', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END