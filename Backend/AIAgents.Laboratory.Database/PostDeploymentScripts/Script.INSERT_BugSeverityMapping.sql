-- Write your own SQL object definition here, and it'll be included in your package.
IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugSeverityMapping]
WHERE [IsActive]=1 AND [SeverityName]='High')
BEGIN
    INSERT INTO [dbo].[BugSeverityMapping]
        ([SeverityName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('High', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END

IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugSeverityMapping]
WHERE [IsActive]=1 AND [SeverityName]='Medium')
BEGIN
    INSERT INTO [dbo].[BugSeverityMapping]
        ([SeverityName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('Medium', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END

IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugSeverityMapping]
WHERE [IsActive]=1 AND [SeverityName]='Low')
BEGIN
    INSERT INTO [dbo].[BugSeverityMapping]
        ([SeverityName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('Low', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END

IF NOT EXISTS (SELECT TOP 1
    1
FROM [dbo].[BugSeverityMapping]
WHERE [IsActive]=1 AND [SeverityName]='NA')
BEGIN
    INSERT INTO [dbo].[BugSeverityMapping]
        ([SeverityName], [IsActive], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy])
    VALUES
        ('NA', 1, GETUTCDATE(), 'System', GETUTCDATE(), 'System')
END