using Microsoft.Extensions.Logging;
using Mobile.Core.Interface;

namespace Mobile.Core.Services.LoggerService
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }
    }
}