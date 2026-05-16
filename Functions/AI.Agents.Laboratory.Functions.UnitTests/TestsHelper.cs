namespace AI.Agents.Laboratory.Functions.UnitTests;

/// <summary>
/// Helper class for unit tests in the AI.Agents.Laboratory.Functions.UnitTests namespace, providing common test data and utilities to support the testing of various components within the project.
/// </summary>
internal static class TestsHelper
{
    /// <summary>
    /// A static readonly Guid used as a test correlation ID in unit tests.
    /// </summary>
    public static readonly Guid TestCorrelationIdGuid = Guid.NewGuid();

    /// <summary>
    /// A string used as a test exception message in unit tests to verify that exceptions are thrown and handled correctly within the components being tested.
    /// </summary>
    internal static readonly string TestExceptionMessage = "Test exception message for unit testing.";
}
