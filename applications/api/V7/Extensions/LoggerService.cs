using NLog;

namespace V7
{
    public class LoggerService
    {
        public class LoggerManager
        {
            private static readonly Logger logger = LogManager.GetCurrentClassLogger();

            public LoggerManager()
            {

            }
            public void LogDebug(string message)
            {
                logger.Debug(message);
            }

            public void LogError(string message)
            {
                logger.Error(message);
            }

            public void LogInfo(string message)
            {
                logger.Info(message);
            }

            public void LogWarn(string message)
            {
                logger.Warn(message);
            }
        }
    }
}
