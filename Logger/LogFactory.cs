namespace Logger;


/*
The `LogFactory` should be updated with a new method `ConfigureFileLogger`. This should take in a file path and store it in a **private member**. It should use this when instantiating a new `FileLogger` in its `CreateLogger` method. ❌✔

    - If the file logger has not be configured in the `LogFactory`, its `CreateLogger` method should return `null`. ❌✔
    - Inside of `BaseLoggerExtensions` implement **extension methods** on `BaseLogger` for
    - `Error`, ❌✔
    - `Warning`, ❌✔
    - `Information`, and ❌✔
    - `Debug`. ❌✔
  
  Each of these methods should take in a `string` for the message, as well as a **parameter array** of arguments for the message. Each of these extension methods is expected to be a shortcut for calling the `BaseLogger.Log` method, by automatically supplying the appropriate `LogLevel`. These methods should throw an exception if the `BaseLogger` parameter is null. There are a couple example unit tests to get you started.
*/

public class LogFactory
{
    private string? _filePath;
    /// <summary>
    /// Creates a new instance of a logger for the specified class name. If the file logger has not been configured, it returns null.   
    /// </summary>
    /// <param name="className">
    /// <returns> a "FileLogger" object>
    public BaseLogger? CreateLogger(string className) // initialize classname at logger creation
    {
        if (string.IsNullOrEmpty(_filePath))
        {
            return null;
        }

        return new FileLogger(_filePath) { ClassName = className };
    }


    /// <summary>
    /// Configures the file logger with the specified file path. This method should be called before creating any loggers.
    /// </summary>
    /// <param name="filePath"></param>
    public void ConfigureFileLogger(string filePath)
    {
        _filePath = filePath;
    }
}
