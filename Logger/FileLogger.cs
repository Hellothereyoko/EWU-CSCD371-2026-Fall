namespace Logger;

using System;


/// <summary>
/// FileLogger is a concrete implementation of the BaseLogger class that logs messages to a specified file. It appends log messages to the file, including the current date/time, the name of the class that created the logger, the log level, and the message itself.
/// </summary>
public class FileLogger : BaseLogger
{
    private readonly string _filePath;


    /// <summary>
    /// Initializes a new instance of the FileLogger class with the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the file where log messages will be written.</param>
    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }


    /// <summary>
    /// Logs a message to the specified file, including the current date/time, the name of the class that created the logger, the log level, and the message itself. Each log entry is appended on its own line in the file.
    /// </summary>  
    public override void Log(LogLevel logLevel, string message)
    {
        var logEntry = $"{DateTime.Now} {ClassName} {logLevel}: {message}";
        System.IO.File.AppendAllText(_filePath, logEntry + Environment.NewLine);
    }
}