namespace JobLogger.Exceptions
{
    using System;
 

    public class JobLoggerException:Exception
    {
        public JobLoggerException(string message) : base(message) { }
    }
}
