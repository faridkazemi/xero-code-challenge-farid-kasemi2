using Microsoft.Extensions.Logging;

namespace RefactorThis
{
    public class ApplicationLogger
    {
        private ILogger _logger;
        public ApplicationLogger(ILogger logger)
        {
            _logger = logger;
        }
    }
}
