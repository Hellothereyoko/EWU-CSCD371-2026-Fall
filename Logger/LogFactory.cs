namespace Logger;

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
