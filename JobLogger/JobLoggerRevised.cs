//Code Review
//Please review the following code snippet.Assume that all referenced assemblies have
//been properly included.
//The code is used to log different messages throughout an application. We want the
//ability to be able to log to a text file, the console and/or the database.Messages can be
//marked as message, warning or error.We also want the ability to selectively be able to
//choose what gets logged, such as to be able to log only errors or only errors and
//warnings.
//1) If you were to review the following code, what feedback would you give? Please
//be specific and indicate any errors that would occur as well as other best practices
//and code refactoring that should be done.
//2) Rewrite the code based on the feedback you provided in question 1. Please
//include unit tests on your code.

namespace JobLogger
{
    using Exceptions;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;

    public class JobLogger
    {

        public bool LogToFile { get; set; }
        public bool LogToConsole { get; set; }
        public bool LogToDatabase { get; set; }
        public LogType LogType { get; set; }

        private static JobLogger _jobLogger;

        private JobLogger()
        {
        }

        public void ConfigurateLog(bool logToFile, bool logToConsole, bool logToDatabase, LogType logType)
        {
            LogToDatabase = logToDatabase;
            LogToFile = logToFile;
            LogToConsole = logToConsole;
            LogType = logType;
        }

        public static JobLogger GetJobLogger()
        {
            return _jobLogger ?? (_jobLogger = new JobLogger());
        }

        public void LogMessage(string message, MessageType messageType)
        {

            if (string.IsNullOrEmpty(message))
                throw new JobLoggerException("Message is empty");

            if (!LogToConsole && !LogToFile && !LogToDatabase)
                throw new JobLoggerException("Invalid output configuration, please set at least an output");

            if (!ShouldLog(messageType))
                throw new JobLoggerException("The message could not be logged");

            var trim = message.Trim();

            if (LogToConsole)
            {
                Console.ForegroundColor = GetColor(messageType);
                Console.WriteLine(DateTime.Now.ToShortDateString() + trim);
            }
            if (LogToFile)
            {
                var logFileDirectory = ConfigurationManager.AppSettings["LogFileDirectory"];
                if (logFileDirectory == null)
                    throw new JobLoggerException("LogFileDirectory Key is not configured");

                var path = logFileDirectory + "LogFile" +
                                 DateTime.Now.ToShortDateString() + ".txt";
                try
                {
                    using (var sw = File.AppendText(path))
                    {
                            sw.WriteLine(DateTime.Now.ToShortDateString() + trim);
                    }

                }
                catch (Exception)
                {

                    throw new JobLoggerException("The message could not be logged in the file");
                }
                
            }
            if (LogToDatabase)
            {
                LogInDb(trim, messageType);
            }

        }

        public void LogInDb(string message, MessageType type)
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            if (connectionString == null)
                throw new JobLoggerException("ConnectionString Key is not configured");

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("Insert into Log Values('" + message + "', " + type + ")");
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new JobLoggerException("Could not save the message in the DB");
                }
            }
        }

        public ConsoleColor GetColor(MessageType messageType)
        {
            if ((int) messageType >= (int) LogType.Errors)
                return ConsoleColor.Red;
            return (int) messageType >= (int) LogType.ErrorsWarning ? ConsoleColor.Yellow : ConsoleColor.White;
        }

        public bool ShouldLog(MessageType messageType)
        {
            return (int) messageType >= (int) LogType;
        }
    }
}