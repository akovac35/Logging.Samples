// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;

namespace Shared.Mocks
{
    public class BusinessLogicMock<T>
    {
        public BusinessLogicMock(ILogger<BusinessLogicMock<T>> logger = null)
        {
            if (logger != null) _logger = logger;
        }

        private ILogger _logger = NullLogger.Instance;

        public static Task<int> GetTaskInstance(ILoggerFactory loggerFactory)
        {
            var tmp = new BusinessLogicMock<T>(loggerFactory.CreateLogger<BusinessLogicMock<T>>());
            return tmp.FirstLevelAsync((new Random()).Next(0, 3000));
        }

        public int FirstLevel(int delay = 3000)
        {
            _logger.Here(l => l.Entering(delay));

            SecondLevel(delay);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public int SecondLevel(int delay)
        {
            _logger.Here(l => l.Entering(delay));

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public async Task<int> FirstLevelAsync(int delay = 3000)
        {
            _logger.Here(l => l.Entering(delay));

            await SecondLevelAsync(delay).ConfigureAwait(false);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public async Task<int> SecondLevelAsync(int delay)
        {
            _logger.Here(l => l.Entering(delay));

            await ThirdLevelAsync(delay).ConfigureAwait(false);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public async Task<int> ThirdLevelAsync(int delay)
        {
            _logger.Here(l => l.Entering(delay));

            await Task.Delay(delay).ConfigureAwait(false);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }
    }
}
