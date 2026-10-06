using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Logger.Tests;

[TestClass]
public class FileLoggerTests
{
    private string _testFilePath;

    [TestInitialize]
    public void Setup()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), $"test_log_{Guid.NewGuid()}.txt");
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [TestMethod]
    public void FileLogger_Constructor_InitializesSuccessfully()
    {
        // Arrange
        var filePath = _testFilePath;

        // Act
        var logger = new FileLogger(filePath);

        // Assert
        Assert.IsNotNull(logger);
    }

    [TestMethod]
    public void FileLogger_Log_AppendsMessageToFile()
    {
        // Arrange
        var filePath = _testFilePath;
        var logger = new FileLogger(filePath) { ClassName = nameof(FileLoggerTests) };
        var message = "Test message";

        // Act
        logger.Log(LogLevel.Information, message);

        // Assert
        Assert.IsTrue(File.Exists(filePath));
        var content = File.ReadAllText(filePath);
        Assert.IsTrue(content.Contains("FileLoggerTests"));
        Assert.IsTrue(content.Contains("Information"));
        Assert.IsTrue(content.Contains(message));
    }

    [TestMethod]
    public void FileLogger_Log_AppendsMultipleMessagesOnSeparateLines()
    {
        // Arrange
        var filePath = _testFilePath;
        var logger = new FileLogger(filePath) { ClassName = nameof(FileLoggerTests) };

        // Act
        logger.Log(LogLevel.Error, "First message");
        logger.Log(LogLevel.Warning, "Second message");
        logger.Log(LogLevel.Information, "Third message");

        // Assert
        var lines = File.ReadAllLines(filePath);
        Assert.AreEqual(3, lines.Length);
        Assert.IsTrue(lines[0].Contains("Error"));
        Assert.IsTrue(lines[0].Contains("First message"));
        Assert.IsTrue(lines[1].Contains("Warning"));
        Assert.IsTrue(lines[1].Contains("Second message"));
        Assert.IsTrue(lines[2].Contains("Information"));
        Assert.IsTrue(lines[2].Contains("Third message"));
    }

    [TestMethod]
    public void FileLogger_Log_IncludesDateTime()
    {
        // Arrange
        var filePath = _testFilePath;
        var logger = new FileLogger(filePath) { ClassName = nameof(FileLoggerTests) };

        // Act
        var beforeTime = DateTime.Now;
        logger.Log(LogLevel.Debug, "Test");
        var afterTime = DateTime.Now;

        // Assert
        var content = File.ReadAllText(filePath);
        Assert.IsTrue(content.Length > 0);
        // Verify that the content starts with a date/time pattern (year should be present)
        Assert.IsTrue(content.Contains(beforeTime.Year.ToString()));
    }

    [TestMethod]
    public void FileLogger_Log_WithDifferentLogLevels()
    {
        // Arrange
        var filePath = _testFilePath;
        var logger = new FileLogger(filePath) { ClassName = nameof(FileLoggerTests) };

        // Act
        logger.Log(LogLevel.Error, "Error message");
        logger.Log(LogLevel.Warning, "Warning message");
        logger.Log(LogLevel.Information, "Info message");
        logger.Log(LogLevel.Debug, "Debug message");

        // Assert
        var content = File.ReadAllText(filePath);
        Assert.IsTrue(content.Contains("Error"));
        Assert.IsTrue(content.Contains("Warning"));
        Assert.IsTrue(content.Contains("Information"));
        Assert.IsTrue(content.Contains("Debug"));
    }
}
