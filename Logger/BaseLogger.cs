namespace Logger;


/*
 * BaseLogger is an abstract class that defines the basic logging functionality.
 * It provides a method to log messages with a specified log level.
 *
 * 
 * There is an existing `BaseLogger` class. It needs an **auto property** to store class name -  the name of the class that calls the `LogFactory`'s `CreateLogger()` method. This class (that calls the `CreateLogger()` method) will identify its name using nameof(), as specified below.) This property should be set in the `LogFactory` using an **object initializer**. ❌✔
  
    - Create a `FileLogger` that derives from `BaseLogger`. It should take in a path to a file to write the log message to. When its `Log` method is called, it should **append** messages on their own line in the file. The output should include all of the following:
    - The current date/time ❌✔
    - The name of the class that created the logger ❌✔
    - The log level ❌✔
    - The message ❌✔
    - The format may vary, but an example might look like this "10/7/2019 12:38:59 AM FileLoggerTests Warning: Test message"


 */
public abstract class BaseLogger
{

    /*
     * Log is an abstract method that must be implemented by derived classes.
     * It takes in a log level and a message to log.
     */
    public abstract void Log(LogLevel logLevel, string message);


        //auto property to get the log level of the logger.
            // public LogLevel LogLevel { get; set; } = LogLevel.Info;

        //Call upon the create logger function in LogFactory to create a logger for the specified class name.
            // public static BaseLogger CreateLogger(string className);
}

