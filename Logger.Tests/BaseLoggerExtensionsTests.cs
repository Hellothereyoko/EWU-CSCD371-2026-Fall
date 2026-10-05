using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace Logger.Tests;
/* Each of these methods should take in a `string` for the message, as well as a **parameter array** of arguments for the message. Each of these extension methods is expected to be a shortcut for calling the `BaseLogger.Log` method, by automatically supplying the appropriate `LogLevel`. These methods should throw an exception if the `BaseLogger` parameter is null. There are a couple example unit tests to get you started.
- Use the nameof() operator when identifying the class name to the logger ❌✔
- Ensure you turn on Warnings as Errors (TreatWarningsAsErrors) ✔
- Ensure that you turn on code analysis (EnableNETAnalyzers) ✔
- Ensure that you turn on CodeAnalysisTreatWarningsAsErrors ✔
- Ensure that you turn on EnforceCodeStyleInBuild ✔
- Set `LangVersion` and the `TargetFramework` to the latest released versions available (preview versions optional) ✔
- Turn on Nullability (`Nullable`) ✔
- **All of the above should be unit tested.***/
[TestClass]
public class BaseLoggerExtensionsTests
{
    [TestMethod]
    public void Error_WithNullLogger_ThrowsException()
    {
        // Arrange

        // Act
        try
        {
            BaseLoggerExtensions.Error(null, "");
            Assert.Fail("Expected ArgumentNullException");
        }
        catch (System.ArgumentNullException)
        {
            // expected
        }
    }

    [TestMethod]
    public void Error_WithData_LogsMessage()
    {
        // Arrange
        var logger = new TestLogger();

        // Act
        logger.Error("Message {0}", 42);

        // Assert
        Assert.HasCount(1, logger.LoggedMessages);
        Assert.AreEqual(LogLevel.Error, logger.LoggedMessages[0].LogLevel);
        Assert.AreEqual("Message 42", logger.LoggedMessages[0].Message);
    }

}

public class TestLogger : BaseLogger
{
    public List<(LogLevel LogLevel, string Message)> LoggedMessages { get; } = new List<(LogLevel, string)>();

    public override void Log(LogLevel logLevel, string message)
    {
        LoggedMessages.Add((logLevel, message));
    }
}