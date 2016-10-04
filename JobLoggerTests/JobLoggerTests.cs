namespace JobLoggerTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using JobLogger;
    using JobLogger.Exceptions;

    [TestClass]
    public class JobLoggerTests
    {
        private JobLogger _jobLogger;

        [TestInitialize]
        public void Initialize()
        {
            _jobLogger= JobLogger.GetJobLogger();
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerException),"Message is empty")]
        public void When_LogMessage_EmptyMessage_Throw_JobLoggerException()
        {

            _jobLogger.LogMessage(String.Empty, MessageType.Warning);

        }
        [TestMethod]
        [ExpectedException(typeof(JobLoggerException), "Invalid output configuration, please set at least an output")]
        public void When_LogMessage_NotConfigure_Throw_JobLoggerException()
        {
            _jobLogger.LogMessage("Warning01", MessageType.Warning);

        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerException), "The message could not be logged")]
        public void When_LogMessage_MessageType_lessThanLogType_Error_Throw_JobLoggerException()
        {
            _jobLogger.LogMessage("Warning01", MessageType.Warning);

        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerException), "The message could not be logged")]
        public void When_LogMessage_MessageType_lessThanLogType_ErrorsWarning_Throw_JobLoggerException()
        {
            _jobLogger.LogMessage("Warning01", MessageType.Message);

        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerException), "ConnectionString Key is not configured")]
        public void When_LogInDB_connectionString_Is_null_Throw_JobLoggerException()
        {
            _jobLogger.LogInDb("Warning01", MessageType.Message);

        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerException), "LogFileDirectory Key is not configured")]
        public void When_LogMessage_LogFileDirectory_Is_null_Throw_JobLoggerException()
        {
            _jobLogger.LogMessage("Warning01", MessageType.Message);

        }
        

        [TestMethod]
        public void When_ShouldLog_MessageType_Message_LogType_All_Returns_True()
        {
            _jobLogger.ConfigurateLog(false, false, false, LogType.All);
            var shouldLogResult = _jobLogger.ShouldLog(MessageType.Message);
            Assert.IsTrue(shouldLogResult);
        }

        [TestMethod]
        public void When_ShouldLog_MessageType_Message_LogType_Errors_Returns_False()
        {
            _jobLogger.ConfigurateLog(false, false, false, LogType.Errors);
            var shouldLogResult = _jobLogger.ShouldLog(MessageType.Message);
            Assert.IsFalse(shouldLogResult);
        }

        [TestMethod]
        public void When_GetColor_MessageType_Error_LogType_Errors_Returns_Red()
        {
            _jobLogger.ConfigurateLog(false, false, false, LogType.Errors);
            var getColorResult = _jobLogger.GetColor(MessageType.Error);

            Assert.IsInstanceOfType(getColorResult, typeof(ConsoleColor));
            Assert.IsTrue(getColorResult.Equals(ConsoleColor.Red));
        }

        [TestMethod]
        public void When_GetColor_MessageType_Error_LogType_ErrorsWarning_Returns_Red()
        {
            _jobLogger.ConfigurateLog(false, false, false, LogType.ErrorsWarning);
            var getColorResult = _jobLogger.GetColor(MessageType.Error);

            Assert.IsInstanceOfType(getColorResult, typeof(ConsoleColor));
            Assert.IsTrue(getColorResult.Equals(ConsoleColor.Red));
        }

        [TestMethod]
        public void When_GetColor_MessageType_Warning_LogType_ErrorsWarning_Returns_Yellow()
        {
            _jobLogger.ConfigurateLog(false, false, false, LogType.ErrorsWarning);
            var getColorResult = _jobLogger.GetColor(MessageType.Warning);

            Assert.IsInstanceOfType(getColorResult, typeof(ConsoleColor));
            Assert.IsTrue(getColorResult.Equals(ConsoleColor.Yellow));
        }

        [TestMethod]
        public void When_GetColor_MessageType_Message_LogType_ErrorsWarning_Returns_White()
        {
            _jobLogger.ConfigurateLog(false, false, false, LogType.ErrorsWarning);
            var getColorResult = _jobLogger.GetColor(MessageType.Message);

            Assert.IsInstanceOfType(getColorResult, typeof(ConsoleColor));
            Assert.IsTrue(getColorResult.Equals(ConsoleColor.White));
        }


    }
}
