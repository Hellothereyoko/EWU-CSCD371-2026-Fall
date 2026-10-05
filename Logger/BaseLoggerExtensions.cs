using System;
namespace Logger;


/*Inside of `BaseLoggerExtensions` implement **extension methods** on `BaseLogger` for
  - `Error`, ❌✔
  - `Warning`, ❌✔
  - `Information`, and ❌✔
  - `Debug`. ❌✔
  Each of these methods should take in a `string` for the message, as well as a **parameter array** of arguments for the message. Each of these extension methods is expected to be a shortcut for calling the `BaseLogger.Log` method, by automatically supplying the appropriate `LogLevel`. These methods should throw an exception if the `BaseLogger` parameter is null. There are a couple example unit tests to get you started.
  */

public static class BaseLoggerExtensions
{
  public static void Error(this BaseLogger logger, string message, params object[] args)
  {
    ArgumentNullException.ThrowIfNull(logger);
    var formatted = (args != null && args.Length > 0) ? string.Format(message, args) : message;
    logger.Log(LogLevel.Error, formatted);
  }

  public static void Warning(this BaseLogger logger, string message, params object[] args)
  {
    ArgumentNullException.ThrowIfNull(logger);
    var formatted = (args != null && args.Length > 0) ? string.Format(message, args) : message;
    logger.Log(LogLevel.Warning, formatted);
  }

  public static void Information(this BaseLogger logger, string message, params object[] args)
  {
    ArgumentNullException.ThrowIfNull(logger);
    var formatted = (args != null && args.Length > 0) ? string.Format(message, args) : message;
    logger.Log(LogLevel.Information, formatted);
  }

  public static void Debug(this BaseLogger logger, string message, params object[] args)
  {
    ArgumentNullException.ThrowIfNull(logger);
    var formatted = (args != null && args.Length > 0) ? string.Format(message, args) : message;
    logger.Log(LogLevel.Debug, formatted);
  }

}
