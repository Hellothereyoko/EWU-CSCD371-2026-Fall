using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Logger.Tests;

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
    [TestMethod]
    public void Information_WithData_LogsMsg()
    {
        var logger = new TestLogger();

        logger.Information("Message {0}", 42);

        Assert.HasCount(1, logger.LoggedMessages);
        Assert.AreEqual(LogLevel.Information, logger.LoggedMessages[0].LogLevel);
        Assert.AreEqual("Message 42", logger.LoggedMessages[0].Message);

    }

    [TestMethod]
    public void Information_WithNoArgs_DoesNotFormat()
    {
        var logger = new TestLogger();

        logger.Information("Use {0}");

        Assert.AreEqual("Use {0}", logger.LoggedMessages[0].Message);
    }
    [TestMethod]
    public void Error_WithNullMsg_ThrowsException()
    {
        TestLogger logger = new TestLogger();

        void NullAct()
        {
            logger.Error(null);
        }
        Assert.Throws<ArgumentNullException>(NullAct);
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
