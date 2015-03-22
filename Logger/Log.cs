using System;
using System.IO;
using log4net;
using log4net.Config;

namespace Logger
{
    /// <summary>
    /// The static logging class.
    /// </summary>
    public static class Log
    {
        private static ILog _log;

        /// <summary>
        /// The log4net configuration file path.
        /// </summary>
        public static FileInfo Config { get; private set; }

        /// <summary>
        /// The log file path.
        /// </summary>
        public static FileInfo Path { get; private set; }

        /// <summary>
        /// Creates the logger for logging.
        /// </summary>
        /// <param name="configFilePath">The log4net configuration file path.</param>
        /// <param name="logFilePath">The log file path.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Create(FileInfo configFilePath, FileInfo logFilePath)
        {
            if (configFilePath == null)
            {
                throw new ArgumentNullException("configFilePath", "The config file path is null");
            }

            if (logFilePath == null)
            {
                throw new ArgumentNullException("logFilePath", "The log file path is null");
            }

            GlobalContext.Properties["logDirectory"] = logFilePath.DirectoryName;
            GlobalContext.Properties["logName"] = logFilePath.Name;

            Config = configFilePath;
            Path = logFilePath;
        }

        /// <summary>
        /// Stops logging.
        /// </summary>
        public static void Stop()
        {
            if (_log != null)
            {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Starts logging. Note: Call Create before starting logging to initialize the logger.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Start()
        {
            if (Config == null)
            {
                throw new Exception("The config file path is null. Please call Log.Create to initialize logging.");
            }

            if (Path == null)
            {
                throw new Exception("The log file path is null. Please call Log.Create to initialize logging.");
            }

            XmlConfigurator.ConfigureAndWatch(Config);
            _log = LogManager.GetLogger(Path.Name);
        }

        /// <summary>
        /// Creates the logger and starts logging.
        /// </summary>
        /// <param name="configFilePath">The log4net configuration file path.</param>
        /// <param name="logFilePath">The log file path.</param>
        public static void CreateAndStart(FileInfo configFilePath, FileInfo logFilePath)
        {
            Create(configFilePath, logFilePath);
            Start();
        }

        private static void SendMessage(Action<object> logAction, Action<object, Exception> logActionWithException,
            string formatString, Exception exception, params object[] args)
        {
            string message = string.Format(formatString, args);
            if (exception != null && logActionWithException != null)
            {
                logActionWithException(message, exception);
            }
            else if (logAction != null)
            {
                logAction(message);
            }
        }

        // Default message sender (aka Console).
        private static void SendMessage(string formatString, Exception exception, params object[] args)
        {
            SendMessage(Console.WriteLine, (o, e) => Console.WriteLine("{0} [Exception: {1}]", o, e), formatString, exception, args);
        }

        /// <summary>
        /// Sends a Debug message to the logger including an exception to be logged.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="exception">If an exception is given, it will be printed with the log message.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Debug(string formatString, Exception exception, params object[] args)
        {
            if (_log != null)
            {
                SendMessage(_log.Debug, _log.Debug, formatString, exception, args);
            }
            else
            {
                SendMessage(formatString, exception, args);
            }
        }

        /// <summary>
        /// Sends a Debug message to the logger.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Debug(string formatString, params object[] args)
        {
            Debug(formatString, null, args);
        }

        /// <summary>
        /// Sends a Error message to the logger including an exception to be logged.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="exception">If an exception is given, it will be printed with the log message.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Error(string formatString, Exception exception, params object[] args)
        {
            if (_log != null)
            {
                SendMessage(_log.Error, _log.Error, formatString, exception, args);
            }
            else
            {
                SendMessage(formatString, exception, args);
            }
        }

        /// <summary>
        /// Sends a Error message to the logger.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Error(string formatString, params object[] args)
        {
            Error(formatString, null, args);
        }

        /// <summary>
        /// Sends a Fatal message to the logger including an exception to be logged.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="exception">If an exception is given, it will be printed with the log message.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Fatal(string formatString, Exception exception, params object[] args)
        {
            if (_log != null)
            {
                SendMessage(_log.Fatal, _log.Fatal, formatString, exception, args);
            }
            else
            {
                SendMessage(formatString, exception, args);
            }
        }

        /// <summary>
        /// Sends a Fatal message to the logger.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Fatal(string formatString, params object[] args)
        {
            Fatal(formatString, null, args);
        }

        /// <summary>
        /// Sends a Info message to the logger including an exception to be logged.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="exception">If an exception is given, it will be printed with the log message.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Info(string formatString, Exception exception, params object[] args)
        {
            if (_log != null)
            {
                SendMessage(_log.Info, _log.Info, formatString, exception, args);
            }
            else
            {
                SendMessage(formatString, exception, args);
            }
        }

        /// <summary>
        /// Sends a Info message to the logger.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Info(string formatString, params object[] args)
        {
            Info(formatString, null, args);
        }

        /// <summary>
        /// Sends a Warn message to the logger including an exception to be logged.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="exception">If an exception is given, it will be printed with the log message.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Warn(string formatString, Exception exception, params object[] args)
        {
            if (_log != null)
            {
                SendMessage(_log.Warn, _log.Warn, formatString, exception, args);
            }
            else
            {
                SendMessage(formatString, exception, args);
            }
        }

        /// <summary>
        /// Sends a Warn message to the logger.
        /// </summary>
        /// <param name="formatString">The formatted message string.</param>
        /// <param name="args">The arguments for the formatted message string.</param>
        public static void Warn(string formatString, params object[] args)
        {
            Warn(formatString, null, args);
        }
    }
}