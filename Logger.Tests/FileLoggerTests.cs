using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Globalization;

namespace Logger.Tests;

[TestClass]
public class FileLoggerTests
{
    private string _testFilePath = string.Empty;

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
        Assert.Contains("FileLoggerTests", content);
        Assert.Contains("Information", content);
        Assert.Contains(message, content);
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
        Assert.HasCount(3, lines);
        Assert.Contains("Error", lines[0]);
        Assert.Contains("First message", lines[0]);
        Assert.Contains("Warning", lines[1]);
        Assert.Contains("Second message", lines[1]);
        Assert.Contains("Information", lines[2]);
        Assert.Contains("Third message", lines[2]);
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
        Assert.IsGreaterThan(0, content.Length);
        // Verify that the content contains the year (culture-invariant)
        Assert.Contains(beforeTime.Year.ToString(CultureInfo.InvariantCulture), content);
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
        Assert.Contains("Error", content);
        Assert.Contains("Warning", content);
        Assert.Contains("Information", content);
        Assert.Contains("Debug", content);
    }

    [TestMethod]
    public void FileLogger_Constructor_ThrowsOnNullOrWhitespace()
    {
        try
        {
            _ = new FileLogger(null!);
            Assert.Fail("Expected ArgumentException for null filePath");
        }
        catch (ArgumentException)
        {
            // expected
        }

        try
        {
            _ = new FileLogger("   ");
            Assert.Fail("Expected ArgumentException for whitespace filePath");
        }
        catch (ArgumentException)
        {
            // expected
        }
    }

    [TestMethod]
    public void FileLogger_Log_WithNullClassName_WritesWithoutClass()
    {
        // Arrange
        var filePath = _testFilePath;
        var logger = new FileLogger(filePath); // ClassName not set (null)
        var message = "NoClass";

        // Act
        logger.Log(LogLevel.Information, message);

        // Assert
        Assert.IsTrue(File.Exists(filePath));
        var content = File.ReadAllText(filePath);
        Assert.Contains("Information", content);
        Assert.Contains(message, content);
        Assert.IsFalse(content.Contains(nameof(FileLoggerTests)));
    }

    [TestMethod]
    public void FileLogger_Log_NonExistentDirectory_DoesNotCreateFile()
    {
        // Arrange
        var dir = Path.Combine(Path.GetTempPath(), "nonexistent", Guid.NewGuid().ToString());
        var filePath = Path.Combine(dir, "log.txt");
        var logger = new FileLogger(filePath) { ClassName = nameof(FileLoggerTests) };

        // Act
        logger.Log(LogLevel.Debug, "Should not create file");

        // Assert - because FileLogger swallows IO exceptions, no file should exist
        Assert.IsFalse(File.Exists(filePath));
    }

  
}
