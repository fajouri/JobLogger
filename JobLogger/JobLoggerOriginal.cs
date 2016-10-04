//Code Review
//We should do a refactor to this class!!


using System;
// unnesesary usings
using System.Linq;
// unnesesary usings
using System.Text;
public class JobLoggerOriginal
{
    private static bool _logToFile;
    private static bool _logToConsole;
    private static bool _logMessage;
    private static bool _logWarning;
    private static bool _logError;
    //private variable should be name with _ and lowercase
    private static bool LogToDatabase;
    //_initialized its never used, we should eliminate it
    private bool _initialized;

    //Why set this propertys in the constructor if the method "LogMessage" is static???
    public JobLoggerOriginal(bool logToFile, bool logToConsole, bool logToDatabase, bool
    logMessage, bool logWarning, bool logError)
    {
        _logError = logError;
        _logMessage = logMessage;
        _logWarning = logWarning;
        LogToDatabase = logToDatabase;
        _logToFile = logToFile;
        _logToConsole = logToConsole;
    }

    //If you call LogMessage without creating and instance it will fail 
    // message string and bool at the same time
    public static void LogMessage(string message, bool message, bool warning, bool error)//we have to name more specific variables, now its ambiguos
    {
        //we should check if the string is null before we use it
        message.Trim();

        //ask for string null or empty
        if (message == null || message.Length == 0)
        {
            //try to change the if to avoid empty returns
            return;
        }
        if (!_logToConsole && !_logToFile && !LogToDatabase)
        {
            //we should give more datail about the error
            throw new Exception("Invalid configuration");
        }
        if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning
        && !error))
        {
            //we should give more datail about the error
            throw new Exception("Error or Warning or Message must be specified");
        }
        //we should encapsulate this or call the responsable to get the dbconection
        System.Data.SqlClient.SqlConnection connection = new
        System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        connection.Open();
        //Names should be meaningful within their context.
        int t;
        if (message && _logMessage)
        {
            t = 1;
        }
        if (error && _logError)
        {
            t = 2;
        }
        if (warning && _logWarning)
        {
            t = 3;
        }

        System.Data.SqlClient.SqlCommand command = new
        System.Data.SqlClient.SqlCommand("Insert into Log Values('" + message + "', " +t.ToString() + ")");
        command.ExecuteNonQuery();
        //vNames should be meaningful within their context.
        string l;
        //we should use the stringWritter
        if
        (!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
        {
            l =
            System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
        }
        //Finally always write the message, so we shouldn't use the if
        if (error && _logError)
        {
            l = l + DateTime.Now.ToShortDateString() + message;
        }
        if (warning && _logWarning)
        {
            l = l + DateTime.Now.ToShortDateString() + message;
        }
        if (message && _logMessage)
        {
            l = l + DateTime.Now.ToShortDateString() + message;
        }
        //we should use the stringWritter
        System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings[
        "LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", l);
        if (error && _logError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        if (warning && _logWarning)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        if (message && _logMessage)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine(DateTime.Now.ToShortDateString() + message);
    }
}